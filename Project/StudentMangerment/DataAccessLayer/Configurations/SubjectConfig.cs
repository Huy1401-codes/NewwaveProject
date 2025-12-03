using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class SubjectConfig : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasMany<GradeComponent>()
                   .WithOne(gc => gc.Subject)
                   .HasForeignKey(gc => gc.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
