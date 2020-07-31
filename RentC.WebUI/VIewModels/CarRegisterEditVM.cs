using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentC.WebUI.VIewModels
{
    public class CarRegisterEditVM
    {
        [Key]
        public int CarID { get; set; }

        [Required(ErrorMessage = "Plate field is required")]
        [RegularExpression(@"[A-Z]{2}\s[0-9]{2}\s[A-Z]{3}",
            ErrorMessage = "Invalid plate.\nFormat - AB 12 CDE.")]
        public string Plate { get; set; }

        [Required(ErrorMessage = "Manufacturer field is required")]
        [StringLength(20, MinimumLength = 3)]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Model field is required")]
        [StringLength(25, MinimumLength = 1)]
        public string Model { get; set; }

        [Required(ErrorMessage = "Daily price field is required")]
        [DataType(DataType.Currency)]
        [Range(50, 500)]
        [Display(Name = "Daily Price")]
        public decimal? DailyPrice { get; set; }

        public IEnumerable<SelectListItem> Locations { get; set; }

        [Required(ErrorMessage = "Location field is required")]
        [Display(Name = "Location")]
        public int LocationZipCode { get; set; }
    }
}