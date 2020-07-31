using RentC.WebUI.VIewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.WebUI.Models.Security.Attribute
{
    public class AdultPerson : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (CustomerRegisterEditVM)validationContext.ObjectInstance;

            if (model.BirthDate == null)
            {
                return new ValidationResult("Date of birth is required.");
            }

            var age = DateTime.Now.Year - model.BirthDate.Value.Year;

            return (age < 18) ?
                new ValidationResult("Customer should be at least 18 years old.") :
                ValidationResult.Success;
        }
    }
}