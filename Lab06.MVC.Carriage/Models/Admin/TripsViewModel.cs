using System.Collections.Generic;

namespace Lab06.MVC.Carriage.Models.Admin
{
    public class TripsViewModel
    {
        public IEnumerable<TripViewModel> Trips { get; set; }
        public IEnumerable<RouteViewModel> Routes { get; set; }
        public TripViewModel WrongInputTripViewModel { get; set; }
    }
}