using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly IGradeComponentService _gradeService;

        public SubjectController(ISubjectService subjectService, IGradeComponentService gradeService)
        {
            _subjectService = subjectService;
            _gradeService = gradeService;
        }

        // GET: api/Subject
        [HttpGet]
        public async Task<IActionResult> GetSubjects(string? search, int page = 1, int pageSize = 10)
        {
            var (data, totalItems) = await _subjectService.GetPagedSubjectsAsync(search, page, pageSize);
            return Ok(new
            {
                Data = data,
                Search = search,
                Page = page,
                PageSize = pageSize,
                Total = totalItems
            });
        }

        // POST: api/Subject
        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var subject = new Subject
            {
                Name = dto.Name,
                Credit = dto.Credit,
                IsStatus = true
            };

            await _subjectService.CreateAsync(subject);

            if (dto.GradeComponents != null)
            {
                foreach (var c in dto.GradeComponents)
                {
                    if (!string.IsNullOrEmpty(c.ComponentName))
                    {
                        await _gradeService.AddComponent(new CreateGradeComponentDto
                        {
                            SubjectId = subject.Id,
                            ComponentName = c.ComponentName,
                            Weight = c.Weight,
                            IsDelete = true
                        });
                    }
                }
            }

            return CreatedAtAction(nameof(GetSubjectById), new { id = subject.Id }, subject);
        }

        // GET: api/Subject/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null)
                return NotFound();

            var dto = new CreateSubjectDto
            {
                Name = subject.Name,
                Credit = subject.Credit,
                IsStatus = subject.IsStatus,
                GradeComponents = (await _gradeService.GetComponentsOfSubject(subject.Id))
                                  .Select(c => new CreateGradeComponentDto
                                  {
                                      ComponentName = c.ComponentName,
                                      Weight = c.Weight
                                  }).ToList()
            };

            return Ok(dto);
        }

        // PUT: api/Subject/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] CreateSubjectDto dto)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null)
                return NotFound();

            subject.Name = dto.Name;
            subject.Credit = dto.Credit;
            subject.IsStatus = dto.IsStatus;

            await _subjectService.UpdateAsync(subject);

            if (dto.GradeComponents != null)
            {

            }

            return Ok(subject);
        }

        // DELETE: api/Subject/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null)
                return NotFound();

            await _subjectService.DeleteAsync(subject.Id);
            return NoContent();
        }
    }
}
