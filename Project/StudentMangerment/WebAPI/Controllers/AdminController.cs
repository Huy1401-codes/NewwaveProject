using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.DTOs.ManagerStudent;
using BusinessLogicLayer.DTOs.ManagerTeacher;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;

        public AdminController(IUserService userService, IStudentService studentService, ITeacherService teacherService)
        {
            _userService = userService;
            _studentService = studentService;
            _teacherService = teacherService;
        }

        // GET: api/Admin/ListRestore
        [HttpGet("ListRestore")]
        public async Task<IActionResult> ListRestore(string search = "", int pageIndex = 1, int pageSize = 10, int? roleId = null)
        {
            var (users, total) = await _userService.GetRestoreUsersAsync(search, pageIndex, pageSize, roleId);

            return Ok(new
            {
                Users = users,
                Total = total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Search = search,
                RoleId = roleId
            });
        }

        // GET: api/Admin/EditTrash/5
        [HttpGet("EditTrash/{id}")]
        public async Task<IActionResult> EditTrash(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        // POST: api/Admin/EditTrashStatus/5
        [HttpPost("EditTrashStatus/{id}")]
        public async Task<IActionResult> EditTrashStatus(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            var updateDto = new UserUpdateDto
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                IsStatus = true,
                RoleId = user.RoleIds
            };

            await _userService.UpdateAsync(updateDto);

            return Ok(new { Message = "Khôi phục tài khoản thành công!" });
        }

        // GET: api/Admin/ListStudent
        [HttpGet("ListStudent")]
        public async Task<IActionResult> ListStudent(int page = 1, int pageSize = 10, string search = "")
        {
            var result = await _studentService.GetPagedAsync(page, pageSize, search);

            var vm = new StudentPagedDto
            {
                Students = result.Data,
                TotalCount = result.TotalCount,
                Page = page,
                PageSize = pageSize,
                Search = search
            };

            return Ok(vm);
        }

        // POST: api/Admin/CreateStudent
        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (success, errorMessage) = await _studentService.CreateAsync(dto);

            if (!success) return BadRequest(new { Error = errorMessage });

            return Ok(new { Message = "Tạo Student thành công!" });
        }

        // GET: api/Admin/ListTeacher
        [HttpGet("ListTeacher")]
        public async Task<IActionResult> ListTeacher(int page = 1, int pageSize = 10, string search = "")
        {
            var result = await _teacherService.GetPagedAsync(page, pageSize, search);

            var vm = new TeacherPagesDto
            {
                Teachers = result.Data,
                TotalCount = result.TotalCount,
                Page = page,
                PageSize = pageSize,
                Search = search
            };

            return Ok(vm);
        }

        // POST: api/Admin/CreateTeacher
        [HttpPost("CreateTeacher")]
        public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (success, errorMessage) = await _teacherService.CreateAsync(dto);

            if (!success) return BadRequest(new { Error = errorMessage });

            return Ok(new { Message = "Tạo Teacher thành công!" });
        }

        // GET: api/Admin/SearchUsers?search=...
        [HttpGet("SearchUsers")]
        public async Task<IActionResult> SearchUsers(string search)
        {
            var users = await _studentService.GetAvailableStudentUsersAsync(search);

            var results = users.Select(u => new
            {
                id = u.UserId,
                text = $"{u.FullName} - Email: ({u.Email}) - Phone: ({u.Phone})"
            });

            return Ok(results);
        }

        // GET: api/Admin/SearchTeacher?search=...
        [HttpGet("SearchTeacher")]
        public async Task<IActionResult> SearchTeacher(string search)
        {
            var users = await _teacherService.GetAvailableTeacherUsersAsync(search);

            var results = users.Select(u => new
            {
                id = u.UserId,
                text = $"{u.FullName} - Email: ({u.Email}) - Phone: ({u.Phone})"
            });

            return Ok(results);
        }
    }
}
