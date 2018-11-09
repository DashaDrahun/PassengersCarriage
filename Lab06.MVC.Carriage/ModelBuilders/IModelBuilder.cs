using Lab06.MVC.Carriage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.ViewModelsForViews.Admin;

namespace Lab06.MVC.Carriage.ModelBuilders
{
    public interface IModelBuilder
    {
        HomeIndexViewModel Build();
        OrderViewModel BuilOrderViewModel(TripModel trip);
        OrderModel BuildOrderModel(OrderViewModel order, string userId);
        RoutesViewModel BuildValidRoutesViewModel(IEnumerable<RouteModel> routeModels);
        TripsViewModel BuildValidTripsViewModel(IEnumerable<TripModel> tripModels, IEnumerable<RouteModel> routeModels);
        void RebuildNewInvalidRoutesViewModel(RoutesViewModel validModel, RouteViewModel wrongInputModel);
        void RebuildOldItemsInvalidRoutesViewModel(RoutesViewModel validModel, RouteViewModel wrongOldModel);
        TripModel BuildNewTripModel(TripViewModel trip);
        void RebuildNewInvalidTripsViewModel(TripsViewModel validModel, TripViewModel wrongInputModel);
        void RebuildOldItemsInvalidTripsViewModel(TripsViewModel validModel, TripViewModel wrongOldModel);
    }
}
