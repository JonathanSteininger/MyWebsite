using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyActualWebsite.Models
{
	public class StatBar
	{
		[Key]
		public int StatBarID { get; set; }


		[Range(0,100, ErrorMessage = "must be between 0 and 100")]
		[Required]
        [Display(Name = "Percentage (0~100)")]
        public int Precentage { get; set; }


		[DataType(DataType.Text)]
		[StringLength(100, ErrorMessage = "File name must be less than 100 charecters.")]
		[IllegalCharecters(ErrorMessage = "Illegal Charecters detected")]
        [Display(Name = "Icon File Path")]
        public string? IconPath { get; set; }

		[ForeignKey("StatBarCatagoryID"), Display(Name = "Catagory")]
		public int? StatBarCatagoryID { get; set; }
		public StatBarCatagory? StatBarCatagory { get; set;}
	}

}

