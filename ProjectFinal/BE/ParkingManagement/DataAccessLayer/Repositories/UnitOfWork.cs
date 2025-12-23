using DataAccessLayer.Context;
using DomainLayer.Entities;
using DomainLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ParkingDbContext _context;

        public UnitOfWork(ParkingDbContext context)
        {
            _context = context;
        }

        private IRepository<User>? _users;
        public IRepository<User> Users =>
            _users ??= new GenericRepository<User>(_context);

        private IRepository<Role>? _roles;
        public IRepository<Role> Roles =>
            _roles ??= new GenericRepository<Role>(_context);

        private IRepository<UserRole>? _userRoles;
        public IRepository<UserRole> UserRoles =>
            _userRoles ??= new GenericRepository<UserRole>(_context);

        private IRepository<Vehicle>? _vehicles;
        public IRepository<Vehicle> Vehicles =>
            _vehicles ??= new GenericRepository<Vehicle>(_context);

        private IRepository<VehicleType>? _vehicleTypes;
        public IRepository<VehicleType> VehicleTypes =>
            _vehicleTypes ??= new GenericRepository<VehicleType>(_context);

        private IRepository<ParkingSlot>? _parkingSlots;
        public IRepository<ParkingSlot> ParkingSlots =>
            _parkingSlots ??= new GenericRepository<ParkingSlot>(_context);

        private IRepository<ParkingRecord>? _parkingRecords;
        public IRepository<ParkingRecord> ParkingRecords =>
            _parkingRecords ??= new GenericRepository<ParkingRecord>(_context);

        private IRepository<MonthlyPass>? _monthlyPasses;
        public IRepository<MonthlyPass> MonthlyPasses =>
            _monthlyPasses ??= new GenericRepository<MonthlyPass>(_context);

        private IRepository<ParkingFeeRule>? _parkingFeeRules;
        public IRepository<ParkingFeeRule> ParkingFeeRules =>
            _parkingFeeRules ??= new GenericRepository<ParkingFeeRule>(_context);

        private IRepository<RefreshToken>? _refreshTokens;
        public IRepository<RefreshToken> RefreshTokens =>
            _refreshTokens ??= new GenericRepository<RefreshToken>(_context);

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
