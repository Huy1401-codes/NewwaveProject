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

            var classSemester = await _classSemesterRepo.GetClassSemesterAsync(classId);
            if (classSemester == null)
                throw new Exception("ClassSemester not found");

            foreach (var (componentId, score) in scores)
            {
                var existingGrade = await _gradeRepo.GetSingleGradeAsync(
                    studentId,
                    classSemester.Id, 
                    componentId
                );

                if (existingGrade != null)
                {
                    existingGrade.Score = score;
                    existingGrade.UpdatedAt = DateTime.Now;
                }
                else
                {
                    var newGrade = new StudentGrade
                    {
                        StudentId = studentId,
                        ClassId = classId,               
                        ClassSemesterId = classSemester.Id, 
                        SubjectId = subjectId,
                        GradeComponentId = componentId,
                        Score = score,
                        UpdatedAt = DateTime.Now
                    };

                    await _gradeRepo.AddOrUpdateGradeAsync(newGrade);
                }
            }

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
            var classInfo = await _classRepo.GetByIdAsync(classId);
            if (classInfo == null)
                throw new Exception("Class not found");

            int subjectId = classInfo.SubjectId;

            var classSemester = await _classSemesterRepo.GetClassSemesterAsync(classId);
            if (classSemester == null)
                throw new Exception("ClassSemester not found");

            var components = await _gradeRepo.GetGradeComponentsAsync(subjectId);

            using var package = new ExcelPackage(file.OpenReadStream());
            var ws = package.Workbook.Worksheets[0];

            for (int row = 2; row <= ws.Dimension.End.Row; row++)
            {
                if (!int.TryParse(ws.Cells[row, 1].Value?.ToString(), out int studentId))
                    continue; 

                int col = 2; 
                foreach (var comp in components)
                {
                    var cellValue = ws.Cells[row, col].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(cellValue) && double.TryParse(cellValue, out double score))
                    {
                       
                        var existingGrade = await _gradeRepo.GetSingleGradeAsync(studentId, classSemester.Id, comp.GradeComponentId);

                        if (existingGrade != null)
                        {
                           
                            existingGrade.Score = score;
                            existingGrade.UpdatedAt = DateTime.Now;
                        }
                        else
                        {
                       
                            var newGrade = new StudentGrade
                            {
                                StudentId = studentId,
                                ClassId = classId,             
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
            var classInfo = await _classRepo.GetByIdAsync(classId);
            var components = await _gradeRepo.GetGradeComponentsAsync(classInfo.SubjectId);
            var students = await _classStudentRepo.GetStudentsWithUserByClassAsync(classId);

            var allGrades = await _gradeRepo.GetGradesByClassAsync(classId);

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Grades");

            ws.Cells[1, 1].Value = "Student ID";
            ws.Cells[1, 2].Value = "Full Name";

            int col = 3;
            foreach (var comp in components)
            {
                ws.Cells[1, col].Value = comp.ComponentName;
                col++;
            }

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
