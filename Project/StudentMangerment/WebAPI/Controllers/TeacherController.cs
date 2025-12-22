using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.ManagerTeacher;
using BusinessLogicLayer.Services.Interface.RoleTeacher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        // GET: api/Teacher/Classes
        [HttpGet("Classes")]
        public async Task<IActionResult> GetClasses(int page = 1, string search = "", int? semesterId = null)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int teacherId = int.Parse(userIdClaim.Value);
            var (classes, total) = await _teacherService
                .GetTeacherClassesAsync(teacherId, page, 10, search, semesterId);

            return Ok(new { Classes = classes, Total = total, Page = page, Search = search });
        }

        // GET: api/Teacher/Students?classId=1
        [HttpGet("Students")]
        public async Task<IActionResult> GetStudents([FromQuery] int classId, int page = 1, string search = "")
        {
            if (classId <= 0) return BadRequest("ClassId is required.");

            var (students, total) = await _teacherService.GetStudentsInClassAsync(classId, page, 10, search);
            return Ok(new { Students = students, Total = total, Page = page, Search = search });
        }
        
        // POST: api/Teacher/UpdateGrade
        [HttpPost("UpdateGrade")]
        public async Task<IActionResult> UpdateGrade([FromBody] UpdateGradeDto dto)
        {
            if (dto.ClassId <= 0) return BadRequest("ClassId is required.");

            try
            {
                await _teacherService.UpdateGradeAsync(dto.ClassId, dto.StudentId, dto.Scores ?? new Dictionary<int, double>());
                return Ok(new { Message = "Cập nhật điểm thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

        // GET: api/Teacher/EnterGrades?classId=1&studentId=2
        [HttpGet("EnterGrades")]
        public async Task<IActionResult> EnterGrades(int classId, int studentId)
        {
            var student = await _teacherService.GetStudentByIdAsync(studentId);
            if (student == null) return NotFound("Student not found");

            var components = await _teacherService.GetGradeComponentsByClassIdAsync(classId, studentId);
            var model = new EnterGradeViewModel
            {
                ClassId = classId,
                StudentId = studentId,
                StudentName = student.FullName,
                Components = components.ToList()
            };
            return Ok(model);
        }

        // POST: api/Teacher/ImportExcel?classId=1
        [HttpPost("ImportExcel")]
        public async Task<IActionResult> ImportExcel([FromQuery] int classId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File không hợp lệ!");

            try
            {
                await _teacherService.ImportGradesAsync(classId, file);
                return Ok(new { Message = "Import điểm thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Import thất bại: {ex.Message}" });
            }
        }

        // GET: api/Teacher/ExportExcel?classId=1
        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel(int classId)
        {
            var data = await _teacherService.ExportGradesAsync(classId);
            return File(data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Grades_Class_{classId}.xlsx");
        }

        // GET: api/Teacher/Statistics?classId=1
        [HttpGet("Statistics")]
        public async Task<IActionResult> GetStatistics(int classId)
        {
            var result = await _teacherService.GetClassStatisticsAsync(classId);
            return Ok(result);
        }
    }
}
