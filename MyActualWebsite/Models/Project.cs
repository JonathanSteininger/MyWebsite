using System;
using System.ComponentModel.DataAnnotations;
namespace MyActualWebsite.Models
{
	public class Project
	{
		[Key]
		public int ProjectKey { get; set; }


		[DataType(DataType.Text)]
		[Required]
		[StringLength(100, ErrorMessage = "must be less than 100 charecters")]
        [Display(Name = "Title")]
        public string Title { get; set; }


		[Required]
		[DataType(DataType.MultilineText)]
        [Display(Name = "Body Text")]
        public string Body { get; set; }


		[IllegalCharecters(ErrorMessage = "Illegal Charecters Detected")]
		[DataType(DataType.Text)]
        [Display(Name = "Video File Path")]
        public string? VideoFilePath { get; set; }
	}
}

