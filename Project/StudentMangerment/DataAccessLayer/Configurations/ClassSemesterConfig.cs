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
    public class ClassSemesterConfig : IEntityTypeConfiguration<ClassSemester>
    {
        public void Configure(EntityTypeBuilder<ClassSemester> builder)
        {
            builder.HasOne(cs => cs.Class)
                   .WithMany(c => c.ClassSemesters)
                   .HasForeignKey(cs => cs.ClassId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cs => cs.Semester)
                   .WithMany(s => s.ClassSemesters)
                   .HasForeignKey(cs => cs.SemesterId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(cs => cs.ClassSchedules)
                   .WithOne(sch => sch.ClassSemester)
                   .HasForeignKey(sch => sch.ClassSemesterId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
