using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleTeacher
{
    public interface IClassSemesterRepository
    {
        Task<ClassSemester> GetClassSemesterAsync(int classId, int? semesterId = null);
    }
}
