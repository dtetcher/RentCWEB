using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.WebUI.VIewModels
{
    public class UserRegisterEditVM
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}",
            ErrorMessage = "Minimum eight characters, at least one uppercase letter, " +
                            "one lowercase letter, one number and one special character:")]
        public string Password { get; set; }


        [Required]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Roles { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int RoleID { get; set; }
    }
}