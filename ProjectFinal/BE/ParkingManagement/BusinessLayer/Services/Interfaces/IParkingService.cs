using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLayer.DTOs.Parking.ParkingDTOs;

namespace BusinessLayer.Services.Interfaces
{
    public interface IParkingService
    {
        Task<ParkingRecordDto> VehicleInAsync(VehicleInDto dto);
        Task<ParkingRecordDto> VehicleOutAsync(VehicleOutDto dto);
        Task<IEnumerable<ParkingRecordDto>> GetCustomerHistoryAsync(int customerId);
    }
}
