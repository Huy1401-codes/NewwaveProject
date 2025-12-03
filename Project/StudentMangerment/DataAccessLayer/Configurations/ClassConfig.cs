using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class ClassConfig : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasOne(c => c.Subject)
                   .WithMany()
                   .HasForeignKey(c => c.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Teacher)
                   .WithMany()
                   .HasForeignKey(c => c.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);

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
