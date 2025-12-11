using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.RoleAdmin
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly SchoolContext _context;
        public SubjectRepository(SchoolContext context) => _context = context;

        public IQueryable<Subject> GetAllQueryable() => _context.Subjects.Where(s => s.IsStatus==true);

        public async Task<Subject> GetByIdAsync(int id) =>
            await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id && s.IsStatus==true);

        public async Task AddAsync(Subject sub) => await _context.Subjects.AddAsync(sub);

        public async Task UpdateAsync(Subject sub) => _context.Subjects.Update(sub);

        public async Task SoftDeleteAsync(int id)
        {
            var sub = await _context.Subjects.FindAsync(id);
            if (sub != null)
            {
                sub.IsStatus = true;
                _context.Subjects.Update(sub);
            }
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();


        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            return await _context.Subjects.OrderBy(s => s.Name).ToListAsync();
        }
    }

}
