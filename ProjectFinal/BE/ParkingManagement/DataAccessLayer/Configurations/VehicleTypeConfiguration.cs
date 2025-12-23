using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class VehicleTypeConfiguration : IEntityTypeConfiguration<VehicleType>
    {
        public void Configure(EntityTypeBuilder<VehicleType> builder)
        {
            builder.ToTable("VehicleTypes");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }

}
