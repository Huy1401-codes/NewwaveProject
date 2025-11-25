using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountService _service;

        public AuthController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _service.LoginAsync(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Tên người dùng hoặc mật khẩu không hợp lệ.");
                return View(model);
            }

            // lưu session
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);

            // lấy role
            var role = user.UserRoles.First().Role.RoleName;
            HttpContext.Session.SetString("Role", role);

            // điều hướng theo role
            if (role == "Admin") return RedirectToAction("Index", "Admin");
            if (role == "Teacher") return RedirectToAction("Index", "Teacher");
            if (role == "Student") return RedirectToAction("Index", "Student");

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

}
