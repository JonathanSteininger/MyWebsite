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

        [DataType(DataType.Text)]
        [Display(Name = "Image FileName")]
        public string? ImageFileName { get; set; }

		[DataType(DataType.Text)]
        [Display(Name = "Video File Path")]
        public string? VideoFilePath { get; set; }

		[DataType(DataType.Text)]
		[Display(Name = "GitHub Resipitory")]
		public string? ResipitoryUrl { get; set;}

    }
}

