using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleStudent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Implementations.RoleStudent
{
    public class StudentGradeRepository : IStudentGradeRepository
    {
        private readonly SchoolContext _context;

        public StudentGradeRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<List<StudentGrade>> GetGradesByClassSemesterAsync(int classSemesterId)
        {
            return await _context.StudentGrades
                .Include(g => g.Student).ThenInclude(s => s.User)
                .Include(g => g.GradeComponent)
                .Where(g => g.ClassSemesterId == classSemesterId)
                .ToListAsync();
        }

        public async Task SaveGradesAsync(List<StudentGrade> grades)
        {
            foreach (var grade in grades)
            {
                var existing = await _context.StudentGrades
                    .FirstOrDefaultAsync(g =>
                        g.StudentId == grade.StudentId &&
                        g.GradeComponentId == grade.GradeComponentId &&
                        g.ClassSemesterId == grade.ClassSemesterId);

                if (existing == null)
                    _context.StudentGrades.Add(grade);
                else
                    existing.Score = grade.Score;
            }

            await _context.SaveChangesAsync();
        }
    }

}
