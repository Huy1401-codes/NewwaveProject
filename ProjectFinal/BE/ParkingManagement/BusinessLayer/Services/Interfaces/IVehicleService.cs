using BusinessLayer.DTOs.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleDto>> GetAllAsync();
        Task CreateAsync(VehicleCreateDto dto);
    }

}
