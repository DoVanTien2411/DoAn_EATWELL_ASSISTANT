using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EatWellAssistant.Models.Entities
{
    public class Cart
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cart()
        {
            cartItems = new HashSet<CartItems>();
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cartId { get; set; }
        public int? totalGram { get; set; }
        public decimal? totalCalories { get; set; }
        public decimal? totalProtein { get; set; }
        public decimal? totalCarb { get; set; }
        public decimal? totalFat { get; set; }
        public decimal? totalAlcohol { get; set; }

        public DateTime? createdAt { get; set; }

        public DateTime? updatedAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartItems> cartItems { get; set; }

        public virtual Users users { get; set; }
    }
}
