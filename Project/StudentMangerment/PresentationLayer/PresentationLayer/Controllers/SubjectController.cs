using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;
        private readonly IGradeComponentService _gradeService;

        public SubjectController(ISubjectService subjectService, IGradeComponentService gradeService)
        {
            _subjectService = subjectService;
            _gradeService = gradeService;
        }

        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            var (data, totalItems) = await _subjectService.GetPagedSubjectsAsync(search, page, pageSize);
            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = totalItems;
            return View(data);
        }

        [HttpGet]
        public IActionResult Create() => View(new CreateSubjectDto());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSubjectDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

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

            TempData["SuccessMessage"] = "Tạo môn học thành công!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null) return NotFound();

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
            return View(dto);
        }
    }
}
