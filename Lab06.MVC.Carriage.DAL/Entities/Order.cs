namespace Lab06.MVC.Carriage.DAL.Entities
{
    public class Order
    {
        public int OrderId { get; set; }

        public int TripId { get; set; }

        public string UserId { get; set; }

        public virtual Trip Trip { get; set; }

        public virtual AppUser User { get; set; }

        public int SeatNumber { get; set; }
    }
}
