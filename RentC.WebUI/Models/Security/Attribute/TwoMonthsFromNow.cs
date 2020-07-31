using RentC.WebUI.VIewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.WebUI.Models.Security.Attribute
{
    public class TwoMonthsFromNowRegister : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (ReservationRegisterVM)validationContext.ObjectInstance;

            if (model.EndDate == null)
                return new ValidationResult("End date is required.");

            if (model.StartDate == null)
                return new ValidationResult("Start date is required.");

            var rent_period = model.EndDate.Value.Subtract(model.StartDate.Value);

            return (rent_period.TotalDays > 60 ) ?
                new ValidationResult("Maximal rent time two months.") :
                ValidationResult.Success;
        }
    }

    public class TwoMonthsFromNowEdit : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (ReservationEditVM)validationContext.ObjectInstance;

            if (model.EndDate == null)
                return new ValidationResult("End date is required.");

            if (model.StartDate == null)
                return new ValidationResult("Start date is required.");

            var rent_period = model.EndDate.Value.Subtract(model.StartDate.Value);

            return (rent_period.TotalDays > 60) ?
                new ValidationResult("Maximal rent time two months.") :
                ValidationResult.Success;
        }
    }
}