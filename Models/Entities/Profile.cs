using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatWellAssistant.Models.Entities
{
    public class Profile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int profileId { get; set; }

        public int? age { get; set; }
        public string? gender { get; set; }
        public DateTime? birthDay { get; set; }
        public decimal? height { get; set; }
        public decimal? width { get; set; }

        public virtual Users account { get; set; }
    }
}
