using DataAccessLayer.Context;
using DataAccessLayer.Repositories.Interfaces;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Implementations
{
    public class ParkingFeeRuleRepository
      : GenericRepository<ParkingFeeRule>, IParkingFeeRuleRepository
    {
        public ParkingFeeRuleRepository(ParkingDbContext context)
            : base(context) { }

        public async Task<ParkingFeeRule?> GetActiveRuleAsync(int vehicleTypeId)
        {
            return await _dbSet
                .Where(x =>
                    x.VehicleTypeId == vehicleTypeId &&
                    x.IsActive &&
                    x.EffectiveFrom <= DateTime.Now)
                .OrderByDescending(x => x.EffectiveFrom)
                .FirstOrDefaultAsync();
        }
    }

}
