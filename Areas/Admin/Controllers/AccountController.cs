using EatWellAssistant.Models;
using EatWellAssistant.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EatWellAssistant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly DBContext ctx;

        public AccountController(DBContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index()
        {
            var list = ctx.Users
                .Where(item => item.role.Equals("user"))
             .ToList();
            return View(list);
        }

        public IActionResult Block(int? userId)
        {
            if (userId != null)
            {
                var user = ctx.Users.FirstOrDefault(item => item.userId == userId);
                if (user != null)
                {
                    user.status = "block";
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult UnBlock(int? userId)
        {
            if (userId != null)
            {
                var user = ctx.Users.FirstOrDefault(item => item.userId == userId);
                if (user != null)
                {
                    user.status = "active";
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
