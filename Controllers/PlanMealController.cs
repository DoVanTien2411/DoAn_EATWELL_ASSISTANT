using EatWellAssistant.Models;
using EatWellAssistant.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EatWellAssistant.Controllers
{
    public class PlanMealController : Controller
    {
        private readonly DBContext ctx;

        public PlanMealController(DBContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index()
        {
            var viewModel = getViewModel();
            return View(viewModel);
        }
        private PlanMealViewModel getViewModel()
        {
            var userId = Request.Cookies["UserId"];
            var user = ctx.Users.FirstOrDefault(item => item.userId == Convert.ToInt32(string.IsNullOrEmpty(userId) ? -1 : userId));
            var cartByUser = user != null
                ? ctx.Cart
                .Include(item => item.cartItems)
                .ThenInclude(item => item.food)
                .FirstOrDefault(item => item.users.userId == user.userId)
                : new Cart();
            var today = DateTime.Now;
            var listDay = Enumerable.Range(0, 6)
                .Select(day => today.AddDays(day)).ToList();
            var dList = listDay.Select(e => new { e.Date }).ToList();
            var listPlanMeal = user == null
                ? new List<PlanMeal>()
                : ctx.PlanMeal
                .Include(item => item.meals)
                .Where(item => listDay.Contains(item.date.Value.Date))
                .ToList();
            return new PlanMealViewModel(cartByUser ?? new Cart(), listPlanMeal, listDay);
        }

        public IActionResult Breakfast(int index)
        {
            var selectedCheckboxes = Request.Form["selection"].ToList();
            updatePlanMeal("Breakfast", index, selectedCheckboxes);
            return RedirectToAction("Index");
        }
        public IActionResult Lunch(int index)
        {
            var selectedCheckboxes = Request.Form["selection"].ToList();
            updatePlanMeal("Lunch", index, selectedCheckboxes);
            return RedirectToAction("Index");
        }
        public IActionResult Dinner(int index)
        {
            var selectedCheckboxes = Request.Form["selection"].ToList();
            updatePlanMeal("Dinner", index, selectedCheckboxes);
            return RedirectToAction("Index");
        }

        public IActionResult Check(int index)
        {
            var userId = Request.Cookies["UserId"];
            var user = ctx.Users
                .FirstOrDefault(item => item.userId == Convert.ToInt32(string.IsNullOrEmpty(userId) ? -1 : userId));
            if(user == null)
            {
                return RedirectToAction("Index", "Authentication");
            }
            var viewModel = getViewModel();
            var d = viewModel.listDay[index];
            var plan = ctx.PlanMeal
                .Include(item => item.users)
                .Include(item => item.meals)
                .ThenInclude(item => item.mealDetails)
                .ThenInclude(item => item.food)
                .FirstOrDefault(item => item.date.Value.Date == d.Date && item.users.userId == user.userId);
            plan = plan ?? new PlanMeal
            {
                meals = new HashSet<Meal>(),
            };
            viewModel = viewModel.getPlanDetail(d, plan);
            return View(viewModel);
        }


        private IActionResult updatePlanMeal(string meal, int index, List<string?> listCartItemId)
        {
            var userId = Request.Cookies["UserId"];
            var user = ctx.Users.FirstOrDefault(item => item.userId == Convert.ToInt32(string.IsNullOrEmpty(userId) ? -1 : userId));
            if (user == null)
            {
                return RedirectToAction("Index", "Authentication");
            }

            var viewModel = getViewModel();
            var dateChoose = viewModel.listDay.ToList()[index];
            var plMeal = ctx.PlanMeal
                .Include(item => item.users)
                .Include(item => item.meals)
                .ThenInclude(item => item.mealDetails)
                .FirstOrDefault(item =>
                item.users.userId == user.userId
                && item.date.Value.Date == dateChoose.Date);
            var listCartItem = ctx.CartItems
                    .Where(item => listCartItemId.Contains(item.cartItemId.ToString()))
                    .ToList();
            var m = new Meal
            {
                mealName = meal,
                mealDetails = new HashSet<MealDetail>()
            };
            foreach (var item in listCartItem)
            {
                m.mealDetails.Add(new MealDetail
                {
                    foodId = item.foodId,
                    gram = item.gram ?? 0,
                    totalCalories = item.totalCalories,
                    totalProtein = item.totalProtein,
                    totalCarb = item.totalCarb,
                    totalFat = item.totalFat,
                    totalAlcohol = item.totalAlcohol
                });
            }
            m.totalCalories = listCartItem.Sum(item => item.totalCalories);
            m.totalProtein = listCartItem.Sum(item => item.totalProtein);
            m.totalCarb = listCartItem.Sum(item => item.totalCarb);
            m.totalFat = listCartItem.Sum(item => item.totalFat);
            m.totalAlcohol = listCartItem.Sum(item => item.totalAlcohol);
            if (plMeal == null)
            {
                plMeal = new Models.Entities.PlanMeal
                {
                    date = dateChoose,
                    users = user,
                    meals = new HashSet<Models.Entities.Meal> { m }
                };
                ctx.PlanMeal.Add(plMeal);
                ctx.SaveChanges();
            }
            else
            {
                var mealExist = ctx.Meal
                    .Include(item => item.planMeal)
                    .FirstOrDefault(item => meal.Equals(item.mealName) && item.planMealId == plMeal.planMealId);
                
                if (mealExist == null)
                {
                    m.planMealId = plMeal.planMealId;
                    ctx.Meal.Add(m);
                    ctx.SaveChanges();
                }
                else
                {
                    var mealDetail = ctx.MealDetail.Where(item => item.mealId == mealExist.mealId).ToList();
                    foreach (var cid in listCartItem)
                    {
                        var existFood = mealDetail.FirstOrDefault(item => item.foodId == cid.foodId);
                        if(existFood == null)
                        {
                            var mealDeatailNew = new MealDetail
                            {
                                foodId = cid.foodId,
                                mealId = mealExist.mealId,
                                gram = cid.gram ?? 0,
                                totalCalories = cid.totalCalories,
                                totalProtein = cid.totalProtein,
                                totalCarb = cid.totalCarb,
                                totalFat = cid.totalFat,
                                totalAlcohol = cid.totalAlcohol,
                            };
                            ctx.MealDetail.Add(mealDeatailNew);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            var mdetail = ctx.MealDetail.FirstOrDefault(e => e.mealDetailId == existFood.mealDetailId);
                            if(mdetail != null)
                            {
                                mdetail.gram += cid.gram ?? 0;
                                mdetail.totalCalories += cid.totalCalories ?? 0;
                                mdetail.totalProtein += cid.totalProtein ?? 0;
                                mdetail.totalCarb += cid.totalCarb ?? 0;
                                mdetail.totalFat += cid.totalFat ?? 0;
                                mdetail.totalAlcohol += cid.totalAlcohol ?? 0;
                                ctx.SaveChanges();
                            }
                        }
                    }
                    var mDetails = ctx.MealDetail.Where(item => item.mealId == mealExist.mealId);
                    mealExist.totalCalories = mDetails.Sum(item => item.totalCalories);
                    mealExist.totalProtein = mDetails.Sum(item => item.totalProtein);
                    mealExist.totalCarb = mDetails.Sum(item => item.totalCarb);
                    mealExist.totalFat = mDetails.Sum(item => item.totalFat);
                    mealExist.totalAlcohol = mDetails.Sum(item => item.totalAlcohol);
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
