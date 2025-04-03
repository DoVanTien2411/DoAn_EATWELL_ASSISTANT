using EatWellAssistant.Models;
using EatWellAssistant.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace EatWellAssistant.Controllers
{
    [AllowAnonymous]
    public class PostController : Controller
    {
        private readonly DBContext ctx;

        public PostController(DBContext context)
        {
            ctx = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] CreatePost model)
        {
            var userId = Request.Cookies["UserId"];
            Users user = null;
            if (!string.IsNullOrEmpty(userId))
                user = ctx.Users.Find(Int32.Parse(userId));
            if (user == null)
                return Json(new { Message = "Vui lòng đăng nhập lại để tiếp tục!", Success = false });

            var post = new Post
            {
                Content = model.Content,
                CreateUserId = user.userId,
                CreatedAt = DateTime.Now
            };
            if (model.Image != null)
            {
                try
                {
                    var folder = "wwwroot/files/" + post.CreatedAt.ToString("yyyyMMddHHmmss") + new Random().Next(10000, 99999);
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                    var filePath = folder + "/" + model.Image.FileName;
                    using (var file = new FileStream(filePath, FileMode.CreateNew))
                        model.Image.CopyTo(file);
                    post.Image = filePath.Replace("wwwroot", "");
                }
                catch (Exception e)
                {
                    return Json(new { Message = "Có lỗi xảy ra, vui lòng thử lại!", Success = false });
                }
            }
            ctx.Posts.Add(post);
            ctx.SaveChanges();
            return Json(new { Message = "Đăng bài thành công!", Success = true, data = new {
                post.Id,
                post.Content,
                post.Image,
                CreatedAt = post.CreatedAt.ToString("HH:mm dd/MM/yyyy"),
                CreateUserId = post.CreateUserId,
                CreateUser = user.fullName,
                Likes = 0,
                Comments = 0,
                IsLiked = false
            } });
        }

        [HttpPost("/Post/Load")]
        public IActionResult LoadPost([FromBody] LoadPostModel model)
        {
            var userId = Request.Cookies["UserId"];
            Users user = null;
            if (!string.IsNullOrEmpty(userId))
                user = ctx.Users.Find(Int32.Parse(userId));

            var query = ctx.Posts.AsNoTracking().Include(x => x.User).Include(x => x.Likes)
                .Include(x => x.Comments).Where(x => !x.IsReported);
            if (!string.IsNullOrEmpty(model.SearchKey))
                query = query.Where(x => x.Content.ToLower().Contains(model.SearchKey.ToLower()));
            if (model.CurrentIds.Any())
                query = query.Where(x => !model.CurrentIds.Contains(x.Id));
            if (model.OrderBy == 0)
                query = query.OrderByDescending(x => Guid.NewGuid()).ThenByDescending(x => x.CreatedAt);
            else if (model.OrderBy == 1)
                query = query.OrderByDescending(x => x.CreatedAt);
            else
                query = query.OrderByDescending(x => x.Comments.Count());

            var list = query.Take(15).ToList().Select(x => new
            {
                x.Id,
                x.Content,
                x.Image,
                CreatedAt = x.CreatedAt.ToString("HH:mm dd/MM/yyyy"),
                CreateUserId = x.CreateUserId,
                CreateUser = x.User.fullName,
                Likes = x.Likes.Count(),
                Comments = x.Comments.Count(),
                IsLiked = x.Likes.Any(u => u.CreateUserId == user?.userId)
            }).ToList();
            return Json(list);
        }

        [HttpGet("/Post/{id}")]
        public IActionResult PostView(int id)
        {
            var userId = Request.Cookies["UserId"];
            Users user = null;
            if (!string.IsNullOrEmpty(userId))
                user = ctx.Users.Find(Int32.Parse(userId));

            var post = ctx.Posts.Include(x => x.Likes).Include(x => x.User).Include(x => x.Comments).AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (post == null)
                return NotFound();

            ViewData["User"] = user;
            return PartialView(post);
        }

        [HttpGet("/Post/{id}/Remove")]
        public IActionResult Delete(int id)
        {
            var userId = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var user = ctx.Users.Find(Int32.Parse(userId));

            var post = ctx.Posts.Include(x => x.Comments).Include(x => x.Likes).FirstOrDefault(x => x.Id == id);
            if (post == null)
                return NotFound();

            if (user.userId != post.CreateUserId)
                return Unauthorized();

            ctx.Posts.Remove(post);
            ctx.SaveChanges();
            return NoContent();
        }

        [HttpGet("/Post/{id}/Report")]
        public IActionResult Report(int id)
        {
            var post = ctx.Posts.Include(x => x.Comments).Include(x => x.Likes).FirstOrDefault(x => x.Id == id);
            if (post == null)
                return NotFound();

            post.IsReported = true;
            ctx.Entry(post).State = EntityState.Modified;
            ctx.SaveChanges();
            return NoContent();
        }

        [HttpPost("/Post/Comment")]
        public IActionResult Comment([FromBody] CommentModel model)
        {
            var userId = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var user = ctx.Users.Find(Int32.Parse(userId));
            var post = ctx.Posts.FirstOrDefault(x => x.Id == model.PostId && !x.IsReported);
            if (post == null)
                return NotFound();

            var comment = new Comment
            {
                PostId = post.Id,
                Content = model.Content,
                CreateAt = DateTime.Now,
                CreateUserId = user.userId,
                CreateUserName = user.fullName
            };
            ctx.Comments.Add(comment);
            ctx.SaveChanges();
            return Json(new
            {
                comment.Id,
                comment.PostId,
                comment.Content,
                CreateAt = comment.CreateAt.ToString("HH:mm dd/MM/yyyy"),
                comment.CreateUserId,
                comment.CreateUserName
            });
        }

        [HttpPost("/Post/Like/{id}")]
        public IActionResult Like(int id)
        {
            var userId = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var user = ctx.Users.Find(Int32.Parse(userId));
            var post = ctx.Posts.FirstOrDefault(x => x.Id == id && !x.IsReported);
            if (post == null)
                return NotFound();

            var like = ctx.Likes.FirstOrDefault(x => x.PostId == id && x.CreateUserId == user.userId);
            if (like != null)
                ctx.Likes.Remove(like);
            else
                ctx.Likes.Add(new Like { PostId = post.Id, CreateUserId = user.userId, CreateAt = DateTime.Now });
            ctx.SaveChanges();
            var totalLike = ctx.Likes.Where(x => x.PostId == post.Id).Count();
            return Json(totalLike);
        }
    }
}
