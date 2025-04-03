using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatWellAssistant.Models.Entities
{
    public class Meal
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Meal()
        {
            mealDetails = new List<MealDetail>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mealId { get; set; }

        [ForeignKey("PlanMeal")]
        public int? planMealId { get; set; }

        public string mealName { get; set; }

        public decimal? totalCalories { get; set; }
        public decimal? totalProtein { get; set; }
        public decimal? totalCarb { get; set; }
        public decimal? totalFat { get; set; }
        public decimal? totalAlcohol { get; set; }

        public virtual PlanMeal planMeal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MealDetail> mealDetails { get; set; }
    }
}
