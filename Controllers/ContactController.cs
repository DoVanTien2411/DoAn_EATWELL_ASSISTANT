using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using EatWellAssistant.Models;

namespace EatWellAssistant.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            ViewData["CurrentController"] = "Contact";
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(ContactForm model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Email người nhận mặc định
                    string toEmail = "hotroewa@gmail.com";

                    // Email người gửi là email hiện đang đăng nhập trong web
                    string fromEmail = User.Identity.Name;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(new MailAddress(toEmail));
                    mail.Subject = model.Subject;
                    mail.Body = $"Name: {model.Name}\nEmail: {model.Email}\nPhone: {model.Phone}\n\n{model.Message}";

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("dovantien24112003@gmail.com", "0898360389tienn"); // Thay thế bằng email và mật khẩu của bạn
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }

                    ViewBag.Message = "Your message has been sent successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Error: {ex.Message}";
                }
            }

            return View("Index", model);
        }
    }
}
