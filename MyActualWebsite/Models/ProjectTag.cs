using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyActualWebsite.Models
{
    public class ProjectTag
    {
        [ForeignKey("ProjectKey"), Display(Name = "Project")]
        public int? ProjectKey { get; set; }
        public Project? Project { get; set; }

        [ForeignKey("TagID"), Display(Name = "Tag")]
        public int? TagID { get; set; }
        public Tag? Tag { get; set; }

    }
}
