using BusinessLayer.DTOs.Auth;
using BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            _logger.LogInformation("Login attempt for {Email}", request.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for login request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.LoginAsync(request);

                if (response == null)
                {
                    _logger.LogWarning("Login failed for {Email}", request.Email);
                    return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });
                }

                _logger.LogInformation("Login successful for {Email}", request.Email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during login for {Email}", request.Email);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            _logger.LogInformation("Refresh token attempt");

            try
            {
                var response = await _authService.RefreshTokenAsync(refreshToken);
                _logger.LogInformation("Refresh token successful for user {UserId}", response.User.Id);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Invalid or expired refresh token");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during refresh token");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            _logger.LogInformation("Change password attempt for user {UserId}", dto.UserId);

            try
            {
                var result = await _authService.ChangePasswordAsync(dto);
                if (!result)
                {
                    _logger.LogWarning("Change password failed for user {UserId}", dto.UserId);
                    return BadRequest("Wrong old password");
                }

                _logger.LogInformation("Password changed successfully for user {UserId}", dto.UserId);
                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during change password for user {UserId}", dto.UserId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            _logger.LogInformation("Forgot password attempt for email {Email}", dto.Email);

            try
            {
                var result = await _authService.ForgotPasswordAsync(dto.Email);
                if (!result)
                {
                    _logger.LogWarning("Forgot password failed, email not found: {Email}", dto.Email);
                    return NotFound("Email not found");
                }

                _logger.LogInformation("New password generated and sent to email {Email}", dto.Email);
                return Ok("New password sent to email");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during forgot password for email {Email}", dto.Email);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
