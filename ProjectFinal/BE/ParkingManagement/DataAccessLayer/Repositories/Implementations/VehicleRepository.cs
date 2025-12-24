using DataAccessLayer.Context;
using DataAccessLayer.Repositories.Interfaces;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ParkingDbContext context) : base(context) { }

        public async Task<Vehicle?> GetByPlateNumberAsync(string plateNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(v => v.PlateNumber == plateNumber && v.IsActive && !v.IsDeleted);
        }
    }
}
