using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.DTOs.ManagerStudent;
using BusinessLogicLayer.DTOs.ManagerTeacher;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;


        public AdminController(IUserService userService, IStudentService student, ITeacherService teacherService)
        {
            _userService = userService;
            _studentService = student;
            _teacherService = teacherService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListRestore(string search, int pageIndex = 1, int pageSize = 10, int? roleId = null)
        {
            var (users, total) = await _userService.GetRestoreUsersAsync(search, pageIndex, pageSize, roleId);

            var roles = await _userService.GetAllAsync();
            ViewBag.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();

            ViewBag.Total = total;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Search = search;
            ViewBag.RoleId = roleId;

            return View("/Views/Admin/ListRestore.cshtml", users);
        }


        /// <summary>
        ///   Restore account         
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("EditTrash/{id}")]
        public async Task<IActionResult> EditTrash(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrashStatus(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            var updateDto = new UserUpdateDto
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                IsStatus = true,
                RoleId = user.RoleIds
            };

            await _userService.UpdateAsync(updateDto);

            TempData["SuccessMessage"] = "Khôi phục tài khoản thành công!";
            return RedirectToAction("ListRestore", "Admin");
        }


        public async Task<IActionResult> ListStudent(int page = 1, int pageSize = 10, string search = "")
        {
            var result = await _studentService.GetPagedAsync(page, pageSize, search);

            var vm = new StudentPagedDto
            {
                Students = result.Data,
                TotalCount = result.TotalCount,
                Page = page,
                PageSize = pageSize,
                Search = search
            };

            return View(vm);
        }


        [HttpGet]
        public IActionResult CreateStudent()
        {
            return View();
        }

        /// <summary>
        /// Support filter and search
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchUsers(string search)
        {
            var users = await _studentService.GetAvailableStudentUsersAsync(search);

            var results = users.Select(u => new
            {
                id = u.UserId,
                text = $"{u.FullName} - Email: ({u.Email}) - Phone: ({u.Phone})"
            });

            return Json(results);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStudent(CreateStudentDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var (success, errorMessage) = await _studentService.CreateAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(dto);
            }

            TempData["Success"] = "Tạo Student thành công!";
            return RedirectToAction("ListStudent");
        }


        public async Task<IActionResult> ListTeacher(int page = 1, int pageSize = 10, string search = "")
        {
            var result = await _teacherService.GetPagedAsync(page, pageSize, search);

            var vm = new TeacherPagesDto
            {
                Teachers = result.Data,
                TotalCount = result.TotalCount,
                Page = page,
                PageSize = pageSize,
                Search = search
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult CreateTeacher()
        {
            return View();
        }

        /// <summary>
        /// hỗ trợ lọc và search ở phần gắn giáo viên
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchTeacher(string search)
        {
            var users = await _teacherService.GetAvailableTeacherUsersAsync(search);

            var results = users.Select(u => new
            {
                id = u.UserId,
                text = $"{u.FullName} - Email: ({u.Email}) - Phone: ({u.Phone})"
            });

            return Json(results);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeacher(CreateTeacherDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var (success, errorMessage) = await _teacherService.CreateAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(dto);
            }

            TempData["Success"] = "Tạo Teacher thành công!";
            return RedirectToAction("ListTeacher");
        }

    }
}
