using DataAccessLayer.Context;
using DataAccessLayer.Repositories.Interfaces;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations
{
    public class MonthlyPassRepository
     : GenericRepository<MonthlyPass>, IMonthlyPassRepository
    {
        public MonthlyPassRepository(ParkingDbContext context)
            : base(context)
        {
        }

        public async Task<MonthlyPass?> GetActiveByVehicleIdAsync(int vehicleId)
        {
            var now = DateTime.UtcNow;

            return await _dbSet
                .Include(x => x.Vehicle)
                .Where(x =>
                    x.VehicleId == vehicleId &&
                    x.StartDate <= now &&
                    x.EndDate >= now &&
                    x.IsActive)
                .FirstOrDefaultAsync();
        }
    }

}
