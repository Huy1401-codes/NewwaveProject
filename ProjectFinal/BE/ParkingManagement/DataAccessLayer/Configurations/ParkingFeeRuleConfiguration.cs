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
    public class ParkingFeeRuleConfiguration : IEntityTypeConfiguration<ParkingFeeRule>
    {
        public void Configure(EntityTypeBuilder<ParkingFeeRule> builder)
        {
            builder.ToTable("ParkingFeeRules");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.VehicleType)
                   .WithMany(x => x.ParkingFeeRules)
                   .HasForeignKey(x => x.VehicleTypeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
