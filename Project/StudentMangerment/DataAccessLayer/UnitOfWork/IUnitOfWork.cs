using DataAccessLayer.Repositories.Interface.RoleAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IStudentRepository Students { get; }
        ITeacherRepository Teachers { get; }
        ISubjectRepository Subjects { get; }
        IRoleRepository Roles { get; }
        ISemesterRepository Semesters { get; }
        IGradeComponentRepository GradeComponents { get; }
        IAccountRepository Accounts { get; }
        IClassSemesterRepository ClassSemesters { get; }

        Task<int> SaveAsync();
    }

}
