using Lab06.MVC.Carriage.BL.Interfaces;

namespace Lab06.MVC.Carriage.BL.Model
{
    public class OrderModel: IModel
    {
        public int Id { get; set; }

        public int TripId { get; set; }

        public string UserId { get; set; }

        public int SeatNumber { get; set; }

        public virtual TripModel Trip { get; set; }
    }
}
