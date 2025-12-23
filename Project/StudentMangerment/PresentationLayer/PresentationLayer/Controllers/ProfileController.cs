using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Student")]
    public class ProfileController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICloudinaryService _cloudinaryService;

        public ProfileController(IAccountService accountService, ICloudinaryService cloudinaryService)
        {
            _accountService = accountService;
            _cloudinaryService = cloudinaryService;
        }

        // GET: /Profile/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return RedirectToAction("Login", "Auth");

            int userId = int.Parse(userIdClaim);
            var profile = await _accountService.GetMyProfileAsync(userId);

            if (profile == null)
                return NotFound();

            var model = new UserProfileUpdateDto
            {
                FullName = profile.FullName,
                Email = profile.Email,
                Phone = profile.Phone,
                ImageUrl = profile.ImageUrl,
                TeacherCode = profile.TeacherCode,
                StudentCode = profile.StudentCode
            };

            return View(model);
        }

        // GET: /Profile/Edit
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var profile = await _accountService.GetMyProfileAsync(userId);

            if (profile == null)
                return NotFound();

            var model = new UserProfileUpdateDto
            {
                FullName = profile.FullName,
                Email = profile.Email,
                Phone = profile.Phone,
                ImageUrl = profile.ImageUrl,
                TeacherCode = profile.TeacherCode,
                StudentCode = profile.StudentCode
            };

            return View(model);
        }

        // POST: /Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserProfileUpdateDto model, [FromServices] ILogger<ProfileController> logger)
        {
            if (string.IsNullOrWhiteSpace(model.FullName))
                ModelState.AddModelError(nameof(model.FullName), "Full name is required.");

            if (!ModelState.IsValid)
            {
                foreach (var kv in ModelState)
                {
                    foreach (var error in kv.Value.Errors)
                    {
                        logger.LogWarning("ModelState error: {Key} - {Error}", kv.Key, error.ErrorMessage);
                    }
                }
                return View(model);
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            model.UserId = userId;

            if (model.ImageFile != null)
            {
                logger.LogInformation("Uploading file: {FileName}", model.ImageFile.FileName);
                var imageUrl = await _cloudinaryService.UploadImageAsync(model.ImageFile);
                model.ImageUrl = imageUrl;
                logger.LogInformation("File uploaded successfully. URL: {ImageUrl}", imageUrl);
            }
            else
            {
                logger.LogInformation("No file uploaded.");
            }

            var result = await _accountService.UpdateProfileAsync(model);

            if (!result.Success)
            {
                logger.LogError("UpdateProfileAsync failed: {ErrorMessage}", result.ErrorMessage);
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            logger.LogInformation("Profile updated successfully for UserId: {UserId}", userId);
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Index");
        }
    }
}
