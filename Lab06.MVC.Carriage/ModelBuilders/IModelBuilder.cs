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
        RoutesViewModel BuildValidViewModel(IEnumerable<RouteModel> routeModels);
        TripsViewModel BuildValidViewModel(IEnumerable<TripModel> tripModels, IEnumerable<RouteModel> routeModels);
        void RebuildNewInvalidViewModel(RoutesViewModel validModel, RouteViewModel wrongInputModel);
        void RebuildOldItemsInvalidViewModel(RoutesViewModel validModel, RouteViewModel wrongOldModel);
        TripModel BuildTripModel(TripViewModel trip);
        void RebuildNewInvalidViewModel(TripsViewModel validModel, TripViewModel wrongInputModel);
        void RebuildOldItemsInvalidViewModel(TripsViewModel validModel, TripViewModel wrongOldModel);
    }
}
