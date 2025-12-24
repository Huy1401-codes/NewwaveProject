using AutoMapper;
using BusinessLayer.DTOs.ParkingFeeRules;
using BusinessLayer.Services.Interfaces;
using DataAccessLayer.UnitOfWork;
using DomainLayer.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services.Implementation
{
    public class ParkingFeeRuleService : IParkingFeeRuleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<ParkingFeeRuleService> _logger;

        public ParkingFeeRuleService(
            IUnitOfWork uow,
            IMapper mapper,
            ILogger<ParkingFeeRuleService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ParkingFeeRuleDto>> GetAllAsync()
        {
            _logger.LogInformation("Get all parking fee rules");
            var rules = await _uow.ParkingFeeRules.GetAllAsync();
            return _mapper.Map<IEnumerable<ParkingFeeRuleDto>>(rules);
        }

        public async Task<ParkingFeeRuleDto> CreateAsync(ParkingFeeRuleCreateDto dto)
        {
            var entity = _mapper.Map<ParkingFeeRule>(dto);
            entity.IsActive = true;
            entity.CreatedAt = DateTime.UtcNow;

            await _uow.ParkingFeeRules.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return _mapper.Map<ParkingFeeRuleDto>(entity);
        }

        public async Task UpdateAsync(int id, ParkingFeeRuleUpdateDto dto)
        {
            var entity = await _uow.ParkingFeeRules.GetByIdAsync(id);
            if (entity == null) throw new Exception("Rule not found");

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            _uow.ParkingFeeRules.Update(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.ParkingFeeRules.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Rule not found");

            _uow.ParkingFeeRules.Remove(entity);
            await _uow.SaveChangesAsync();
        }
    }
}
