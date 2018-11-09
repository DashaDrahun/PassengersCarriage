using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Lab06.MVC.Carriage.Enums;
using Lab06.MVC.Carriage.UIValidation;

namespace Lab06.MVC.Carriage.Models
{
    public class TripViewModel: IValidatableObject
    {
        [CombinedTripValidation("DepartureDate", "DepartureTime", "ArrivalDate", "ArrivalTime")]
        [HiddenInput(DisplayValue = false)]
        public int TripId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int RouteId { get; set; }

        public virtual RouteViewModel Route { get; set; }

        [Required]
        [DisplayName("Date of departure")]
        public System.DateTime DepartureDate { get; set; }

        [Required]
        [DisplayName("Time of departure")]
        public System.DateTime DepartureTime { get; set; }

        [Required]
        [DisplayName("Date of arrival")]
        public System.DateTime ArrivalDate { get; set; }

        [Required]
        [DisplayName("Time of arrival")]
        public System.DateTime ArrivalTime { get; set; }

        [Required]
        public int FreeSeatNumber { get; set; }

        [Required]
        [Range(3, 1000, ErrorMessage = "Price must be > 3")]
        public double Price { get; set; }

        public List<int> NumbersOfFreeSeats { get; set; }

        public string HtmlFormatting { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            var maxNumberSeats = Enum.GetValues(typeof(MotorVehiclesSeets)).Cast<int>().Max();

            if (FreeSeatNumber <= 0 || FreeSeatNumber > maxNumberSeats)
            {
                errors.Add(new ValidationResult("FreeSeetsNumber must be > 0 and <= max seets number in motor vehicle"));
            }

            return errors;
        }
    }
}