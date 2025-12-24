using BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BusinessLayer.DTOs.Parking.ParkingDTOs;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlyPassController : ControllerBase
    {
        private readonly IMonthlyPassService _service;

        public MonthlyPassController(IMonthlyPassService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all monthly passes
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Create new monthly pass
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MonthlyPassCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok(new { message = "Monthly pass created successfully" });
        }
    }
}
