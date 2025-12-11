using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.Username).IsUnique();

            builder.HasMany(u => u.UserRoles)
                   .WithOne(ur => ur.User)
                   .HasForeignKey(ur => ur.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Student)
                   .WithOne(s => s.User)
                   .HasForeignKey<Student>(s => s.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Teacher)
                   .WithOne(t => t.User)
                   .HasForeignKey<Teacher>(t => t.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
