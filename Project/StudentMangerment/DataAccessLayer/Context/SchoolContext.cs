using DataAccessLayer.Configurations;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;

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
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassSemester> ClassSemesters { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<GradeComponent> GradeComponents { get; set; }
        public DbSet<StudentGrade> StudentGrades { get; set; }
        public DbSet<ClassStudent> ClassStudents { get; set; }
        public DbSet<Semester> Semesters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new UserRoleConfig());
            modelBuilder.ApplyConfiguration(new StudentConfig());
            modelBuilder.ApplyConfiguration(new TeacherConfig());
            modelBuilder.ApplyConfiguration(new SubjectConfig());
            modelBuilder.ApplyConfiguration(new ClassConfig());
            modelBuilder.ApplyConfiguration(new ClassSemesterConfig());
            modelBuilder.ApplyConfiguration(new ClassScheduleConfig());
            modelBuilder.ApplyConfiguration(new GradeComponentConfig());
            modelBuilder.ApplyConfiguration(new StudentGradeConfig());
            modelBuilder.ApplyConfiguration(new ClassStudentConfig());
            modelBuilder.ApplyConfiguration(new SemesterConfig());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SchoolContext).Assembly);
        }

    }
}
