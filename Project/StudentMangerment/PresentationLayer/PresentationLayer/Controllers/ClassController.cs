using BusinessLogicLayer.DTOs.Admin.ManagerClass;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Controllers
{
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
            var list = await _classService.GetAllAsync();
            return View(list);
        }

        // =====================
        // CREATE
        // =====================
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

        // =====================
        // EDIT
        // =====================
        public async Task<IActionResult> Edit(int id)
        {
            var cls = await _classService.GetByIdAsync(id); // ClassDetailDto
            if (cls == null) return NotFound();

            // Map từ ClassDetailDto sang ClassUpdateDto
            var dto = new ClassUpdateDto
            {
                ClassId = cls.ClassId,
                ClassName = cls.ClassName,
                IsStatus = cls.IsStatus ?? true,
                SubjectId = cls.SubjectId,
                SemesterId = cls.SemesterId,
                TeacherId = cls.TeacherId,
                StudentIds = cls.Students.Select(s => s.StudentId).ToList()
            };

            await LoadDropdowns(dto.StudentIds);
            return View(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ClassUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(dto.StudentIds);
                return View(dto);
            }

            await _classService.UpdateAsync(dto);
            return RedirectToAction("Index");
        }


        // =====================
        // Dropdown Loading
        // =====================
        private async Task LoadDropdowns(List<int>? selectedStudents = null)
        {
            // Lấy dữ liệu, bảo vệ null
            var semesters = await _semesterService.GetAllAsync() ?? new List<Semester>();
            var subjects = await _subjectService.GetAllNameAsync() ?? new List<Subject>();
            var teachers = await _teacherService.GetAllAsync() ?? new List<Teacher>();
            var students = await _studentService.GetAllAsync() ?? new List<Student>();

            // Đảm bảo selectedStudents không null
            selectedStudents ??= new List<int>();
            var studentListForSelect = students
    .Select(s => new
    {
        s.StudentId,
        FullName = s.User?.FullName ?? "[No Name]"
    }).ToList();

            var teacherListForSelect = teachers
    .Select(t => new
    {
        t.TeacherId,
        FullName = t.User?.FullName ?? "[No Name]"
    }).ToList();


            ViewBag.Semesters = new SelectList(semesters, "SemesterId", "Name");
            ViewBag.Subjects = new SelectList(subjects, "SubjectId", "Name");
            ViewBag.Teachers = new SelectList(teacherListForSelect, "TeacherId", "FullName");
            ViewBag.Students = new MultiSelectList(studentListForSelect, "StudentId", "FullName", selectedStudents ?? new List<int>());
        }


        public async Task<IActionResult> Details(int id)
        {
            // Lấy entity Class từ database
            var cls = await _classService.GetByIdAsync(id); // cls là entity Class

            if (cls == null)
                return NotFound();

            // Map từ entity sang DTO
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
                //Students = cls.ClassStudents.Select(x => new StudentInClassDto
                //{
                //    StudentId = x.StudentId,
                //    FullName = x.Student.User.FullName
                //}).ToList
            };

            return View(dto);
        }


    }

}
