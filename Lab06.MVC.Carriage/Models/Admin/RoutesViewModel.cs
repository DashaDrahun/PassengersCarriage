using System.Collections.Generic;

namespace Lab06.MVC.Carriage.Models.Admin
{
    public class RoutesViewModel
    {
        public IEnumerable<RouteViewModel> Routes { get; set; }
        public RouteViewModel WrongInputRouteViewModel { get; set; }
    }
}