using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Implementations.RoleAdmin;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchoolContext _context;

        public IUserRepository Users { get; }
        public IStudentRepository Students { get; }
        public ITeacherRepository Teachers { get; }
        public ISubjectRepository Subjects { get; }
        public IRoleRepository Roles { get; }
        public ISemesterRepository Semesters { get; }
        public IGradeComponentRepository GradeComponents { get; }
        public IAccountRepository Accounts { get; }
        public IClassSemesterRepository ClassSemesters { get; }



        public UnitOfWork(SchoolContext context)
        {
            _context = context;

            Users = new UserRepository(context);
            Students = new StudentRepository(context);
            Teachers = new TeacherRepository(context);
            Subjects = new SubjectRepository(context);
            Roles = new RoleRepository(context);
            Semesters = new SemesterRepository(context);
            GradeComponents = new GradeComponentRepository(context);
            Accounts = new AccountRepository(context);
            ClassSemesters = new ClassSemesterRepository(context);
        }

        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }

}
