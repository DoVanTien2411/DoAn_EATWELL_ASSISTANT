using EatWellAssistant.Models;
using EatWellAssistant.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EatWellAssistant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly DBContext ctx;

        public CategoryController(DBContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index()
        {
            var listCate = ctx.Category.Where(item => item.status == true).ToList();
            return View(listCate);
        }

        public IActionResult Add()
        {
            return View(new Category());
        }

        [HttpPost]
        public ActionResult doAdd(Category cate)
        {
            cate.status = true;
            ctx.Category.Add(cate);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int? id)
        {
            var cate = ctx.Category.FirstOrDefault(item => item.categoryId == id);
            if (cate == null)
            {
                return RedirectToAction("Index");
            }
            return View(cate);
        }


        [HttpPost]
        public ActionResult doUpdate(Category cate)
        {
            var c = ctx.Category.FirstOrDefault(item => item.categoryId == cate.categoryId);
            if (c != null)
            {
                c.categoryName = cate.categoryName;
                c.description = cate.description;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            var c = ctx.Category.FirstOrDefault(item => item.categoryId == id);
            if (c != null)
            {
                c.status = false;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
