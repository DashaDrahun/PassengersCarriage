using System;
using System.Collections.Generic;

namespace Lab06.MVC.Carriage.BL.Model
{
    public class TripModel
    {
        public int TripId { get; set; }

        public int RouteId { get; set; }

        public virtual RouteModel Route { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        public List<int> NumbersOfFreeSeats { get; set; }

        public double Price { get; set; }
    }
}
