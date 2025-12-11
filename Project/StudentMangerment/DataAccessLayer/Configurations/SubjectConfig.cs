using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class SubjectConfig : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasKey(s => s.Id);
            builder.HasMany(s => s.StudentGrades)
                   .WithOne(sg => sg.Subject)
                   .HasForeignKey(sg => sg.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.GradeComponents)
                   .WithOne(gc => gc.Subject)
                   .HasForeignKey(gc => gc.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
