using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using RentC.WebUI.Models.DAL;
using RentC.WebUI.Infrastructure.Concrete;

namespace RentC.WebUI.Models
{
    public class UserLoginVM
    {
        [Required(ErrorMessage = "Login is required")]
        [Display(Name = "Login")]
        public int? UserID { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}