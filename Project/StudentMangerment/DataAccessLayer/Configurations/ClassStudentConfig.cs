using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class ClassStudentConfig : IEntityTypeConfiguration<ClassStudent>
    {
        public void Configure(EntityTypeBuilder<ClassStudent> builder)
        {
            builder.HasKey(cs => new { cs.ClassId, cs.StudentId });

            builder.HasOne(cs => cs.Class)
                   .WithMany(c => c.ClassStudents)
                   .HasForeignKey(cs => cs.ClassId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cs => cs.Student)
                   .WithMany(s => s.ClassStudents)
                   .HasForeignKey(cs => cs.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
