using System.Collections.Generic;

namespace Lab06.MVC.Carriage.DAL.Entities
{
    public class Route
    {
        public int RouteId { get; set; }

        public double Kilometres { get; set; }

        public string CityDepart { get; set; }

        public string CityArr { get; set; }

        public virtual ICollection<Trip> Trips { get; private set; }
    }
}
