using static NuGet.Packaging.PackagingConstants;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EatWellAssistant.Models.Entities
{
    public class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            role = "user"; // Gán giá trị mặc định cho role là "user"
            carts = new HashSet<Cart>();
            planMeals = new HashSet<PlanMeal>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId { get; set; }

        [StringLength(255)]
        public string? email { get; set; }

        [StringLength(255)]
        public string? password { get; set; }

        [StringLength(255)]
        public string? fullName { get; set; }

        [StringLength(50)]
        public string? role { get; set; }

        [StringLength(50)]
        public string? status { get; set; }

        [ForeignKey("Profile")]
        public int? profileId { get; set; }

        public DateTime? createdAt { get; set; }

        public DateTime? updatedAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> carts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanMeal> planMeals { get; set; }
        public virtual Profile profile { get; set; }
    }
}
