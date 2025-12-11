using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class ClassScheduleConfig : IEntityTypeConfiguration<ClassSchedule>
    {
        public void Configure(EntityTypeBuilder<ClassSchedule> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasOne(s => s.Class)
                   .WithMany(c => c.ClassSchedules) 
                   .HasForeignKey(s => s.ClassId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(s => s.ClassSemester)
                   .WithMany(cs => cs.ClassSchedules)
                   .HasForeignKey(s => s.ClassSemesterId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
