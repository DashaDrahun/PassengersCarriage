using System.Collections.Generic;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.DAL.Entities;

namespace Lab06.MVC.Carriage.BL.Interfaces
{
    public interface IAdminService
    {
        IEnumerable<TripModel> GetAllTrips();
        OperationDetails CreateTrip(TripModel item);
        OperationDetails UpdateTrip(TripModel item);
        OperationDetails DeleteTrip(TripModel item);

        Route GetExistedRoute(RouteModel route);
        IEnumerable<RouteModel> GetAllRoutes();
        OperationDetails CreateRoute(RouteModel item);
        OperationDetails UpdateRoute(RouteModel item);
        OperationDetails DeleteRoute(RouteModel item);
    }
}
