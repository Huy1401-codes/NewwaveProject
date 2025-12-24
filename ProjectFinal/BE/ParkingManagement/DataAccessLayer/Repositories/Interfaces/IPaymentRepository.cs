using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<PaymentTransaction>
    {
        Task<IEnumerable<PaymentTransaction>> GetByVehicleIdAsync(int vehicleId);
    }
}
