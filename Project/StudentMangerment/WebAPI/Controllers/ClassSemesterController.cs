using BusinessLogicLayer.DTOs.Admin.ManagerClass;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ClassSemesterController : ControllerBase
    {
        private readonly IClassSemesterService _classService;

        public ClassSemesterController(IClassSemesterService service)
        {
            _classService = service;
        }

        // GET: api/ClassSemester
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _classService.GetAllAsync();
            return Ok(list);
        }

        // GET: api/ClassSemester/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _classService.GetByIdAsync(id);
            if (model == null)
                return NotFound();

            return Ok(model);
        }

        // POST: api/ClassSemester
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClassCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _classService.CreateAsync(dto);
            return Ok(new { message = "Created successfully" });
        }

        // PUT: api/ClassSemester/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClassUpdateDto dto)
        {
            if (id != dto.ClassId)
                return BadRequest("Id mismatch");

            await _classService.UpdateAsync(dto);
            return Ok(new { message = "Updated successfully" });
        }

        // DELETE: api/ClassSemester/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _classService.DeleteAsync(id);
            return Ok(new { message = "Deleted successfully" });
        }
    }
}
