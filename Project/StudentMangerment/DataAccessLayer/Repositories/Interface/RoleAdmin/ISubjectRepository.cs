using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.Common;

public interface ISubjectRepository : IRepository<Subject>
{
    IQueryable<Subject> GetActiveSubjects();
    Task SoftDeleteAsync(int id);
}
