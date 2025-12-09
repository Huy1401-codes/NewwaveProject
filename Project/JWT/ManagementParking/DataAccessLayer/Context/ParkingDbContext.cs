using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Context
{
    public class ParkingDbContext : DbContext
    {
        public ParkingDbContext(DbContextOptions<ParkingDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ParkingSlot> ParkingSlots { get; set; }
        public DbSet<ParkingRecord> ParkingRecords { get; set; }
        public DbSet<ParkingFeeRule> ParkingFeeRules { get; set; }
        public DbSet<MonthlyPass> MonthlyPasses { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            // Unique indexes
            model.Entity<User>().HasIndex(u => u.Email).IsUnique();
            model.Entity<Vehicle>().HasIndex(v => v.PlateNumber).IsUnique();
            model.Entity<ParkingSlot>().HasIndex(s => s.SlotCode).IsUnique();

            // UserRole N-N
            model.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });

            model.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            model.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Vehicle – Owner
            model.Entity<Vehicle>()
                .HasOne(v => v.Owner)
                .WithMany(u => u.Vehicles)
                .HasForeignKey(v => v.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Monthly Pass
            model.Entity<MonthlyPass>()
                .HasOne(mp => mp.Vehicle)
                .WithMany(v => v.MonthlyPasses)
                .HasForeignKey(mp => mp.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            model.Entity<MonthlyPass>()
                .HasOne(mp => mp.User)
                .WithMany(u => u.MonthlyPasses)
                .HasForeignKey(mp => mp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            model.Entity<RefreshToken>()
               .HasOne(rt => rt.User)
               .WithMany(u => u.RefreshTokens)
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade); 

            // Seed roles
            model.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Staff" },
                new Role { RoleId = 3, RoleName = "Guest" }
            );
        }
    }


}
