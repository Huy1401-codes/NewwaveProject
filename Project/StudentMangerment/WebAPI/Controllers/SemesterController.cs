using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _semesterService;

        public SemesterController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        // GET: api/Semester
        [HttpGet]
        public async Task<IActionResult> GetAll(string search = "", int pageIndex = 1, int pageSize = 10)
        {
            var (semesters, total) = await _semesterService.GetPagedSemestersAsync(search, pageIndex, pageSize);
            return Ok(new
            {
                Data = semesters,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                Search = search
            });
        }

        // GET: api/Semester/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var semester = await _semesterService.GetByIdAsync(id);
            if (semester == null)
                return NotFound();

            return Ok(semester);
        }

        // POST: api/Semester
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SemesterCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _semesterService.AddAsync(dto);
                return Ok(new { message = "Semester created successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/Semester/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SemesterDto dto)
        {
            if (id != dto.SemesterId)
                return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _semesterService.UpdateAsync(dto);
                return Ok(new { message = "Semester updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/Semester/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var semester = await _semesterService.GetByIdAsync(id);
        //    if (semester == null)
        //        return NotFound();

        //    await _semesterService.DeleteAsync(id);
        //    return Ok(new { message = "Semester deleted successfully" });
        //}
    }
}
