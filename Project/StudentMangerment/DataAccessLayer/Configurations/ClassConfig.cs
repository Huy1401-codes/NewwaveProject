using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class ClassConfig : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.ClassStudents)
                   .WithOne(cs => cs.Class)
                   .HasForeignKey(cs => cs.ClassId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.ClassSemesters)
                   .WithOne(cs => cs.Class)
                   .HasForeignKey(cs => cs.ClassId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
