using BusinessLayer.DTOs.ParkingSlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IParkingSlotService
    {
        Task<IEnumerable<ParkingSlotDto>> GetAllAsync();
        Task CreateAsync(ParkingSlotCreateDto dto);
    }

}
