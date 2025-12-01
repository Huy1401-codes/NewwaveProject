using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.Interface.RoleStudent
{
    public interface IStudentRepository
    {
        IQueryable<ClassStudent> GetStudentClassesQuery(int studentId);
        IQueryable<StudentGrade> GetStudentGradesQuery(int studentId);

        IQueryable<ClassSchedule> GetStudentSchedulesQuery(int studentId);
    }
}
