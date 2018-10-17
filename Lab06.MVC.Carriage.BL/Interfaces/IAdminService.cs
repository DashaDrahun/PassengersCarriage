using System.Collections.Generic;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.DAL.Entities;

namespace Lab06.MVC.Carriage.BL.Interfaces
{
    public interface IAdminService
    {
        IEnumerable<TripModel> GetAllTrips();
        void CreateTrip(TripModel item);
        void UpdateTrip(TripModel item);
        void DeleteTrip(TripModel item);

        Route GetExistedRoute(RouteModel route);
        IEnumerable<RouteModel> GetAllRoutes();
        void CreateRoute(RouteModel item);
        void UpdateRoute(RouteModel item);
        void DeleteRoute(RouteModel item);
    }
}
