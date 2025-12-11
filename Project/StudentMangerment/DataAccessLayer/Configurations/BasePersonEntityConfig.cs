using DataAccessLayer.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class BasePersonEntityConfig<T> : IEntityTypeConfiguration<T> where T : BasePersonEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(e => e.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Email)
                   .HasMaxLength(100);

            builder.Property(e => e.Phone)
                   .HasMaxLength(20);
        }
    }
}
