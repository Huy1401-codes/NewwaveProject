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

        /// <summary>
        /// danh sách account
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="roleId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        // GET: /User/
        public async Task<IActionResult> List(string search, int pageIndex = 1, int pageSize = 10, int? roleId = null, bool? status = null)
        {
            // Lấy danh sách user
            var (users, total) = await _userService.GetPagedUsersAsync(search, pageIndex, pageSize, roleId, status);

            // Lấy danh sách role để hiển thị dropdown filter
            var roles = await _userService.GetAllAsync(); // hoặc repository trực tiếp
            ViewBag.Roles = roles;

            // Gán các thông tin paging + filter
            ViewBag.Total = total;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Search = search;
            ViewBag.RoleId = roleId;
            ViewBag.Status = status;

            return View("Account/List", users);
        }


        // GET: /User/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }



        // GET: /User/Create
        public async Task<IActionResult> Create()
        {
            // Lấy tất cả role trừ Admin
            var rolesFromDb = await _userService.GetAllAsync();

            // Chuyển thành SelectListItem để bind dropdown
            ViewBag.Roles = rolesFromDb.Select(r => new SelectListItem
            {
                Value = r.RoleId.ToString(),
                Text = r.RoleName
            }).ToList();

            return View("Account/Create", new UserCreateDto());
        }

        // POST: /User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateDto user)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate Roles nếu form lỗi
                var rolesFromDb = await _userService.GetAllAsync();
                ViewBag.Roles = rolesFromDb.Select(r => new SelectListItem
                {
                    Value = r.RoleId.ToString(),
                    Text = r.RoleName
                }).ToList();

                return View("Account/Create", user);
            }

            await _userService.AddAsync(user);
            return RedirectToAction("Account/List");
        }

        // GET: /User/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserUpdateDto user)
        {
            if (id != user.UserId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(user);

            await _userService.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }

        // GET: /User/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }







    }
}
