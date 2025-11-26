using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleStudent
{
    public interface IStudentRepository
    {
        IQueryable<ClassStudent> GetStudentClassesQuery(int studentId);
        IQueryable<StudentGrade> GetStudentGradesQuery(int studentId);
    }
}
