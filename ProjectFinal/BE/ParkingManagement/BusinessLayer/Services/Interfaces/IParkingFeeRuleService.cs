using BusinessLayer.DTOs.ParkingFeeRules;

namespace BusinessLayer.Services.Interfaces
{
    public interface IParkingFeeRuleService
    {
        Task<IEnumerable<ParkingFeeRuleDto>> GetAllAsync();
        Task<ParkingFeeRuleDto> CreateAsync(ParkingFeeRuleCreateDto dto);
        Task UpdateAsync(int id, ParkingFeeRuleUpdateDto dto);
        Task DeleteAsync(int id);
    }

}
