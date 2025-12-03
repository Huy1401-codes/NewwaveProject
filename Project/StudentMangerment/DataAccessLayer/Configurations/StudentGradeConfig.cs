using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class StudentGradeConfig : IEntityTypeConfiguration<StudentGrade>
    {
        public void Configure(EntityTypeBuilder<StudentGrade> builder)
        {
            builder.HasOne(sg => sg.Student)
                   .WithMany()
                   .HasForeignKey(sg => sg.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sg => sg.Subject)
                   .WithMany()
                   .HasForeignKey(sg => sg.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sg => sg.ClassSemester)
                   .WithMany()
                   .HasForeignKey(sg => sg.ClassSemesterId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sg => sg.GradeComponent)
                   .WithMany(gc => gc.StudentGrades)
                   .HasForeignKey(sg => sg.GradeComponentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
