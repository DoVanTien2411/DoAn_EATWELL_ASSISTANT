using EatWellAssistant.Models.Entities;

namespace EatWellAssistant.Models
{
    public class FoodViewModel
    {
        public Food food;
        public List<Category> categories;
        public List<Food> listFoods;
        public FoodViewModel(List<Category> listCate, Food p = null)
        {
            this.food = p;
            this.categories = listCate;
        }
        public FoodViewModel() { }
        public FoodViewModel(List<Food> list, List<Category> listCate) { this.listFoods = list; this.categories = listCate; }
    }
}
