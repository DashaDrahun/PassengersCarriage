using System.Collections.Generic;
using Lab06.MVC.Carriage.DAL.Interfaces;

namespace Lab06.MVC.Carriage.DAL.Entities
{
    public class Route: IEntity
    {
        public int Id { get; set; }

        public double Kilometres { get; set; }

        public string CityDepart { get; set; }

        public string CityArr { get; set; }

        public virtual ICollection<Trip> Trips { get; private set; }
    }
}
