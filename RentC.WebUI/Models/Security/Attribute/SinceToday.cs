using RentC.WebUI.VIewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.WebUI.Models.Security.Attribute
{
    public class SinceTodayRegister : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (ReservationRegisterVM)validationContext.ObjectInstance;

            if (model.StartDate == null)
                return new ValidationResult("Start date is required.");

            if (model.EndDate == null)
                return new ValidationResult("End date is required.");

            if (model.StartDate < DateTime.Now)
                return new ValidationResult("Start date can't be in the past.");

            if (model.StartDate > model.EndDate)
                return new ValidationResult("Start date must be less than end date.");

            var rent_period = model.EndDate.Value.Subtract(model.StartDate.Value);

            if (rent_period.TotalDays > 60)
                return new ValidationResult("Maximal rent time two months.");

            return ValidationResult.Success;
        }
    }

    public class SinceTodayEdit : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (ReservationEditVM)validationContext.ObjectInstance;

            if (model.StartDate == null)
                return new ValidationResult("Start date is required.");

            if (model.EndDate == null)
                return new ValidationResult("End date is required.");

            if (model.StartDate < DateTime.Now)
                return new ValidationResult("Start date can't be in the past.");

            if (model.StartDate > model.EndDate)
                return new ValidationResult("Start date must be less than end date.");

            var rent_period = model.EndDate.Value.Subtract(model.StartDate.Value);

            if (rent_period.TotalDays > 60)
                return new ValidationResult("Maximal rent time two months.");

            return ValidationResult.Success;
        }
    }
}