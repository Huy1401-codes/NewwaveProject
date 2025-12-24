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
    public class ParkingRepository : GenericRepository<ParkingRecord>, IParkingRepository
    {

        public ParkingRepository(ParkingDbContext context)
             : base(context)
        {
        }

        public async Task<ParkingRecord?> GetActiveParkingByLicensePlateAsync(string licensePlate)
        {
            return await _dbSet
                .Include(p => p.Vehicle)
                .Include(p => p.ParkingSlot)
                .Include(p => p.Vehicle.VehicleType)
                .Where(p => p.Vehicle.PlateNumber == licensePlate && p.TimeOut == null)
                .FirstOrDefaultAsync();
        }
    }
}
