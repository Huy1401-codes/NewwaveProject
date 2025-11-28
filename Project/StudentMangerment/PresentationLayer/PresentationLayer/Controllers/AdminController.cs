using BusinessLogicLayer.DTOs.Admin;
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

        public AdminController(IUserService userService)
        {
            _userService = userService;
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
                Value = r.RoleId.ToString(),
                Text = r.RoleName
            }).ToList();

            ViewBag.Total = total;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Search = search;
            ViewBag.RoleId = roleId;

            return View("/Views/Admin/ListRestore.cshtml", users);
        }


        /// <summary>
        ///            Vẫn lỗi chưa nhận URL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/Admin/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditStatus(int id)
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

    }
}
