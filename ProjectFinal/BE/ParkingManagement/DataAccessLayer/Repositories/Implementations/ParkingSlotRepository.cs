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
    public class ParkingSlotRepository : GenericRepository<ParkingSlot>, IParkingSlotRepository
    {
        public ParkingSlotRepository(ParkingDbContext context) : base(context) { }

        public async Task<ParkingSlot?> GetAvailableSlotAsync()
        {
            return await _dbSet.FirstOrDefaultAsync(s => !s.IsOccupied && s.IsActive && !s.IsDeleted);
        }
    }
}
