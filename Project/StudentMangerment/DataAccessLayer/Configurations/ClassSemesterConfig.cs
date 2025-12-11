using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class ClassSemesterConfig : IEntityTypeConfiguration<ClassSemester>
    {
        public void Configure(EntityTypeBuilder<ClassSemester> builder)
        {
            builder.HasKey(cs => cs.Id);

            builder.HasMany(cs => cs.ClassSchedules)
                   .WithOne(s => s.ClassSemester)
                   .HasForeignKey(s => s.ClassSemesterId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasMany(cs => cs.StudentGrades)
                   .WithOne(sg => sg.ClassSemester)
                   .HasForeignKey(sg => sg.ClassSemesterId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
