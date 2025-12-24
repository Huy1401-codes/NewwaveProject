using AutoMapper;
using BusinessLayer.Services.Interfaces;
using DataAccessLayer.UnitOfWork;
using DomainLayer.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLayer.DTOs.Parking.ParkingDTOs;

namespace BusinessLayer.Services.Implementation
{
    public class MonthlyPassService : IMonthlyPassService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<MonthlyPassService> _logger;

        public MonthlyPassService(
            IUnitOfWork uow,
            IMapper mapper,
            ILogger<MonthlyPassService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MonthlyPassDto>> GetAllAsync()
        {
            _logger.LogInformation("Get all monthly passes");

            var entities = await _uow.MonthlyPasses.GetAllAsync();
            return _mapper.Map<IEnumerable<MonthlyPassDto>>(entities);
        }

        public async Task CreateAsync(MonthlyPassCreateDto dto)
        {
            _logger.LogInformation(
                "Create MonthlyPass for VehicleId={VehicleId}", dto.VehicleId);

            // check vehicle exists
            var vehicle = await _uow.Vehicles.GetByIdAsync(dto.VehicleId);
            if (vehicle == null)
                throw new Exception("Vehicle not found");

            // check active monthly pass
            var activePass =
                await _uow.MonthlyPasses.GetActiveByVehicleIdAsync(dto.VehicleId);

            if (activePass != null)
                throw new Exception("Vehicle already has an active monthly pass");

            var entity = _mapper.Map<MonthlyPass>(dto);

            await _uow.MonthlyPasses.AddAsync(entity);
            await _uow.SaveChangesAsync();

            _logger.LogInformation(
                "MonthlyPass created successfully for VehicleId={VehicleId}", dto.VehicleId);
        }
    }

}
