using System;
using System.ComponentModel.DataAnnotations;
namespace MyActualWebsite.Models
{
	public class Experience
	{
		[Key]
		public int ExperienceKey { get; set; }


        [Display(Name = "Show On Website?")]
        public bool Shown { get; set; } = false;


        [Required, StringLength(60, ErrorMessage = "must be less than 60 charecters"), DataType(DataType.Text), Display(Name = "Companies Name")]
        public string CompanyName { get; set; }


        [StringLength(200, ErrorMessage = "must be less than 200 charecters"), DataType(DataType.Text), Display(Name = "Companies Website Address")]
        public string? link { get; set; }


        [Required, DataType(DataType.Date), Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date), Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }


        [Required, StringLength(30, ErrorMessage = "must be less than 30 charecters"), DataType(DataType.Text), Display(Name = "Position In Company")]
        public string Position { get; set; }


        [Required, DataType(DataType.MultilineText), Display(Name = "Body Text")]
        public string Body { get; set; }

    }
}

