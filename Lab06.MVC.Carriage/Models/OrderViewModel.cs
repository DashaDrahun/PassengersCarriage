using System.ComponentModel;
using System.Web.Mvc;

namespace Lab06.MVC.Carriage.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TripId { get; set; }

        public TripViewModel Trip { get; set; }

        [DisplayName("Seat number")]
        public int SeatNumber { get; set; }
    }
}