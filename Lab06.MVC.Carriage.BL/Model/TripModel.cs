using System;
using System.Collections.Generic;
using Lab06.MVC.Carriage.BL.Interfaces;

namespace Lab06.MVC.Carriage.BL.Model
{
    public class TripModel: IModel
    {
        public int Id { get; set; }

        public int RouteId { get; set; }

        public virtual RouteModel Route { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        public List<int> NumbersOfFreeSeats { get; set; }

        public double Price { get; set; }
    }
}
