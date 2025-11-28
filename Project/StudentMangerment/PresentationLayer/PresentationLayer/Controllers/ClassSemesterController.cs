using BusinessLogicLayer.DTOs.Admin.ManagerClass;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class ClassSemesterController : Controller
    {
        private readonly IClassSemesterService _classService;

        public ClassSemesterController(IClassSemesterService service)
        {
            _classService = service;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _classService.GetAllAsync();
            return View(list);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ClassCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _classService.CreateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _classService.GetByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClassUpdateDto dto)
        {
            await _classService.UpdateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _classService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
