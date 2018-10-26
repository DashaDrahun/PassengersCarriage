using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Lab06.MVC.Carriage.Models
{
    public class HomeIndexViewModel
    {
        public List<RouteViewModel> AllRoutes { get; set; }
        public List<string> Pictures { get; set; }
    }
}