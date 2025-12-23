using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<UserRole> UserRoles { get; }

        IRepository<Vehicle> Vehicles { get; }
        IRepository<VehicleType> VehicleTypes { get; }

        IRepository<ParkingSlot> ParkingSlots { get; }
        IRepository<ParkingRecord> ParkingRecords { get; }

        IRepository<MonthlyPass> MonthlyPasses { get; }
        IRepository<ParkingFeeRule> ParkingFeeRules { get; }

        IRepository<RefreshToken> RefreshTokens { get; }

        Task<int> SaveChangesAsync();
    }
}
