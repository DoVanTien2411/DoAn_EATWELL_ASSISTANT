using Microsoft.AspNetCore.Mvc;

namespace EatWellAssistant.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            ViewData["CurrentController"] = "About";
            return View();
        }
    }
}
