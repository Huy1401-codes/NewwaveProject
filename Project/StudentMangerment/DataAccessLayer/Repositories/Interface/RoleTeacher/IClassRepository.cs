using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.Interface.RoleTeacher
{
    public interface IClassRepository
    {
        Task<(IEnumerable<Class> Data, int TotalCount)>
        GetTeacherClassesAsync(int teacherId, int page, int pageSize, string search, int? semesterId);

        Task<Class> GetByIdAsync(int classId);

        /// <summary>
        /// Teacher xem danh sách student
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        Task<List<Class>> GetClassesByTeacherAsync(int teacherId);
        Task<Class?> GetClassWithStudentsAsync(int classId);
    }

}
