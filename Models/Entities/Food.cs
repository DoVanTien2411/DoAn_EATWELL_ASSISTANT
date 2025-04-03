using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatWellAssistant.Models.Entities
{
    public class Food
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Food()
        {
            mealDetails = new HashSet<MealDetail>();
            cartItems = new HashSet<CartItems>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? foodId { get; set; }

        public string foodName { get; set; }

        public decimal? caloriesPerGram { get; set; }
        public string? imageUrl { get; set; }
        public bool? status { get; set; }

        public decimal? protein { get; set; }
        public decimal? carbs { get; set; }
        public decimal? fat { get; set; }
        public decimal? alcohol { get; set; }

        public virtual Category category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MealDetail> mealDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartItems> cartItems { get; set; }
    }
}
