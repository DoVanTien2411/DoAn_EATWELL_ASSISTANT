using EatWellAssistant.Models;
using EatWellAssistant.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Newtonsoft.Json;
using System.Security.Policy;
using System.Linq;

namespace EatWellAssistant.Controllers
{
    public class MaterialController : Controller
    {
        private readonly DBContext ctx;

        public MaterialController(DBContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index(int? page, int? cate)
        {
            var p = TempData["page"] ?? (page ?? 1);
            var c = TempData["cate"] ?? (cate ?? -1);
            var viewModel = getViewModel(Convert.ToInt32(p), Convert.ToInt32(c));
            return View(viewModel);
        }

        private MaterialViewModel getViewModel(int? page, int? cate)
        {
            int pageSize = 6;
            var p = page ?? 1;
            var c = cate ?? -1;
            var listProduct = c == -1
                ? ctx.Food
                .Include(item => item.category)
                .Where(item => item.status == true)
                .ToPagedList(p, pageSize)
                : ctx.Food
                .Include(item => item.category)
                .Where(item => item.status == true && item.category.categoryId == cate)
                .ToPagedList(p, pageSize);
            var listCate = ctx.Category.Where(item => item.status == true).ToList();
            var userId = Request.Cookies["UserId"];
            var user = ctx.Users.FirstOrDefault(item => item.userId == Convert.ToInt32(string.IsNullOrEmpty(userId) ? -1 : userId));
            var cartByUser = user != null ? ctx.Cart
                .Include(item => item.users)
                .Include(item => item.cartItems)
                .ThenInclude(item => item.food)
                .FirstOrDefault(item => item.users.userId == user.userId)
                : new Cart();
            return new MaterialViewModel(listProduct, listCate, p, c, cartByUser ?? new Cart());
        }
        public IActionResult Delete(int? id, int? page, int? cate)
        {
            ActionUpdate(id, "remove");
            TempData["page"] = page;
            TempData["cate"] = cate;
            return RedirectToAction("Index");
        }
        public IActionResult Up(int? id, int? page, int? cate)
        {
            ActionUpdate(id, "up");
            TempData["page"] = page;
            TempData["cate"] = cate;
            return RedirectToAction("Index");
        }
        public IActionResult Down(int? id, int? page, int? cate)
        {
            ActionUpdate(id, "down");
            TempData["page"] = page;
            TempData["cate"] = cate;
            return RedirectToAction("Index");
        }

        private void ActionUpdate(int? id, string action)
        {
            var cItem = ctx.CartItems.Include(item => item.food).Include(item => item.cart).FirstOrDefault(item => item.cartItemId == id);
            if (cItem == null) { return; }
            var cartId = cItem.cartId;
            if ("remove".Equals(action))
            {
                ctx.CartItems.Remove(cItem);
            }
            else
            {
                var f = cItem.food;
                cItem.gram = "up".Equals(action) ? cItem.gram + 1 : cItem.gram - 1;
                if (cItem.gram < 1) { return; }
                cItem.totalCalories = cItem.gram * f.caloriesPerGram;
                cItem.totalProtein = cItem.gram * f.protein;
                cItem.totalCarb = cItem.gram * f.carbs;
                cItem.totalFat = cItem.gram * f.fat;
                cItem.totalAlcohol = cItem.gram * f.alcohol;
            }
            ctx.SaveChanges();
            var cart = ctx.Cart.FirstOrDefault(item => item.cartId == cartId);
            var listCartItem = ctx.CartItems
                    .Include(item => item.cart)
                    .Where(item => item.cartId == cart.cartId);
            if (cart != null)
            {
                if (listCartItem.ToList().Count > 0)
                {
                    cart.totalGram = listCartItem.Sum(item => item.gram);
                    cart.totalCalories = listCartItem.Sum(item => item.totalCalories);
                    cart.totalProtein = listCartItem.Sum(item => item.totalProtein);
                    cart.totalCarb = listCartItem.Sum(item => item.totalCarb);       
                    cart.totalFat = listCartItem.Sum(item => item.totalFat);
                    cart.totalAlcohol = listCartItem.Sum(item => item.totalAlcohol);
                    cart.updatedAt = DateTime.Now;
                }
                else
                {
                    cart.totalGram = 0;
                    cart.totalCalories = 0;
                    cart.totalProtein = 0;
                    cart.totalCarb = 0;
                    cart.totalFat = 0;
                    cart.totalAlcohol = 0;
                    cart.updatedAt = DateTime.Now;
                }
                ctx.SaveChanges();
            }
        }

        public IActionResult Order(int? id, int? quantity, int? page, int? cate)
        {
            var viewModel = getViewModel(page, cate);
            var userId = Request.Cookies["UserId"];
            var user = ctx.Users.FirstOrDefault(item => item.userId == Convert.ToInt32(string.IsNullOrEmpty(userId) ? -1 : userId));
            if (user == null)
            {
                return RedirectToAction("Index", "Authentication");
            }
            var f = ctx.Food.FirstOrDefault(item => item.foodId == id);
            if (f == null)
            {
                ViewData["error"] = "Food không tồn tại";

                return View("Index", viewModel);
            }

            var cart = ctx.Cart
                .Include(i => i.cartItems)
                .Include(i => i.users)
                .FirstOrDefault(item => item.users.userId == user.userId);
            var q = quantity ?? 1;
            var cartItem = new CartItems
            {
                foodId = f.foodId,
                gram = q,
                totalCalories = q * f.caloriesPerGram,
                totalProtein = q * f.protein,
                totalFat = q * f.fat,
                totalCarb = q * f.carbs,
                totalAlcohol = q * f.alcohol
            };
            if (cart == null)
            {
                var newCart = new Cart
                {
                    users = user,
                    totalGram = q,
                    totalCalories = cartItem.totalCalories,
                    totalProtein = cartItem.totalProtein,
                    totalFat = cartItem.totalFat,
                    totalCarb = cartItem.totalCarb,
                    totalAlcohol = cartItem.totalAlcohol,
                    cartItems = new HashSet<CartItems> { cartItem }
                };
                ctx.Cart.Add(newCart);
            }
            else
            {
                var cItem = cart.cartItems.FirstOrDefault(item => item.foodId == f.foodId);
                if (cItem == null)
                {
                    cartItem.cart = cart;
                    cart.cartItems.Add(cartItem);
                }
                else
                {
                    cItem.gram += q;
                    cItem.totalCalories = cItem.gram * f.caloriesPerGram;
                    cItem.totalProtein = cItem.gram * f.protein;
                    cItem.totalCarb = cItem.gram * f.carbs;
                    cItem.totalFat = cItem.gram * f.fat;
                    cItem.totalAlcohol = cItem.gram * f.alcohol;
                    cart.cartItems.Add(cItem);
                }
                ctx.SaveChanges();
                var listCartItem = ctx.CartItems
                    .Include(item => item.cart)
                    .Where(item => item.cartId == cart.cartId);

                cart.totalGram = listCartItem.Sum(item => item.gram);
                cart.totalCalories = listCartItem.Sum(item => item.totalCalories);
                cart.totalProtein = listCartItem.Sum(item => item.totalProtein);
                cart.totalFat = listCartItem.Sum(item => item.totalFat);
                cart.totalAlcohol = listCartItem.Sum(item => item.totalAlcohol);
                cart.updatedAt = DateTime.Now;
            }
            ctx.SaveChanges();
            TempData["page"] = page;
            TempData["cate"] = cate;
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int? id, int? quantity, int? page, int? cate)
        {
            var detailFood = ctx.Food.FirstOrDefault(item => item.foodId == id);
            if (detailFood == null)
            {
                ViewData["error"] = "Food không tồn tại";
                return RedirectToAction("Index");
            }
            var viewModel = new MaterialDetailViewModel(detailFood, quantity ?? 1, page, cate);
            return View(viewModel);
        }
    }
}
