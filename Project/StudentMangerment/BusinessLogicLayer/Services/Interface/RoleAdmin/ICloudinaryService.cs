using BusinessLogicLayer.DTOs.Admin;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface ICloudinaryService
    {
        Task<string?> UploadImageAsync(IFormFile file, string folder = "uploads");
        Task<bool> DeleteImageAsync(string imageUrl);
    }

}
