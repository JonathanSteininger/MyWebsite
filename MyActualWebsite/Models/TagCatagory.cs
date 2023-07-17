using System.ComponentModel.DataAnnotations;

namespace MyActualWebsite.Models
{
    public class TagCatagory
    {
        [Key]
        public int CatagoryId { get; set; }

        [Required, StringLength(50, MinimumLength = 1), Display(Name = "Catagory Name")]
        public string CatagoryName { get; set; }

        public List<Tag>? Tags { get; set; }
    }
}
