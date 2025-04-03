using EatWellAssistant.Models.Entities;

namespace EatWellAssistant.Models
{
    public class PlanMealViewModel
    {
        public List<DateTime> listDay { get; set; }
        public Cart cart { get; set; }
        public List<PlanMeal> planMeals { get; set; }
        public DateTime day { get; set; }
        public PlanMeal plan { get; set; }
        public PlanMealViewModel(Cart cart, List<PlanMeal> planMeal, List<DateTime> dayList)
        {
            this.cart = cart;
            this.listDay = dayList;
            this.planMeals = planMeal;
        }

        public PlanMealViewModel getPlanDetail(DateTime d, PlanMeal p)
        {
            this.day = d; this.plan = p;
            return this;
        }
    }
}
