using System;
using System.ComponentModel.DataAnnotations;
namespace MyActualWebsite.Models
{
	public class FAQ
	{
		[Key]
		public int FAQ_ID { get; set; }


		[Required]
		[DataType(DataType.MultilineText)]
		[StringLength(300, ErrorMessage = "Must be less than 300 charecters")]
        [Display(Name = "Question")]
        public string Question { get; set; }


        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(300, ErrorMessage = "Must be less than 300 charecters")]
		[Display(Name = "Answer")]
        public string Answer { get; set; }
	}
}

