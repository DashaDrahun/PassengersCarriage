namespace Lab06.MVC.Carriage.BL.Model
{
    public class TripModel
    {
        public int TripId { get; set; }

        public int RouteId { get; set; }

        public virtual RouteModel Route { get; set; }

        public System.DateTime Departure { get; set; }

        public System.DateTime Arrival { get; set; }

        public byte FreeSeetsNumber { get; set; }

        public double Price { get; set; }
    }
}
