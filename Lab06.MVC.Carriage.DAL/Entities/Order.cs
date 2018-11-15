using Lab06.MVC.Carriage.DAL.Interfaces;

namespace Lab06.MVC.Carriage.DAL.Entities
{
    public class Order: IEntity
    {
        public int Id { get; set; }

        public int TripId { get; set; }

        public string UserId { get; set; }

        public virtual Trip Trip { get; set; }

        public virtual AppUser User { get; set; }

        public int SeatNumber { get; set; }
    }
}
