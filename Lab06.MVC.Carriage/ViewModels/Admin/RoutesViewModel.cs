using System.Collections.Generic;
using Lab06.MVC.Carriage.Models;

namespace Lab06.MVC.Carriage.ViewModelsForViews.Admin
{
    public class RoutesViewModel
    {
        public IEnumerable<RouteViewModel> Routes { get; set; }
        public RouteViewModel WrongInputRouteViewModel { get; set; }
    }
}