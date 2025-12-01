using BusinessLogicLayer.DTOs.Teacher;
using BusinessLogicLayer.Services.Interface.RoleTeacher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        // 1. Danh sách lớp
        public async Task<IActionResult> Classes(int page = 1, string search = "", int? semesterId = null)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();  // hoặc redirect login
            }

            int teacherId = int.Parse(userIdClaim.Value);

            var (classes, total) = await _teacherService
                .GetTeacherClassesAsync(teacherId, page, 10, search, semesterId);

            return View(classes);
        }

        // 2. Danh sách học sinh trong lớp
        public async Task<IActionResult> Students(int? classId, int page = 1, string search = "")
        {
            if (!classId.HasValue)
                return BadRequest("ClassId is required.");

            var (students, total) = await _teacherService
                .GetStudentsInClassAsync(classId.Value, page, 10, search);

            return View(students);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGrade(int? classId, int studentId, Dictionary<int, double> scores)
        {
            if (!classId.HasValue)
            {
                TempData["ErrorMessage"] = "ClassId is required.";
                return RedirectToAction("Students", new { classId = classId ?? 0 });
            }

            try
            {
                scores ??= new Dictionary<int, double>();
                await _teacherService.UpdateGradeAsync(classId.Value, studentId, scores);

                TempData["SuccessMessage"] = "Cập nhật điểm thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
            }

            return RedirectToAction("Students", new { classId = classId.Value });
        }

        public async Task<IActionResult> EnterGrades(int classId, int studentId)
        {
            var student = await _teacherService.GetStudentByIdAsync(studentId);
            var components = await _teacherService.GetGradeComponentsByClassIdAsync(classId, studentId);

            if (student == null)
                return NotFound("Student not found");

            var model = new EnterGradeViewModel
            {
                ClassId = classId,
                StudentId = studentId,
                StudentName = student.FullName,
                Components = components.ToList()
            };

            return PartialView("_EnterGradeModal", model);
        }



        // 4. Import Excel
        [HttpPost]
        public async Task<IActionResult> ImportExcel(int classId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "File không hợp lệ!";
                return RedirectToAction("Students", new { classId });
            }

            try
            {
                await _teacherService.ImportGradesAsync(classId, file);
                TempData["SuccessMessage"] = "Import điểm thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Import thất bại: {ex.Message}";
            }

            return RedirectToAction("Students", new { classId });
        }

        // 5. Export Excel
        public async Task<IActionResult> ExportExcel(int classId)
        {
            var data = await _teacherService.ExportGradesAsync(classId);
            return File(data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Grades_Class_{classId}.xlsx");
        }

        // 6. Thống kê
        public async Task<IActionResult> Statistics(int classId)
        {
            var result = await _teacherService.GetClassStatisticsAsync(classId);
            return View(result);
        }
    }

}
