using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /Admin/Account/List
        public async Task<IActionResult> List(string search, int pageIndex = 1, int pageSize = 10, int? roleId = null, bool? status = null)
        {
            var (users, total) = await _userService.GetPagedUsersAsync(search, pageIndex, pageSize, roleId, status);

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
            ViewBag.Status = status;

            return View("/Views/Admin/Account/List.cshtml", users);
        }



        // GET: /Admin/Account/Create
        public async Task<IActionResult> Create()
        {
            await PopulateRolesAsync();
            return View("/Views/Admin/Account/Create.cshtml", new UserCreateDto());
        }

        // POST: /Admin/Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateDto user)
        {
            await PopulateRolesAsync();

            if (!ModelState.IsValid)
                return View("/Views/Admin/Account/Create.cshtml", user);

            var result = await _userService.AddAsync(user);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                await PopulateRolesAsync();
                return View("/Views/Admin/Account/Create.cshtml", user);
            }

            TempData["SuccessMessage"] = "Tạo tài khoản thành công!";
            return RedirectToAction("List");
        }

        // GET: /Admin/Account/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userDetail = await _userService.GetByIdAsync(id); // UserDetailDto
            if (userDetail == null)
                return NotFound();

            var userUpdate = new UserUpdateDto
            {
                UserId = userDetail.UserId,
                Username = userDetail.Username,
                FullName = userDetail.FullName,
                Email = userDetail.Email,
                Phone = userDetail.Phone,
                IsStatus = userDetail.IsStatus,
                RoleId = userDetail.RoleIds
            };

            await PopulateRolesAsync();
            return View("/Views/Admin/Account/Edit.cshtml", userUpdate);
        }

        // POST: /Admin/Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserUpdateDto user)
        {
            if (id != user.UserId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopulateRolesAsync();
                return View("/Views/Admin/Account/Edit.cshtml", user);
            }

            await _userService.UpdateAsync(user);
            TempData["SuccessMessage"] = "Cập nhật tài khoản thành công!";
            return RedirectToAction("List");
        }

        // POST: /Admin/Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(int UserId, string NewPassword, string ConfirmPassword)
        {
            if (NewPassword != ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu xác nhận không trùng khớp.";
                return RedirectToAction("Edit", new { id = UserId });
            }

            var result = await _userService.ResetPasswordAsync(UserId, NewPassword);

            if (!result)
            {
                TempData["ErrorMessage"] = "Cấp lại mật khẩu thất bại.";
                return RedirectToAction("Edit", new { id = UserId });
            }

            TempData["SuccessMessage"] = "Cấp lại mật khẩu thành công!";
            return RedirectToAction("Edit", new { id = UserId });
        }

        // GET: /Admin/Account/Details/5
        public async Task<IActionResult> Detail(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)   
                return NotFound();

            return View("/Views/Admin/Account/Detail.cshtml", user);
        }

        // GET: /Admin/Account/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View("/Views/Admin/Account/Delete.cshtml", user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            // Map UserDetailDto -> UserUpdateDto
            var updateDto = new UserUpdateDto
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                IsStatus = false,      // Vô hiệu hóa
                RoleId = user.RoleIds
            };

            await _userService.UpdateAsync(updateDto);

            TempData["SuccessMessage"] = "Xóa tài khoản thành công!";
            return RedirectToAction("List");
        }







        // Helper: Populate dropdown roles
        private async Task PopulateRolesAsync()
        {
            var rolesFromDb = await _userService.GetAllAsync();
            ViewBag.Roles = rolesFromDb
                .Where(r => r.RoleName != "Admin")
                .Select(r => new SelectListItem
                {
                    Value = r.RoleId.ToString(),
                    Text = r.RoleName
                })
                .ToList();
        }
    }
}
