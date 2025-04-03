using EatWellAssistant.Models;
using EatWellAssistant.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EatWellAssistant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostReportedController : Controller
    {
        private readonly DBContext ctx;

        public PostReportedController(DBContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index()
        {
            var query = ctx.Posts.Include(x => x.Likes).Include(x => x.Comments).Include(x => x.User).AsNoTracking().Where(x => x.IsReported).ToList();

            return View(query);
        }

        [HttpGet("/Admin/Post/{id}/View")]
        public IActionResult PostView(int id)
        {
            var post = ctx.Posts.Include(x => x.Likes).Include(x => x.User).Include(x => x.Comments).AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (post == null)
                return NotFound();

            return PartialView(post);
        }

        [HttpGet("/Admin/Post/{id}/Cancle")]
        public IActionResult Cancle(int id)
        {
            var post = ctx.Posts.Include(x => x.Likes).Include(x => x.Comments).FirstOrDefault(x => x.Id == id);
            if (post == null) return NotFound();

            post.IsReported = false;
            ctx.Entry(post).State = EntityState.Modified;
            ctx.SaveChanges();
            return NoContent();
        }

        [HttpGet("/Admin/Post/{id}/Remove")]
        public IActionResult Remove(int id)
        {
            var post = ctx.Posts.Include(x => x.Likes).Include(x => x.Comments).FirstOrDefault(x => x.Id == id);
            if (post == null) return NotFound();

            ctx.Posts.Remove(post);
            ctx.SaveChanges();
            return NoContent();
        }
    }
}
