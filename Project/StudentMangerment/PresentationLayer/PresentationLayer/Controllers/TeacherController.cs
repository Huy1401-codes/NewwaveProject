using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
