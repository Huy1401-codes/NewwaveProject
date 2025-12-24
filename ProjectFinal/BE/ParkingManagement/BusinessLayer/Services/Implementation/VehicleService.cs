using AutoMapper;
using BusinessLayer.DTOs.Vehicles;
using BusinessLayer.Services.Interfaces;
using DataAccessLayer.UnitOfWork;
using DomainLayer.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services.Implementation
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(
            IUnitOfWork uow,
            IMapper mapper,
            ILogger<VehicleService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<VehicleDto>> GetAllAsync()
        {
            var vehicles = await _uow.Vehicles.GetAllAsync();
            return _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
        }

        public async Task CreateAsync(VehicleCreateDto dto)
        {
            _logger.LogInformation("Create vehicle {Plate}", dto.PlateNumber);

            var entity = _mapper.Map<Vehicle>(dto);
            await _uow.Vehicles.AddAsync(entity);
            await _uow.SaveChangesAsync();
        }
    }
}
