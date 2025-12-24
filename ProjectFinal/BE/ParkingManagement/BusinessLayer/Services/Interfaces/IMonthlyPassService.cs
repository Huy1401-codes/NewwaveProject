using static BusinessLayer.DTOs.Parking.ParkingDTOs;

namespace BusinessLayer.Services.Interfaces
{
    public interface IMonthlyPassService
    {
        Task<IEnumerable<MonthlyPassDto>> GetAllAsync();
        Task CreateAsync(MonthlyPassCreateDto dto);
    }
}
