using Microsoft.AspNetCore.Mvc;

namespace EatWellAssistant.Controllers
{
    public class HealthConsultingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
