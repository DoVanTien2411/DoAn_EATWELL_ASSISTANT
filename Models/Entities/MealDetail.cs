using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatWellAssistant.Models.Entities
{
    public class MealDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mealDetailId { get; set; }

        [ForeignKey("Meal")]
        public int? mealId { get; set; }

        [ForeignKey("Food")]
        public int? foodId { get; set; }

        public int gram { get; set; }
        public decimal? totalCalories { get; set; }
        public decimal? totalProtein { get; set; }
        public decimal? totalCarb { get; set; }
        public decimal? totalFat { get; set; }
        public decimal? totalAlcohol { get; set; }

        public virtual Meal meal { get; set; }
        public virtual Food food { get; set; }
    }
}
