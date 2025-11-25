using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly SchoolContext _context;
        public ClassRepository(SchoolContext context) => _context = context;

        public IQueryable<Class> GetAllQueryable()
        {
            return _context.Classes
                .Where(c => !c.IsStatus)
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .Include(c => c.Semester)
                .AsQueryable();
        }

        public async Task<Class> GetByIdAsync(int id)
        {
            return await _context.Classes
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .Include(c => c.Semester)
                .FirstOrDefaultAsync(c => c.ClassId == id && !c.IsStatus);
        }

        public async Task AddAsync(Class cls) => await _context.Classes.AddAsync(cls);

        public async Task UpdateAsync(Class cls) => _context.Classes.Update(cls);

        public async Task SoftDeleteAsync(int id)
        {
            var cls = await _context.Classes.FindAsync(id);
            if (cls != null)
            {
                cls.IsStatus = true;
                _context.Classes.Update(cls);
            }
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }

}
