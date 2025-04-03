using EatWellAssistant.Models;
using EatWellAssistant.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EatWellAssistant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FoodController : Controller
    {
        private readonly DBContext ctx;

        public FoodController(DBContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index()
        {
            var listFood = ctx.Food
                .Include(item => item.category)
                .Where(item => item.status == true && item.category.status == true)
                .ToList();
            return View(listFood);
        }

        public ActionResult Add()
        {
            var listCate = ctx.Category
                .Where(item => item.status == true)
                .ToList();
            return View(new FoodViewModel(listCate, new Models.Entities.Food()));
        }

        [HttpPost]
        public ActionResult doAdd(Food food, IFormFile img, IFormCollection form)
        {
            food.status = true;
            if (img.Length > 0)
            {
                var fileName = Path.GetFileName(img.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    img.CopyTo(stream);
                }
                food.imageUrl = $"~/uploads/{fileName}";
            }
            var cateId = Convert.ToInt32(form["cate"]);
            food.category = ctx.Category.FirstOrDefault(item => item.categoryId == cateId);
            ctx.Food.Add(food);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                var f = ctx.Food
               .Include(item => item.category)
               .FirstOrDefault(item => item.foodId == id);
                if (f != null)
                {
                    var listCate = ctx.Category
                    .Where(item => item.status == true)
                    .ToList();
                    var pModel = new FoodViewModel(listCate, f);
                    return View(pModel);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult doUpdate(Food food, IFormFile img, IFormCollection form)
        {
            var p = ctx.Food.FirstOrDefault(item => item.foodId == food.foodId);
            if (p != null)
            {
                p.foodName = food.foodName;
                p.caloriesPerGram = food.caloriesPerGram;
                p.protein = food.protein;
                p.carbs = food.carbs;
                p.fat = food.fat;
                p.alcohol = food.alcohol;
                if (img.Length > 0)
                {
                    var fileName = Path.GetFileName(img.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        img.CopyTo(stream);
                    }
                    p.imageUrl = $"~/uploads/{fileName}";
                }
                var cateId = Convert.ToInt32(form["cate"]);
                food.category = ctx.Category.FirstOrDefault(item => item.categoryId == cateId);
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public ActionResult delete(int id)
        {
            var p = ctx.Food.FirstOrDefault(item => item.foodId == id);
            if (p != null)
            {
                p.status = false;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
