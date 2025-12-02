using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.ManagerTeacher;
using BusinessLogicLayer.Services.Interface.RoleTeacher;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleTeacher;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace BusinessLogicLayer.Services.RoleTeacher
{
    public class TeacherService : ITeacherService
    {
        private readonly IClassRepository _classRepo;
        private readonly IClassStudentRepository _classStudentRepo;
        private readonly IGradeRepository _gradeRepo;
        private readonly IClassSemesterRepository _classSemesterRepo;
        public TeacherService(
            IClassRepository classRepo,
            IClassStudentRepository classStudentRepo,
            IGradeRepository gradeRepo,
            IClassSemesterRepository classSemester)
        {
            _classRepo = classRepo;
            _classStudentRepo = classStudentRepo;
            _gradeRepo = gradeRepo;
            _classSemesterRepo = classSemester;
        }

        /// <summary>
        /// danh sách lớp của giáo viên đảm nhận
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <param name="semesterId"></param>
        /// <returns></returns>
        public Task<(IEnumerable<Class>, int)> GetTeacherClassesAsync(
            int teacherId, int page, int pageSize, string search, int? semesterId)
        {
            return _classRepo.GetTeacherClassesAsync(teacherId, page, pageSize, search, semesterId);
        }

        /// <summary>
        /// danh sách hoc sinh trong lớp
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<StudentDto>, int)> GetStudentsInClassAsync(
       int classId, int page, int size, string search)
        {
            var (students, total) = await _classStudentRepo.GetStudentsByClassAsync(classId, page, size, search);

            var dtos = students.Select(s => new StudentDto
            {
                StudentId = s.StudentId,
                StudentCode = s.StudentCode,
                FullName = s.User?.FullName,
                Email = s.User?.Email
            });

            return (dtos, total);
        }


        /// <summary>
        /// Cập nhật điểm cho từng học sinh của lớp theo môn học
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="studentId"></param>
        /// <param name="scores"></param>
        /// <returns></returns>
        public async Task<bool> UpdateGradeAsync(int classId, int studentId, Dictionary<int, double> scores)
        {
            // Lấy thông tin lớp
            var classInfo = await _classRepo.GetByIdAsync(classId);
            if (classInfo == null)
                throw new Exception("Class not found");

            int subjectId = classInfo.SubjectId;

            // Lấy ClassSemester hiện tại 
            var classSemester = await _classSemesterRepo.GetClassSemesterAsync(classId);
            if (classSemester == null)
                throw new Exception("ClassSemester not found");

            foreach (var (componentId, score) in scores)
            {
                // Kiểm tra grade hiện có
                var existingGrade = await _gradeRepo.GetSingleGradeAsync(
                    studentId,
                    classSemester.Id, // dùng ClassSemesterId
                    componentId
                );

                if (existingGrade != null)
                {
                    // Nếu đã có điểm → cập nhật
                    existingGrade.Score = score;
                    existingGrade.UpdatedAt = DateTime.Now;
                }
                else
                {
                    // Nếu chưa có → tạo mới
                    var newGrade = new StudentGrade
                    {
                        StudentId = studentId,
                        ClassId = classId,               // cần gán để thoả FK
                        ClassSemesterId = classSemester.Id, // bắt buộc phải có
                        SubjectId = subjectId,
                        GradeComponentId = componentId,
                        Score = score,
                        UpdatedAt = DateTime.Now
                    };

                    await _gradeRepo.AddOrUpdateGradeAsync(newGrade);
                }
            }

            // Lưu tất cả thay đổi 1 lần
            await _gradeRepo.SaveAsync();
            return true;
        }


        /// <summary>
        /// Thêm điểm bằng file excel
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<bool> ImportGradesAsync(int classId, IFormFile file)
        {
            // 1. Lấy thông tin lớp
            var classInfo = await _classRepo.GetByIdAsync(classId);
            if (classInfo == null)
                throw new Exception("Class not found");

            int subjectId = classInfo.SubjectId;

            // 2. Lấy ClassSemester hiện tại
            var classSemester = await _classSemesterRepo.GetClassSemesterAsync(classId);
            if (classSemester == null)
                throw new Exception("ClassSemester not found");

            // 3. Lấy danh sách GradeComponent của lớp
            var components = await _gradeRepo.GetGradeComponentsAsync(subjectId);

            // 4. Đọc file Excel
            using var package = new ExcelPackage(file.OpenReadStream());
            var ws = package.Workbook.Worksheets[0];

            for (int row = 2; row <= ws.Dimension.End.Row; row++) // bỏ qua header
            {
                if (!int.TryParse(ws.Cells[row, 1].Value?.ToString(), out int studentId))
                    continue; // bỏ qua nếu studentId không hợp lệ

                int col = 2; // cột điểm bắt đầu từ 2
                foreach (var comp in components)
                {
                    var cellValue = ws.Cells[row, col].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(cellValue) && double.TryParse(cellValue, out double score))
                    {
                        // Kiểm tra grade hiện có
                        var existingGrade = await _gradeRepo.GetSingleGradeAsync(studentId, classSemester.Id, comp.GradeComponentId);

                        if (existingGrade != null)
                        {
                            // Nếu đã có điểm → cập nhật
                            existingGrade.Score = score;
                            existingGrade.UpdatedAt = DateTime.Now;
                        }
                        else
                        {
                            // Nếu chưa có → tạo mới
                            var newGrade = new StudentGrade
                            {
                                StudentId = studentId,
                                ClassId = classId,               // cần để thoả FK
                                ClassSemesterId = classSemester.Id,
                                SubjectId = subjectId,
                                GradeComponentId = comp.GradeComponentId,
                                Score = score,
                                UpdatedAt = DateTime.Now
                            };

                            await _gradeRepo.AddOrUpdateGradeAsync(newGrade);
                        }
                    }

                    col++;
                }
            }

            // 5. Lưu tất cả thay đổi 1 lần
            await _gradeRepo.SaveAsync();
            return true;
        }





        /// <summary>
        /// Xuất điểm ra file excel
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportGradesAsync(int classId)
        {
            // Lấy thông tin lớp, các thành phần điểm và danh sách học sinh
            var classInfo = await _classRepo.GetByIdAsync(classId);
            var components = await _gradeRepo.GetGradeComponentsAsync(classInfo.SubjectId);
            var students = await _classStudentRepo.GetStudentsWithUserByClassAsync(classId);

            // Lấy tất cả điểm của lớp một lần
            var allGrades = await _gradeRepo.GetGradesByClassAsync(classId);
            // allGrades là List<Grade> có StudentId, GradeComponentId, Score

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Grades");

            // Tạo header
            ws.Cells[1, 1].Value = "Student ID";
            ws.Cells[1, 2].Value = "Full Name";

            int col = 3;
            foreach (var comp in components)
            {
                ws.Cells[1, col].Value = comp.ComponentName;
                col++;
            }

            // Điền dữ liệu từng học sinh
            int row = 2;
            foreach (var s in students)
            {
                ws.Cells[row, 1].Value = s.StudentId;
                ws.Cells[row, 2].Value = s.User.FullName;

                col = 3;
                foreach (var comp in components)
                {
                    var grade = allGrades
                        .FirstOrDefault(g => g.StudentId == s.StudentId && g.GradeComponentId == comp.GradeComponentId);
                    ws.Cells[row, col].Value = grade?.Score ?? 0;
                    col++;
                }

                row++;
            }

            // Tự động căn chỉnh cột
            ws.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }




        /// <summary>
        /// Thống kê kết quả điểm
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public async Task<ClassStatisticsDto> GetClassStatisticsAsync(int classId)
        {
            var classInfo = await _classRepo.GetByIdAsync(classId);
            var components = await _gradeRepo.GetGradeComponentsAsync(classInfo.SubjectId);
            var grades = await _gradeRepo.GetGradesByClassAsync(classId);

            // chia theo học sinh
            var grouped = grades.GroupBy(g => g.StudentId);

            var avgScores = new List<double>();

            foreach (var group in grouped)
            {
                double avg = 0;

                foreach (var g in group)
                {
                    var comp = components.First(c => c.GradeComponentId == g.GradeComponentId);
                    avg += (g.Score ?? 0) * (comp.Weight / 100.0);
                }

                avgScores.Add(Math.Round(avg, 2));
            }

            if (!avgScores.Any())
                return new ClassStatisticsDto();

            int total = avgScores.Count;
            int passed = avgScores.Count(a => a >= 5);

            return new ClassStatisticsDto
            {
                TotalStudents = total,
                Passed = passed,
                Failed = total - passed,
                PassRate = Math.Round((double)passed / total * 100, 2),
                FailRate = Math.Round(((double)(total - passed) / total) * 100, 2),
                AverageClassScore = Math.Round(avgScores.Average(), 2),
                MaxScore = avgScores.Max(),
                MinScore = avgScores.Min(),
                ScoreHistogram = avgScores
                    .GroupBy(a => (int)a)
                    .ToDictionary(g => g.Key, g => g.Count())
            };
        }

        public async Task<StudentDto> GetStudentByIdAsync(int studentId)
        {
            var s = await _classStudentRepo.GetStudentByIdAsync(studentId);
            if (s == null) return null;

            return new StudentDto
            {
                StudentId = s.StudentId,
                StudentCode = s.StudentCode,
                FullName = s.User.FullName,
                Email = s.User.Email
            };
        }


        public async Task<IEnumerable<GradeComponentDto>> GetGradeComponentsByClassIdAsync(int classId, int studentId)
        {
            var cls = await _classRepo.GetByIdAsync(classId);
            if (cls == null) return new List<GradeComponentDto>();

            var components = await _gradeRepo.GetGradeComponentsAsync(cls.SubjectId);

            return components.Select(c => new GradeComponentDto
            {
                GradeComponentId = c.GradeComponentId,
                ComponentName = c.ComponentName,
                Weight = c.Weight,
                Score = c.StudentGrades?.FirstOrDefault(g => g.StudentId == studentId)?.Score
            }).ToList();
        }


    }
}
