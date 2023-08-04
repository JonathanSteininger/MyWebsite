using System.ComponentModel.DataAnnotations;

namespace MyActualWebsite.Models
{
    public class StatBarCatagory
    {
        [Key]
        public int StatBarCatagoryID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be in between 2 and 50 chars")]
        public string Name { get; set; }

        public List<StatBar>? statBars { get; set; }
    }
}
