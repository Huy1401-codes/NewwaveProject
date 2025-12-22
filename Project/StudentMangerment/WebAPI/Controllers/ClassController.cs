using BusinessLogicLayer.DTOs.Admin.ManagerClass;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ClassController : ControllerBase
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

        // GET: api/Class
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _classService.GetAllAsync();
            return Ok(list);
        }

        // GET: api/Class/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cls = await _classService.GetByIdAsync(id);
            if (cls == null) return NotFound();

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
                StudentIds = cls.Students.Select(s => s.StudentId).ToList()
            };

            return Ok(dto);
        }

        // POST: api/Class
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClassCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _classService.CreateAsync(dto);
            return Ok(new { Message = "Tạo lớp học thành công!" });
        }

        // PUT: api/Class/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClassUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.ClassId)
                return BadRequest(new { Error = "ID không khớp." });

            await _classService.UpdateAsync(dto);
            return Ok(new { Message = "Cập nhật lớp học thành công!" });
        }

        // GET: api/Class/Dropdowns
        [HttpGet("Dropdowns")]
        public async Task<IActionResult> GetDropdowns()
        {
            var semesters = await _semesterService.GetAllAsync();
            var subjects = await _subjectService.GetAllNameAsync();
            var teachers = await _teacherService.GetAllAsync();
            var students = await _studentService.GetAllAsync();

            return Ok(new
            {
                Semesters = semesters.Select(s => new { s.Id, s.Name }),
                Subjects = subjects.Select(s => new { s.Id, s.Name }),
                Teachers = teachers.Select(t => new { t.Id, Name = t.User?.FullName ?? "[No Name]" }),
                Students = students.Select(s => new { s.Id, Name = s.User?.FullName ?? "[No Name]" })
            });
        }
    }
}
