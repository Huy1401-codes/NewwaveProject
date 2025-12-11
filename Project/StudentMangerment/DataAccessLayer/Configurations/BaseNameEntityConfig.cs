using DataAccessLayer.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
    public class BaseNameEntityConfig<T> : IEntityTypeConfiguration<T> where T : BaseNameEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Code)
                   .HasMaxLength(50);

            builder.Property(e => e.Description)
                   .HasMaxLength(300);
        }
    }

}
