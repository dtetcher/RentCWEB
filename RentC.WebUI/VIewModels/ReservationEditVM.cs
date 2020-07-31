using RentC.WebUI.Models.Security.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentC.WebUI.VIewModels
{
    public class ReservationEditVM
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
        [SinceTodayEdit]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        public DateTime StartDateCopy { get; set; }

        [Required]
        //[TwoMonthsFromNowEdit]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        public DateTime EndDateCopy { get; set; }

        public IEnumerable<SelectListItem> ReservStatuses { get; set; }

        [Required]
        [Display(Name = "Status")]
        public byte Status { get; set; }
        public byte StatusCopy { get; set; }

        public IEnumerable<SelectListItem> Locations { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Coupon Code")]
        public string CouponCode { get; set; }
    }
}