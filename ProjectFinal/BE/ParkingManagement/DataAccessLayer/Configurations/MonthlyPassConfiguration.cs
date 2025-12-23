using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
    public class MonthlyPassConfiguration : IEntityTypeConfiguration<MonthlyPass>
    {
        public void Configure(EntityTypeBuilder<MonthlyPass> builder)
        {
            builder.ToTable("MonthlyPasses");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Vehicle)
                   .WithMany(x => x.MonthlyPasses)
                   .HasForeignKey(x => x.VehicleId);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.MonthlyPasses)
                   .HasForeignKey(x => x.UserId);
        }
    }

}
