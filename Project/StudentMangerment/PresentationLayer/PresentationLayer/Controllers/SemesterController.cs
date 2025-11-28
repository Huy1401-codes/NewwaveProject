using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{

    public class SemesterController : Controller
    {
        private readonly ISemesterService _semesterService;

        public SemesterController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        // GET: /Admin/Semester
        public async Task<IActionResult> Index(string search, int pageIndex = 1, int pageSize = 10)
        {
            var (semesters, total) = await _semesterService.GetPagedSemestersAsync(search, pageIndex, pageSize);
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = total;
            ViewBag.Search = search;

            return View(semesters);
        }

        // GET: /Admin/Semester/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Semester/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SemesterCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _semesterService.AddAsync(dto);
                TempData["Success"] = "Tạo kì học thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex) // validate ngày
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: /Admin/Semester/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var semester = await _semesterService.GetByIdAsync(id);
            if (semester == null) return NotFound();

            var dto = new SemesterDto
            {
                SemesterId = semester.SemesterId,
                Name = semester.Name,
                StartDate = semester.StartDate,
                EndDate = semester.EndDate
            };

            return View(dto);
        }

        // POST: /Admin/Semester/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SemesterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _semesterService.UpdateAsync(dto);
                TempData["Success"] = "Cập nhật kì học thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex) // validate ngày
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: /Admin/Semester/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var semester = await _semesterService.GetByIdAsync(id);
            if (semester == null) return NotFound();

            return View(semester);
        }
    }
}

