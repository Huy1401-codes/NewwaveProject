using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class StudentGradeConfig : IEntityTypeConfiguration<StudentGrade>
    {
        public void Configure(EntityTypeBuilder<StudentGrade> builder)
        {
            builder.HasKey(sg => sg.Id);

            builder.HasOne(sg => sg.Student)
                   .WithMany(s => s.StudentGrades)
                   .HasForeignKey(sg => sg.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sg => sg.Subject)
                   .WithMany(s => s.StudentGrades)
                   .HasForeignKey(sg => sg.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sg => sg.Class)
                   .WithMany(c => c.StudentGrades)
                   .HasForeignKey(sg => sg.ClassId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sg => sg.GradeComponent)
                   .WithMany(gc => gc.StudentGrades)
                   .HasForeignKey(sg => sg.GradeComponentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sg => sg.ClassSemester)
                   .WithMany(cs => cs.StudentGrades)
                   .HasForeignKey(sg => sg.ClassSemesterId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
