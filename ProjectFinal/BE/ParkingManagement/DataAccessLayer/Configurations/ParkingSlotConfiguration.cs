using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class ParkingSlotConfiguration : IEntityTypeConfiguration<ParkingSlot>
    {
        public void Configure(EntityTypeBuilder<ParkingSlot> builder)
        {
            builder.ToTable("ParkingSlots");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SlotCode)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasIndex(x => x.SlotCode).IsUnique();
        }
    }

}
