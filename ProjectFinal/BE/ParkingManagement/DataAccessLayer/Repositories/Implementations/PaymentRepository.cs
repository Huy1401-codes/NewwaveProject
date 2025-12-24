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
    public class PaymentRepository : GenericRepository<PaymentTransaction>, IPaymentRepository
    {
        public PaymentRepository(ParkingDbContext context) : base(context) { }

        public async Task<IEnumerable<PaymentTransaction>> GetByVehicleIdAsync(int vehicleId)
        {
            return await _dbSet
                .Include(p => p.ParkingRecord)
                .Where(p => p.Id == vehicleId)
                .ToListAsync();
        }
    }
}
