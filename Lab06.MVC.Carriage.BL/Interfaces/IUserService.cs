using System.Collections.Generic;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Model;

namespace Lab06.MVC.Carriage.BL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<TripModel> GetAllTrips();
        IEnumerable<RouteModel> GetAllRoutes();
        TripModel GetTripById(int tripId);
        OperationDetails SaveOrder(OrderModel orderModel);
        IEnumerable<OrderModel> GetOrders(string userId);
        OrderModel GetOrderById(int orderId);
        OperationDetails DeleteOrder(int orderId);
    }
}
