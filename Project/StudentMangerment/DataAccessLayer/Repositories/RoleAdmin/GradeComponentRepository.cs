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
    public class GradeComponentRepository : IGradeComponentRepository
    {
        private readonly SchoolContext _context;

        public GradeComponentRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GradeComponent>> GetBySubjectAsync(int subjectId)
        {
            return await _context.GradeComponents
                .Where(x => x.SubjectId == subjectId && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<GradeComponent> GetByIdAsync(int id)
        {
            return await _context.GradeComponents.FindAsync(id);
        }

        public async Task AddAsync(GradeComponent entity)
        {
            _context.GradeComponents.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GradeComponent entity)
        {
            _context.GradeComponents.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var comp = await _context.GradeComponents.FindAsync(id);
            if (comp != null)
            {
                comp.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }

}
