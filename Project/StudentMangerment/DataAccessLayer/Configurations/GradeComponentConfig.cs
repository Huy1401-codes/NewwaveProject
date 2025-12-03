using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
    public class GradeComponentConfig : IEntityTypeConfiguration<GradeComponent>
    {
        public void Configure(EntityTypeBuilder<GradeComponent> builder)
        {
            builder.HasOne(gc => gc.Subject)
                   .WithMany()
                   .HasForeignKey(gc => gc.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(gc => gc.StudentGrades)
                   .WithOne(sg => sg.GradeComponent)
                   .HasForeignKey(sg => sg.GradeComponentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
