using BusinessLogicLayer.DTOs.Admin.ManagerClass;
using BusinessLogicLayer.Enums.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClassController : Controller
    {
        private readonly IClassSemesterService _classService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly ISubjectService _subjectService;
        private readonly ISemesterService _semesterService;

        public ClassController(
            IClassSemesterService classService,
            IStudentService studentService,
            ITeacherService teacherService,
            ISubjectService subjectService,
            ISemesterService semesterService)
        {
            _classService = classService;
            _studentService = studentService;
            _teacherService = teacherService;
            _subjectService = subjectService;
            _semesterService = semesterService;
        }
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var list = await _classService.GetAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClassCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(dto);
            }

            await _classService.CreateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cls = await _classService.GetByIdAsync(id);
            if (cls == null) return NotFound();

            
            var dto = new ClassUpdateDto
            {
                ClassId = cls.ClassId,
                ClassName = cls.ClassName,
                IsStatus = cls.IsStatus == true
                          ? ClassStatus.Active
                          : ClassStatus.Inactive,
                SubjectId = cls.SubjectId,
                SemesterId = cls.SemesterId,
                TeacherId = cls.TeacherId,
                StudentIds = cls.Students.Select(s => s.StudentId).ToList()
            };


            await LoadDropdowns(dto.SemesterId, dto.SubjectId, dto.TeacherId, dto.StudentIds);
            return View(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ClassUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(dto.SemesterId, dto.SubjectId, dto.TeacherId, dto.StudentIds);
                return View(dto);
            }

            await _classService.UpdateAsync(dto);
            return RedirectToAction("Index");
        }

        private async Task LoadDropdowns(int? selectedSemesterId = null,
                                   int? selectedSubjectId = null,
                                   int? selectedTeacherId = null,
                                   List<int>? selectedStudents = null)
        {
            selectedStudents ??= new List<int>();

            var semesters = await _semesterService.GetAllAsync();
            var subjects = await _subjectService.GetAllNameAsync();
            var teachers = await _teacherService.GetAllAsync();
            var students = await _studentService.GetAllAsync();

            ViewBag.Semesters = new SelectList(semesters, "Id", "Name", selectedSemesterId);
            ViewBag.Subjects = new SelectList(subjects, "Id", "Name", selectedSubjectId);
            ViewBag.Teachers = teachers.Select(t => new {
                Value = t.Id.ToString(),
                Text = t.User?.FullName ?? "[No Name]"
            }).ToList();

            ViewBag.Students = students.Select(s => new {
                Value = s.Id.ToString(),
                Text = s.User?.FullName ?? "[No Name]"
            }).ToList();
        }


        public async Task<IActionResult> Details(int id)
        {
            var cls = await _classService.GetByIdAsync(id);

            if (cls == null)
                return NotFound();

            var dto = new ClassDetailDto
            {
                ClassId = cls.ClassId,
                ClassName = cls.ClassName,
                IsStatus = cls.IsStatus,
                SemesterId = cls.SemesterId,
                SemesterName = cls.SemesterName ?? "[No Semester]",
                SubjectId = cls.SubjectId,
                SubjectName = cls.SubjectName ?? "[No Subject]",
                TeacherId = cls.TeacherId,
                TeacherName = cls.TeacherName ?? "[No Teacher]",
            };
            return View(dto);
        }
    }
}
