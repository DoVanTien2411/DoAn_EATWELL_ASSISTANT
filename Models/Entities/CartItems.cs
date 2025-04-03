using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EatWellAssistant.Models.Entities
{
    public class CartItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cartItemId { get; set; }

        [ForeignKey("Cart")]
        public int? cartId { get; set; }

        [ForeignKey("Food")]
        public int? foodId { get; set; }


        public int? gram { get; set; }

        public decimal? totalCalories { get; set; }
        public decimal? totalProtein { get; set; }
        public decimal? totalCarb { get; set; }
        public decimal? totalFat { get; set; }
        public decimal? totalAlcohol { get; set; }

        public virtual Cart cart { get; set; }

        public virtual Food food { get; set; }
    }
}
