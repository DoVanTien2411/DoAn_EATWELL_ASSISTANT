using EatWellAssistant.Models.Entities;

namespace EatWellAssistant.Models
{
    public class MaterialDetailViewModel
    {
        public Food food { get; set; }
        public int quantity { get; set; }
        public int? page { get; set; }
        public int? cate { get; set; }

        public MaterialDetailViewModel(Food food, int quantity, int? page, int? cate)
        {
            this.food = food;
            this.quantity = quantity;
            this.page = page;
            this.cate = cate;
        }
    }
}
