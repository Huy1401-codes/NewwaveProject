using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.RoleAdmin
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly SchoolContext _context;
        public SemesterRepository(SchoolContext context) => _context = context;

        public IQueryable<Semester> GetAllQueryable()
        {
            return _context.Semesters
                .AsQueryable();
        }

        public async Task<Semester> GetByIdAsync(int id)
        {
            return await _context.Semesters.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Semester semester)
        {
            await _context.Semesters.AddAsync(semester);
        }

        public async Task UpdateAsync(Semester semester)
        {
            _context.Semesters.Update(semester);
        }

        public async Task<IEnumerable<Semester>> GetAllAsync()
        {
            return await _context.Semesters.AsNoTracking()
                                 .OrderBy(s => s.Name)
                                 .ToListAsync();
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public async Task<bool> AnyAsync(Expression<Func<Semester, bool>> predicate)
        {
            return await _context.Semesters.AnyAsync(predicate);
        }
    }
}
