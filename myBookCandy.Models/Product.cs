using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myBookCandy.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		public string Description { get; set; }

		public string Author { get; set; }

		[Required]
		[Display(Name ="Category")]
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		[ValidateNever]
		public Category Category { get; set; }

		[Required]
		[Display(Name ="Cover Type")]
		public int CoverTypeId { get; set; }
		[ForeignKey("CoverTypeId")]
		[ValidateNever]
		public CoverType CoverType { get; set; }

		public string ISBN { get; set; }

		[Required]
		[Display(Name = "List Price")]
		[Range(1, 10000)]
		public double ListPrice { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Price - 50")]
        [Range(1, 10000)]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "Price - 100")]
        [Range(1, 10000)]
        public double Price100 { get; set; }

		[Display(Name ="Image")]
		[ValidateNever]
		public string ImageUrl { get; set; }
	}
}
