using BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BusinessLayer.DTOs.Parking.ParkingDTOs;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingService _parkingService;
        private readonly ILogger<ParkingController> _logger;

        public ParkingController(IParkingService parkingService, ILogger<ParkingController> logger)
        {
            _parkingService = parkingService;
            _logger = logger;
        }

        [HttpPost("vehicle/in")]
        public async Task<IActionResult> VehicleIn([FromBody] VehicleInDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var record = await _parkingService.VehicleInAsync(dto);
                return Ok(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "VehicleIn error for {Plate}", dto.PlateNumber);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("vehicle/out")]
        public async Task<IActionResult> VehicleOut([FromBody] VehicleOutDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var record = await _parkingService.VehicleOutAsync(dto);
                return Ok(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "VehicleOut error for {Plate}", dto.PlateNumber);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("history/{customerId}")]
        public async Task<IActionResult> CustomerHistory(int customerId)
        {
            try
            {
                var records = await _parkingService.GetCustomerHistoryAsync(customerId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "History error for customer {Id}", customerId);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
