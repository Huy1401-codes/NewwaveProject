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
    public class ClassSemesterRepository : IClassSemesterRepository
    {
        private readonly SchoolContext _context;

        public ClassSemesterRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<Class> GetByIdAsync(int id)
        {
            return await _context.Classes
                .Include(x => x.Semester)
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                    .ThenInclude(t => t.User) 
                .Include(x => x.ClassStudents)
                    .ThenInclude(cs => cs.Student)
                        .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<List<Class>> GetAllAsync()
        {
            return await _context.Classes
                .Include(x => x.Semester)
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                    .ThenInclude(t => t.User) 
                .Include(x => x.ClassStudents)
                    .ThenInclude(cs => cs.Student)
                        .ThenInclude(s => s.User) 
                .ToListAsync();
        }


        public async Task AddAsync(Class entity)
        {
            _context.Classes.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Class entity)
        {
            _context.Classes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var obj = await _context.Classes.FindAsync(id);
            if (obj != null)
            {
                _context.Classes.Remove(obj);
                await _context.SaveChangesAsync();
            }
        }
    }

}
