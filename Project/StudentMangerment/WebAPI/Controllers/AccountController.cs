using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/AccountApi/list
        [HttpGet("list")]
        public async Task<ActionResult> List(string search, int pageIndex = 1, int pageSize = 10, int? roleId = null, bool? status = null)
        {
            var (users, total) = await _userService.GetPagedUsersAsync(search, pageIndex, pageSize, roleId, status);
            var roles = await _userService.GetAllAsync();

            return Ok(new
            {
                Users = users,
                Total = total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Roles = roles.Select(r => new { r.Id, r.Name }),
                Search = search,
                RoleId = roleId,
                Status = status
            });
        }

        // GET: api/AccountApi/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailDto>> Get(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { Message = "Người dùng không tồn tại" });

            return Ok(user);
        }

        // POST: api/AccountApi/create
        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] UserCreateDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.AddAsync(user);
            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(new { Message = "Tạo tài khoản thành công" });
        }

        // PUT: api/AccountApi/edit/{id}
        [HttpPut("edit/{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] UserUpdateDto user)
        {
            if (id != user.UserId)
                return BadRequest(new { Message = "ID không khớp" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateAsync(user);
            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(new { Message = "Cập nhật tài khoản thành công" });
        }

        // POST: api/AccountApi/reset-password
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                return BadRequest(new { Message = "Mật khẩu xác nhận không trùng khớp" });

            var result = await _userService.ResetPasswordAsync(dto.UserId, dto.NewPassword);
            if (!result)
                return BadRequest(new { Message = "Cấp lại mật khẩu thất bại" });

            return Ok(new { Message = "Cấp lại mật khẩu thành công" });
        }

        // DELETE: api/AccountApi/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { Message = "Người dùng không tồn tại" });

            var updateDto = new UserUpdateDto
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                IsStatus = false,
                RoleId = user.RoleIds
            };

            await _userService.UpdateAsync(updateDto);

            return Ok(new { Message = "Xóa tài khoản thành công" });
        }
       
    }
}
