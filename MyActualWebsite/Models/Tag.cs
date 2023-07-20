using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MyActualWebsite.Models
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }

        [Required, StringLength(60, MinimumLength = 1), Display(Name = "Tag Name")]
        public string TagName { get; set; }

        [ForeignKey("TagCatagoryID"), Display(Name = "Tag Catagory")]
        public int TagCatagoryID { get; set; }
        public TagCatagory? TagCatagory { get; set; }

        public List<Project> Projects { get; } = new();
        public List<ProjectTag> ProjectTags { get; set; } = new();

        [NotMapped]
        public bool isChecked { get; set; } = false;
    }
}
