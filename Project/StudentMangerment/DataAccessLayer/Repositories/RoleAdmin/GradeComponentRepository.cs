using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly  ILogger<GradeComponentRepository> _logger;
        public GradeComponentRepository(SchoolContext context, ILogger<GradeComponentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<GradeComponent>> GetBySubjectAsync(int subjectId)
        {
            try
            {
                return await _context.GradeComponents
              .Where(x => x.SubjectId == subjectId && !x.IsDeleted)
              .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy Thành phần điểm cho Mã môn học {SubjectId}");
                throw;
            }
          
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
