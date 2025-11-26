using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleTeacher;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.RoleTeacher
{
    public class GradeRepository : IGradeRepository
    {
        private readonly SchoolContext _context;

        public GradeRepository(SchoolContext context)
        {
            _context = context;
        }

        // Lấy toàn bộ điểm (nhiều thành phần) của 1 học sinh trong 1 lớp
        public async Task<List<StudentGrade>> GetStudentGradesAsync(int studentId, int classId)
        {
            return await _context.StudentGrades
                .Include(g => g.GradeComponent)
                .Where(g => g.StudentId == studentId && g.ClassId == classId)
                .ToListAsync();
        }

        // Lấy 1 điểm theo từng thành phần
        public async Task<StudentGrade?> GetSingleGradeAsync(int studentId, int classId, int componentId)
        {
            return await _context.StudentGrades
                .FirstOrDefaultAsync(g =>
                    g.StudentId == studentId &&
                    g.ClassId == classId &&
                    g.GradeComponentId == componentId);
        }

        /// <summary>
        /// Add hoặc update điểm cho từng thành phần
        /// </summary>
        public async Task<bool> AddOrUpdateGradeAsync(StudentGrade grade)
        {
            var existing = await GetSingleGradeAsync(
                grade.StudentId,
                grade.ClassId,
                grade.GradeComponentId
            );

            if (existing == null)
            {
                await _context.StudentGrades.AddAsync(grade);
            }
            else
            {
                existing.Score = grade.Score;
            }

            return true;
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        /// <summary>
        /// Lấy tất cả điểm của 1 lớp
        /// </summary>
        public async Task<List<StudentGrade>> GetGradesByClassAsync(int classId)
        {
            return await _context.StudentGrades
                .Include(g => g.Student)
                .Include(g => g.GradeComponent)
                .Where(g => g.ClassId == classId)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy danh sách tất cả GradeComponent theo SubjectId
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public async Task<List<GradeComponent>> GetGradeComponentsAsync(int subjectId)
        {
            return await _context.GradeComponents
                .Where(gc => gc.SubjectId == subjectId && !gc.IsDeleted)
                .ToListAsync();
        }

    }

}
