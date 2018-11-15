using System.Collections.Generic;

namespace Lab06.MVC.Carriage.Models
{
    public class HomeIndexViewModel
    {
        public List<RouteViewModel> AllRoutes { get; set; }
        public List<string> Pictures { get; set; }
    }
}