using DomainLayer.Common.BaseEntities;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer.Context
{
    public class ParkingDbContext : DbContext
    {
        public ParkingDbContext(DbContextOptions<ParkingDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<ParkingSlot> ParkingSlots => Set<ParkingSlot>();
        public DbSet<ParkingRecord> ParkingRecords => Set<ParkingRecord>();

        public DbSet<MonthlyPass> MonthlyPasses => Set<MonthlyPass>();
        public DbSet<ParkingFeeRule> ParkingFeeRules => Set<ParkingFeeRule>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParkingDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(SoftDeleteEntity<int>)
                    .IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(SoftDeleteEntity<int>.IsDeleted));
                    var condition = Expression.Equal(property, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(lambda);
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditableEntity<int> auditable)
                {
                    if (entry.State == EntityState.Added)
                        auditable.SetCreated();
                    else if (entry.State == EntityState.Modified)
                        auditable.SetUpdated();
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }


    }
}
