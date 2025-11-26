using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<ClassStudent> ClassStudents { get; set; }

        // Điểm thành phần của từng môn
        public DbSet<GradeComponent> GradeComponents { get; set; }
        public DbSet<StudentGrade> StudentGrades { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =============================
            // USER - ROLE MANY TO MANY
            // =============================
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });


            // =============================
            // USER - STUDENT (1 - 1)
            // =============================
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // =============================
            // USER - TEACHER (1 - 1)
            // =============================
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.User)
                .WithOne(u => u.Teacher)
                .HasForeignKey<Teacher>(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // =============================
            // CLASS RELATIONSHIP
            // =============================
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Subject)
                .WithMany()
                .HasForeignKey(c => c.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Semester)
                .WithMany()
                .HasForeignKey(c => c.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Teacher)
                .WithMany()
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);


            // =============================
            // CLASS SCHEDULE
            // =============================
            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Class)
                .WithMany()
                .HasForeignKey(cs => cs.ClassId)
                .OnDelete(DeleteBehavior.Restrict);


            // =============================
            // CLASS - STUDENT (MANY TO MANY)
            // =============================
            modelBuilder.Entity<ClassStudent>()
                .HasKey(cs => new { cs.ClassId, cs.StudentId });

            modelBuilder.Entity<ClassStudent>()
                .HasOne(cs => cs.Class)
                .WithMany()
                .HasForeignKey(cs => cs.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassStudent>()
                .HasOne(cs => cs.Student)
                .WithMany()
                .HasForeignKey(cs => cs.StudentId)
                .OnDelete(DeleteBehavior.Restrict);


            // =============================
            // GRADE COMPONENT
            // =============================
            modelBuilder.Entity<GradeComponent>()
                .HasOne(gc => gc.Subject)
                .WithMany()
                .HasForeignKey(gc => gc.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // =============================
            // STUDENT GRADE
            // =============================
            modelBuilder.Entity<StudentGrade>()
                .HasOne(sg => sg.Student)
                .WithMany()
                .HasForeignKey(sg => sg.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentGrade>()
                .HasOne(sg => sg.Subject)
                .WithMany()
                .HasForeignKey(sg => sg.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentGrade>()
                .HasOne(sg => sg.Class)
                .WithMany()
                .HasForeignKey(sg => sg.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentGrade>()
                .HasOne(sg => sg.GradeComponent)
                .WithMany(gc => gc.StudentGrades)
                .HasForeignKey(sg => sg.GradeComponentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
