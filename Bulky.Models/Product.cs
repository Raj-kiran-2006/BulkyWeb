using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using BulkyWeb.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bulky.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title{ get; set; }
        [Required]
        public String Author { get; set; }
        [Required]
        public String Description { get; set; }
        [Required]
        public String ISBN { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "List Price")]
        public int ListPrice { get; set; }


        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for 1-50")]
        public int Price { get; set; }



        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for 50+")]
        public int Price50 { get; set; }


        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for 100+")]
        public int Price100 { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Select a category.")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public string? SellerId { get; set; }

        [ForeignKey(nameof(SellerId))]
        public ApplicationUser? Seller { get; set; }

        [ValidateNever]
        public String ImageUrl { get; set; }




    }
}
