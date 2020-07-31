using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.WebUI.VIewModels
{
    public class PermissionRegisterEditVM
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+_[0-9]+$")]
        [MaxLength(10)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Description { get; set; }
    }
}