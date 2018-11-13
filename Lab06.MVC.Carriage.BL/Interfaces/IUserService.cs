using System.Collections.Generic;
using Lab06.MVC.Carriage.BL.Model;

namespace Lab06.MVC.Carriage.BL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<TripModel> GetAllTrips();
        IEnumerable<RouteModel> GetAllRoutes();
        TripModel GetTripById(int tripId);
        bool SaveOrder(OrderModel orderModel);
        IEnumerable<OrderModel> GetOrders(string userId);
    }
}
