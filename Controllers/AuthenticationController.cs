using EatWellAssistant.Models;
using EatWellAssistant.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EatWellAssistant.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly DBContext ctx;

        public AuthenticationController(DBContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index()
        {
            ViewData["CurrentController"] = "Authentication";
            return View();
        }
        [HttpPost]
        public IActionResult doLogin(Users u)
        {
            var isExistEmail = ctx.Users.FirstOrDefault(item => item.email.Equals(u.email));
            if (isExistEmail == null)
            {
                ViewData["Msg"] = "Username or password invalid";
            }
            else
            {
                if (!u.password.Equals(isExistEmail?.password))
                {
                    ViewData["Msg"] = "Username or password invalid";
                }
                else if ("block".Equals(isExistEmail.status))
                {
                    ViewData["Msg"] = "Your account is blocked.";
                }
                else
                {
                    Response.Cookies.Append("UserId", isExistEmail.userId.ToString(), new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30)
                    });
                    if ("user".Equals(isExistEmail.role))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if ("admin".Equals(isExistEmail?.role.ToLower()))
                    {
                        return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                    }
                }
            }
            ViewData["CurrentController"] = "Authentication";
            return View("Index");
        }

        [HttpPost]
        public IActionResult doSignUp(Users u)
        {
            var isExistEmail = ctx.Users.FirstOrDefault(item => item.email.Equals(u.email));
            ViewData["CurrentController"] = "Authentication";
            if (isExistEmail != null)
            {
                ViewData["Msg"] = "Email is already register";
            }
            else
            {
                u.status = "active";
                u.role = "user";
                ctx.Users.Add(u);
                ctx.SaveChanges();
                ViewData["Msg"] = "Register successfully";
            }
            return View("Index");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserId");
            return RedirectToAction("Index");
        }
    }
}
