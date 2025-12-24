using DataAccessLayer.Context;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using DomainLayer.Entities;

namespace DataAccessLayer.UnitOfWork
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ParkingDbContext _context;

        public UnitOfWork(ParkingDbContext context)
        {
            _context = context;
        }

        private IUserRepository? _users;

        public IUserRepository Users =>
            _users ??= new UserRepository(_context);


        private IGenericRepository<Role>? _roles;
        public IGenericRepository<Role> Roles =>
            _roles ??= new GenericRepository<Role>(_context);

        private IGenericRepository<UserRole>? _userRoles;
        public IGenericRepository<UserRole> UserRoles =>
            _userRoles ??= new GenericRepository<UserRole>(_context);

        private IVehicleRepository? _vehicles;
        public IVehicleRepository Vehicles =>
            _vehicles ??= new VehicleRepository(_context);

        private IGenericRepository<VehicleType>? _vehicleTypes;
        public IGenericRepository<VehicleType> VehicleTypes =>
            _vehicleTypes ??= new GenericRepository<VehicleType>(_context);

        private IParkingSlotRepository _parkingSlots;
        public IParkingSlotRepository ParkingSlots =>
            _parkingSlots ??= new ParkingSlotRepository(_context);

        private IParkingRepository? _parkingRecords;
        public IParkingRepository ParkingRecords =>
            _parkingRecords ??= new ParkingRepository(_context);

        private IMonthlyPassRepository _monthlyPasses;
        public IMonthlyPassRepository MonthlyPasses =>
            _monthlyPasses ??= new MonthlyPassRepository(_context);

        private IParkingFeeRuleRepository _parkingFeeRules;
        public IParkingFeeRuleRepository ParkingFeeRules =>
            _parkingFeeRules ??= new ParkingFeeRuleRepository(_context);

        private IGenericRepository<RefreshToken>? _refreshTokens;
        public IGenericRepository<RefreshToken> RefreshTokens =>
            _refreshTokens ??= new GenericRepository<RefreshToken>(_context);

        private IPaymentRepository _paymentTransaction;
        public IPaymentRepository PaymentRepository =>
            _paymentTransaction ??= new PaymentRepository(_context);


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
