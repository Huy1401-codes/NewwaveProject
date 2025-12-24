using AutoMapper;
using BusinessLayer.DTOs.ParkingSlot;
using BusinessLayer.Services.Interfaces;
using DataAccessLayer.UnitOfWork;
using DomainLayer.Entities;

namespace BusinessLayer.Services.Implementation
{
    public class ParkingSlotService : IParkingSlotService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ParkingSlotService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParkingSlotDto>> GetAllAsync()
        {
            var slots = await _uow.ParkingSlots.GetAllAsync();
            return _mapper.Map<IEnumerable<ParkingSlotDto>>(slots);
        }

        public async Task CreateAsync(ParkingSlotCreateDto dto)
        {
            var slot = _mapper.Map<ParkingSlot>(dto);
            slot.IsOccupied = false;
            slot.IsActive = true;

            await _uow.ParkingSlots.AddAsync(slot);
            await _uow.SaveChangesAsync();
        }
    }

}
