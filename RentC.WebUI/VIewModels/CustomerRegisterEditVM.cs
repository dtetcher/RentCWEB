using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using RentC.WebUI.Models.Security.Attribute;


namespace RentC.WebUI.VIewModels
{
    public class CustomerRegisterEditVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]\w+\s[A-Z]\w+$", ErrorMessage = 
            "Please enter valid name and surname.")]
        [MaxLength(25, ErrorMessage = "Name too long")]
        public string Name { get; set; }    
        
        [Required]
        [Display(Name = "Date of birth")]
        [DataType(DataType.DateTime)]
        [RealPerson]
        [AdultPerson]
        public DateTime? BirthDate { get; set; }

        public IEnumerable<SelectListItem> Locations { get; set; }

        [Display(Name = "Location")]
        public int? LocationZipCode { get; set; }
    }
}