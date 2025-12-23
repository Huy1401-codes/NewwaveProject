using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PlateNumber)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasIndex(x => x.PlateNumber).IsUnique();

            builder.HasOne(x => x.VehicleType)
                   .WithMany(x => x.Vehicles)
                   .HasForeignKey(x => x.VehicleTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Owner)
                   .WithMany(x => x.Vehicles)
                   .HasForeignKey(x => x.OwnerId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

}
