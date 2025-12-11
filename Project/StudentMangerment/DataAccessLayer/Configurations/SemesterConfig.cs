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
    public class SemesterConfig : IEntityTypeConfiguration<Semester>
    {
        public void Configure(EntityTypeBuilder<Semester> builder)
        {
            builder.HasMany(s => s.ClassSemesters)
                   .WithOne(cs => cs.Semester)
                   .HasForeignKey(cs => cs.SemesterId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
