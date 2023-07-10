using System;
using System.ComponentModel.DataAnnotations;
namespace MyActualWebsite.Models
{
	public class Mail
	{
		[Key]
		public int MailID { get; set; }


		[Required]
		[StringLength(50, ErrorMessage = "Must be less than 50 charecters")]
        [DataType(DataType.Text)]
		[Display(Name = "Full Name")]
        public string Name { get; set; }


		[EmailAddress]
        [Display(Name = "Email Address")]
        public string? Address { get; set; }


		[Required]
		[StringLength(15000, ErrorMessage = "Must be less than 15000 Charecters")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Body Text")]
        public string Body { get; set; }

	}
}

