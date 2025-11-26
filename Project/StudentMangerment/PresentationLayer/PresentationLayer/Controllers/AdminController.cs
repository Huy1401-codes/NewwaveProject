using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Middleware;

namespace PresentationLayer.Controllers
{
    [RequireRole("Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /User/Index
        public async Task<IActionResult> Index(string search, int pageIndex = 1, int pageSize = 10, int? roleId = null, bool? status = null)
        {
            var (users, total) = await _userService.GetPagedUsersAsync(search, pageIndex, pageSize, roleId, status);

            ViewBag.Total = total;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Search = search;
            ViewBag.RoleId = roleId;
            ViewBag.Status = status;

            return View(users);
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            await _userService.AddAsync(user);
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, User user)
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
