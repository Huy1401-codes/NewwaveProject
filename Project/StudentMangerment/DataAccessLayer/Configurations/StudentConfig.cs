using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasMany(s => s.ClassStudents)
                   .WithOne(cs => cs.Student)
                   .HasForeignKey(cs => cs.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
