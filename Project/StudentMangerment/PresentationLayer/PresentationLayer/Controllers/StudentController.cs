using BusinessLogicLayer.Services.Interface.RoleStudent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index(
            string? search,
            DateTime? from,
            DateTime? to,
            int page = 1)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();  
            }

            int studentId = int.Parse(userIdClaim.Value);

            Console.WriteLine($"UserIdClaim Value: {userIdClaim.Value}");


            var classes = await _studentService.GetClassesAsync(
                studentId, search, from, to, page, 10
            );

            ViewBag.Search = search;
            ViewBag.From = from;
            ViewBag.To = to;

            return View(classes);
        }

        public async Task<IActionResult> StudentGrade()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();  // hoặc redirect login
            }

            int studentId = int.Parse(userIdClaim.Value);


            var grades = await _studentService.GetGradesAsync(studentId);

            return View(grades);
        }


        public async Task<IActionResult> StudentSchedule(int? dayOfWeek, string? search, int page = 1)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();  // hoặc redirect login
            }

            int studentId = int.Parse(userIdClaim.Value);


            var result = await _studentService.GetStudentSchedulesAsync(
                studentId,
                dayOfWeek,
                search,
                page,
                10
            );

            ViewBag.Day = dayOfWeek;
            ViewBag.Search = search;

            return View(result);
        }
    }
}
