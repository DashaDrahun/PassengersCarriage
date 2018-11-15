using System;
using System.Collections.Generic;
using Lab06.MVC.Carriage.DAL.Interfaces;

namespace Lab06.MVC.Carriage.DAL.Entities
{
    public class Trip: IEntity
    {
        public int Id { get; set; }

        public int RouteId { get; set; }

        public virtual Route Route { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        public string FreeSeetsNumbers { get; set; }

        public double Price { get; set; }

        public virtual ICollection<Order> Orders { get; private set; }
    }
}

