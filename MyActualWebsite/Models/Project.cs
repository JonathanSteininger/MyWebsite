using System;
using System.ComponentModel.DataAnnotations;
namespace MyActualWebsite.Models
{
	public class Project
	{
		[Key]
		public int ProjectKey { get; set; }


		[Required, DataType(DataType.Text), StringLength(100, ErrorMessage = "must be less than 100 charecters"), Display(Name = "Title")]
        public string Title { get; set; }

        [Required, DataType(DataType.MultilineText), Display(Name = "Summary Text")]
        public string Summary { get; set; }

        [Required, DataType(DataType.MultilineText), Display(Name = "Body Text")]
        public string Body { get; set; }


		public int BodyLength => Body?.Length ?? 0;


        [DataType(DataType.Text), Display(Name = "Video File Name")]
        public string? VideoFilePath { get; set; }


		[DataType(DataType.Text), Display(Name = "Image File Name")]
		public string? ImageFileName { get; set; }

		public List<Tag> Tags { get; } = new();
		public List<ProjectTag> ProjectTags { get; } = new();

        [DataType(DataType.Text), Display(Name = "Source Code URL")]
        public string? RepositoryURL { get; set; }

		[Display(Name = "Featured Project")]
		public bool Featured { get; set; } = false;


        [DataType(DataType.Date), Display(Name = "Start Date")]
		public DateTime? StartDate { get; set; }

		[DataType(DataType.Date), Display(Name = "End Date")]
		public DateTime? EndDate { get; set;}

	}
}

