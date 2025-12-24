using AutoMapper;
using BusinessLayer.Services.Interfaces;
using DataAccessLayer.UnitOfWork;
using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.Extensions.Logging;
using static BusinessLayer.DTOs.Parking.ParkingDTOs;

namespace BusinessLayer.Services.Implementation
{
    public class ParkingService : IParkingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ParkingService> _logger;
        private readonly IMapper _mapper;
        //private readonly IPaymentGatewayService _paymentGateway;

        public ParkingService(
            IUnitOfWork unitOfWork,
            ILogger<ParkingService> logger,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            //_paymentGateway = paymentGateway;
        }

        public async Task<ParkingRecordDto> VehicleInAsync(VehicleInDto dto)
        {
            _logger.LogInformation("Vehicle {PlateNumber} entering", dto.PlateNumber);

            var active = await _unitOfWork.ParkingRecords.GetActiveParkingByLicensePlateAsync(dto.PlateNumber);
            if (active != null) throw new InvalidOperationException("Vehicle is already parked");

            var slot = await _unitOfWork.ParkingSlots.GetAvailableSlotAsync();
            if (slot == null) throw new InvalidOperationException("No available parking slot");

            slot.IsOccupied = true;
            _unitOfWork.ParkingSlots.Update(slot);

            var vehicle = await _unitOfWork.Vehicles.GetByPlateNumberAsync(dto.PlateNumber);
            if (vehicle == null)
            {
                vehicle = new Vehicle
                {
                    PlateNumber = dto.PlateNumber,
                    VehicleTypeId = dto.VehicleTypeId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                await _unitOfWork.Vehicles.AddAsync(vehicle);
            }

            var record = new ParkingRecord
            {
                VehicleId = vehicle.Id,
                ParkingSlotId = slot.Id,
                TimeIn = DateTime.UtcNow,
                Note = "Vehicle entered"
            };

            await _unitOfWork.ParkingRecords.AddAsync(record);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ParkingRecordDto>(record);
        }

        public async Task<ParkingRecordDto> VehicleOutAsync(VehicleOutDto dto)
        {
            _logger.LogInformation("Vehicle {PlateNumber} exiting", dto.PlateNumber);

            var record =
                await _unitOfWork.ParkingRecords
                    .GetActiveParkingByLicensePlateAsync(dto.PlateNumber);

            if (record == null)
                throw new InvalidOperationException("Vehicle not found or already exited");

            record.TimeOut = DateTime.UtcNow;

            var rule = await _unitOfWork.ParkingFeeRules
                .GetActiveRuleAsync(record.Vehicle.VehicleTypeId);

            if (rule == null)
                throw new InvalidOperationException("Parking fee rule not found");

            var durationMinutes =
                (record.TimeOut.Value - record.TimeIn).TotalMinutes;

            var blocks =
                Math.Ceiling(durationMinutes / rule.BlockMinutes);

            var fee = (decimal)blocks * rule.PricePerBlock;

            if (rule.MaxPricePerDay.HasValue)
                fee = Math.Min(fee, rule.MaxPricePerDay.Value);

            record.Fee = fee;

            if (record.ParkingSlotId.HasValue)
            {
                var slot = await _unitOfWork.ParkingSlots
                    .GetByIdAsync(record.ParkingSlotId.Value);

                if (slot != null)
                {
                    slot.IsOccupied = false;
                    _unitOfWork.ParkingSlots.Update(slot);
                }
            }

            var payment = new PaymentTransaction
            {
                ParkingRecordId = record.Id,
                Amount = fee,
                PaymentMethod = PaymentMethod.Cash,
                Status = PaymentStatus.Success,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.PaymentRepository.AddAsync(payment);

            record.Note = $"Paid by CASH - {fee:N0}";

            _unitOfWork.ParkingRecords.Update(record);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation(
                "Vehicle {Plate} exited, fee={Fee}",
                dto.PlateNumber,
                fee);

            return _mapper.Map<ParkingRecordDto>(record);
        }


        public async Task<IEnumerable<ParkingRecordDto>> GetCustomerHistoryAsync(int customerId)
        {
            var records = await _unitOfWork.ParkingRecords.FindAsync(r => r.Vehicle.OwnerId == customerId);
            return _mapper.Map<IEnumerable<ParkingRecordDto>>(records);
        }
    }
}
