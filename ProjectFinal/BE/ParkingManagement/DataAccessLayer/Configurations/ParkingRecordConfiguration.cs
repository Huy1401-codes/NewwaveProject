using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class ParkingRecordConfiguration : IEntityTypeConfiguration<ParkingRecord>
    {
        public void Configure(EntityTypeBuilder<ParkingRecord> builder)
        {
            builder.ToTable("ParkingRecords");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Vehicle)
                   .WithMany(x => x.ParkingRecords)
                   .HasForeignKey(x => x.VehicleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ParkingSlot)
                   .WithMany(x => x.ParkingRecords)
                   .HasForeignKey(x => x.ParkingSlotId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
