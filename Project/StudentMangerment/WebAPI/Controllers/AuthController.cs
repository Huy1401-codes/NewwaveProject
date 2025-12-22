using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _service;

        public AuthController(IAccountService service)
        {
            _service = service;
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.LoginAsync(model.Email, model.Password);

            if (result.User == null)
                return Unauthorized(new { Error = result.ErrorMessage });

            var user = result.User;
            var role = user.UserRoles.FirstOrDefault()?.Role.Name ?? "Student";

            // Nếu dùng Cookie Auth trên API
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            // Trả JSON cho frontend xử lý redirect
            return Ok(new
            {
                Message = "Đăng nhập thành công!",
                Role = role,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email
            });
        }

        // POST: api/Auth/Logout
        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Đăng xuất thành công!" });
        }

        // GET: api/Auth/AccessDenied
        [HttpGet("AccessDenied")]
        [AllowAnonymous] // Đảm bảo không bị cookie redirect
        public IActionResult AccessDenied()
        {
            return Forbid(); 
        }

        [HttpPost("loginJWT")]
        [AllowAnonymous] // Đảm bảo không bị cookie redirect
        public async Task<IActionResult> LoginJWT([FromBody] LoginRequestDto dto)

        {
            try
            {
                var result = await _service.LoginJwtAsync(dto.Email, dto.Password);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

    

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var response = await _service.RefreshTokenAsync(refreshToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
