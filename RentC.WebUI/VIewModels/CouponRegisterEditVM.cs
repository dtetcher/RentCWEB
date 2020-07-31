using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace RentC.WebUI.VIewModels
{
    public class CouponRegisterEditVM
    {
        [Required]
        [RegularExpression(@"[\w\d]{7}", ErrorMessage = "Coupon Code can contain only digits or literals.")]
        [Display(Name = "Code")]
        public string CouponCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 99)]
        public decimal? Discount { get; set; }
    }
}