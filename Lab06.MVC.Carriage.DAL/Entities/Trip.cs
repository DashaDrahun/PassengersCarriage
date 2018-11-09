using System;
using System.Collections.Generic;

namespace Lab06.MVC.Carriage.DAL.Entities
{
    public class Trip
    {
        public int TripId { get; set; }

        public int RouteId { get; set; }

        public virtual Route Route { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        //todo: serialise: int[] -> string -> xml 
        public string FreeSeetsNumbers { get; set; }

        public double Price { get; set; }

        public virtual ICollection<Order> Orders { get; private set; }
    }
}

