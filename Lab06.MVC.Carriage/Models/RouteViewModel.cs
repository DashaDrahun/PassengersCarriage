using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Lab06.MVC.Carriage.Enums;
using Lab06.MVC.Carriage.UIValidation;

namespace Lab06.MVC.Carriage.Models
{
    public class RouteViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int RouteId { get; set; }

        [Required]
        [Range(1, (int)BelarusInfo.MaxKmSpread, ErrorMessage = "Kilometres must be >= 1")]
        public double Kilometres { get; set; }

        [Required]
        [CityValidation]
        [MinLength(3, ErrorMessage = "CityDepart must be at least 3 letters")]
        [DisplayName("City of departure")]
        public string CityDepart { get; set; }

        [Required]
        [CityValidation]
        [MinLength(3, ErrorMessage = "CityArr must be at least 3 letters")]
        [DisplayName("City of arrival")]
        public string CityArr { get; set; }

        public string HtmlFormatting { get; set; }
    }
}