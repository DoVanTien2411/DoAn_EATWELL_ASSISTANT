using EatWellAssistant.Models;
using EatWellAssistant.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EatWellAssistant.Controllers
{
    public class CartController : Controller
    {
        private readonly DBContext ctx;

        public CartController(DBContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index()
        {
            var userId = Request.Cookies["UserId"];
            var user = ctx.Users.FirstOrDefault(item => item.userId == Convert.ToInt32(string.IsNullOrEmpty(userId) ? -1 : userId));
            if (user == null)
            {
                return RedirectToAction("Index", "Authentication");
            }
            var cartByUser = ctx.Cart
                .Include(item => item.cartItems)
                .ThenInclude(item => item.food)
                .FirstOrDefault(item => item.users.userId == user.userId);
            var viewModel = new CartViewModel(cartByUser ?? new Cart(), new Models.Entities.CartItems());
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Update(CartItems ci)
        {
            var cartItem = ctx.CartItems
                .Include(item => item.food)
                .FirstOrDefault(item => item.cartItemId == ci.cartItemId);
            if (cartItem != null)
            {
                cartItem.gram = ci.gram;
                cartItem.totalCalories = ci.gram * cartItem.food.caloriesPerGram;
                ctx.SaveChanges();
                updateCart(cartItem.cartId);
            }
            return RedirectToAction("Index");   
        }

        public IActionResult Delete(int id)
        {
            var ci = ctx.CartItems.FirstOrDefault(item => item.cartItemId == id);
            if (ci != null)
            {
                var cartId = ci.cartId;
                ctx.CartItems.Remove(ci);
                ctx.SaveChanges();
                updateCart(cartId);
            }

            return RedirectToAction("Index");
        }

        private void updateCart(int? id)
        {
            var cart = ctx.Cart.FirstOrDefault(item => item.cartId == id);
            var listCartItem = ctx.CartItems
                .Include(item => item.cart)
                .Where(item => item.cartId == id);

            cart.totalGram = listCartItem.Sum(item => item.gram);
            cart.totalCalories = listCartItem.Sum(item => item.totalCalories);
            cart.totalProtein = listCartItem.Sum(item => item.totalProtein);
            cart.totalFat = listCartItem.Sum(item => item.totalFat);
            cart.totalAlcohol = listCartItem.Sum(item => item.totalAlcohol);
            cart.updatedAt = DateTime.Now;
            ctx.SaveChanges();
        }
    }
}
