using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentC.WebUI.Models.Security.Attribute;

namespace RentC.WebUI.VIewModels
{
    public class ReservationRegisterVM
    {
        [Required]
        [ForeignKey("Car")]
        [Display(Name = "Car ID")]
        public int? CarID { get; set; }

        
        [Required]
        [ForeignKey("Customer")]
        [Display(Name = "Customer ID")]
        public int? CustomerID { get; set; }

        [Required]
        [SinceTodayRegister]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Required]
        //[TwoMonthsFromNowRegister]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public IEnumerable<SelectListItem> Locations { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Coupon Code")]
        public string CouponCode { get; set; }
    }
}