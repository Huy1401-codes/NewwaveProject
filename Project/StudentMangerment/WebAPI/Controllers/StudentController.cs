using BusinessLogicLayer.Services.Interface.RoleStudent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Student/Classes
        [HttpGet("Classes")]
        public async Task<IActionResult> GetClasses(
            string? search = null,
            DateTime? from = null,
            DateTime? to = null,
            int page = 1,
            int pageSize = 10)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int studentId = int.Parse(userIdClaim.Value);

            var classes = await _studentService.GetClassesAsync(
                studentId, search, from, to, page, pageSize
            );

            return Ok(new
            {
                Data = classes,
                Search = search,
                From = from,
                To = to,
                Page = page,
                PageSize = pageSize
            });
        }

        // GET: api/Student/Grades
        [HttpGet("Grades")]
        public async Task<IActionResult> GetGrades()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int studentId = int.Parse(userIdClaim.Value);
            var grades = await _studentService.GetGradesAsync(studentId);

            return Ok(grades);
        }

        // GET: api/Student/Schedule
        [HttpGet("Schedule")]
        public async Task<IActionResult> GetSchedule(
            int? dayOfWeek = null,
            string? search = null,
            int page = 1,
            int pageSize = 10)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int studentId = int.Parse(userIdClaim.Value);
            var result = await _studentService.GetStudentSchedulesAsync(
                studentId, dayOfWeek, search, page, pageSize
            );

            return Ok(new
            {
                Data = result,
                DayOfWeek = dayOfWeek,
                Search = search,
                Page = page,
                PageSize = pageSize
            });
        }
    }
}
