using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Lab06.MVC.Carriage.BL.Infrastructure;

namespace Lab06.MVC.Carriage.UIValidation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CombinedTripValidationAttribute : ValidationAttribute
    {
        public CombinedTripValidationAttribute(
            string departureDate,
            string departureTime,
            string arrivalDate,
            string arrivalTime)
        {
            DepartureDateAttr = departureDate;
            DepartureTimeAttr = departureTime;
            ArrivalDateAttr = arrivalDate;
            ArrivaTimeAttr = arrivalTime;
        }

        public string DepartureDateAttr { get; private set; }
        public string DepartureTimeAttr { get; private set; }
        public string ArrivalDateAttr { get; private set; }
        public string ArrivaTimeAttr { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            StringBuilder errorsResult = new StringBuilder();
            var departureDateprop = validationContext.ObjectType.GetProperty(DepartureDateAttr);
            var departureTimeprop = validationContext.ObjectType.GetProperty(DepartureTimeAttr);
            var arrivalDateprop = validationContext.ObjectType.GetProperty(ArrivalDateAttr);
            var arrivalTimeprop = validationContext.ObjectType.GetProperty(ArrivaTimeAttr);
            if (departureDateprop != null && departureTimeprop != null && arrivalDateprop != null &&
                arrivalTimeprop != null)
            {
                DateTime departureDate = (DateTime)departureDateprop.GetValue(validationContext.ObjectInstance, null);
                DateTime departureTime = (DateTime)departureTimeprop.GetValue(validationContext.ObjectInstance, null);
                DateTime arrivalDate = (DateTime)arrivalDateprop.GetValue(validationContext.ObjectInstance, null);
                DateTime arrivalTime = (DateTime)arrivalTimeprop.GetValue(validationContext.ObjectInstance, null);
                DateTime departureResult = new DateTime(
                    departureDate.Year,
                    departureDate.Month,
                    departureDate.Day,
                    departureTime.Hour,
                    departureTime.Minute,
                    0);
                DateTime arrivalResult = new DateTime(
                    arrivalDate.Year,
                    arrivalDate.Month,
                    arrivalDate.Day,
                    arrivalTime.Hour,
                    arrivalTime.Minute,
                    0);
                if (departureResult.CompareTo(DateTime.Now.AddHours(2)) < 0)
                {
                    errorsResult.Append($"Departure must later than {DateTime.Now.AddHours(2).ToString("g")}.");
                }
                if (arrivalResult.CompareTo(departureResult) <= 0)
                {
                    return new ValidationResult(errorsResult.Append(" Arrival must be later than departure")
                        .ToString());
                }
                return ValidationResult.Success;
            }
           throw new PassengersCarriageValidationException("WrongUsageAttribute", "CombinedTripValidationAttribute can be used only with Datetime fields!");
        }
    }
}