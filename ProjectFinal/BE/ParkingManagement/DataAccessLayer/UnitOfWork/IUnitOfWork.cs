using DataAccessLayer.Repositories.Interfaces;
using DomainLayer.Entities;

namespace DataAccessLayer.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<UserRole> UserRoles { get; }

        IVehicleRepository Vehicles { get; }
        IGenericRepository<VehicleType> VehicleTypes { get; }

        IParkingSlotRepository ParkingSlots { get; }
        IParkingRepository ParkingRecords { get; }

        IMonthlyPassRepository MonthlyPasses { get; }
        IParkingFeeRuleRepository ParkingFeeRules { get; }

        IGenericRepository<RefreshToken> RefreshTokens { get; }

        IPaymentRepository PaymentRepository { get; }


        Task<int> SaveChangesAsync();
    }
}
