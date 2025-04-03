using EatWellAssistant.Models;
using Microsoft.AspNetCore.Mvc;
using EatWellAssistant.Models.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EatWellAssistant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChatgptController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
