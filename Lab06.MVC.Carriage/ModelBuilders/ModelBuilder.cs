using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.Models;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.ViewModelsForViews.Admin;

namespace Lab06.MVC.Carriage.ModelBuilders
{
    public class ModelBuilder : IModelBuilder
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public ModelBuilder(IUserService userService, IMapper mapper)
        {
            this.userService = userService
                                ?? throw new ArgumentNullException(nameof(userService));
            this.mapper = mapper
                          ?? throw new ArgumentNullException(nameof(mapper));
        }

        public HomeIndexViewModel Build()
        {
            string pathToPictures = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images"));
            var directory = new DirectoryInfo(pathToPictures);
            var files = directory.GetFiles();
            List<string> picturesPathes = new List<string>();
            foreach (var file in files)
            {
                picturesPathes.Add(Path.Combine(file.Directory.Name, file.Name));
            }

            IEnumerable<RouteModel> routeModels = userService.GetAllRoutes();
            List<RouteViewModel> allRoutesVm = mapper.Map<IEnumerable<RouteModel>, List<RouteViewModel>>(routeModels);

            return new HomeIndexViewModel
            {
                AllRoutes = allRoutesVm,
                Pictures = picturesPathes
            };
        }

        public OrderViewModel BuilOrderViewModel(TripModel trip)
        {
            return new OrderViewModel
            {
                TripId = trip.TripId,
                Trip = mapper.Map<TripModel, TripViewModel>(trip),
                SeatNumber = trip.NumbersOfFreeSeats.Count > 0
                    ? trip.NumbersOfFreeSeats.First()
                    : throw new PassengersCarriageValidationException($"No free seats for trip with id {trip.TripId}")
            };
        }

        public OrderModel BuildOrderModel(OrderViewModel order, string userId)
        {
            return new OrderModel
            {
                TripId = order.TripId,
                SeatNumber = order.SeatNumber,
                UserId = userId
            };
        }

        public RoutesViewModel BuildValidRoutesViewModel(IEnumerable<RouteModel> routeModels)
        {
            var routeVMs = mapper.Map<IEnumerable<RouteModel>, List<RouteViewModel>>(routeModels);

            return new RoutesViewModel
            {
                Routes = routeVMs
            };
        }

        public void RebuildNewInvalidRoutesViewModel(RoutesViewModel validModel, RouteViewModel wrongInputModel)
        {
            wrongInputModel.HtmlFormatting = " colorforerror";
            validModel.WrongInputRouteViewModel = wrongInputModel;
        }

        public void RebuildOldItemsInvalidRoutesViewModel(RoutesViewModel validModel, RouteViewModel wrongOldModel)
        {
            validModel.Routes.Where(x => x.RouteId == wrongOldModel.RouteId).ToList().ForEach(x =>
            {
                x.CityArr = wrongOldModel.CityArr;
                x.CityDepart = wrongOldModel.CityDepart;
                x.HtmlFormatting = " colorforerror";
                x.Kilometres = wrongOldModel.Kilometres;
            });
        }

        public TripModel BuildNewTripModel(TripViewModel trip)
        {
            var tripModel = mapper.Map<TripViewModel, TripModel>(trip);
            tripModel.NumbersOfFreeSeats = Enumerable.Range(1, trip.FreeSeatNumber).ToList();
            return tripModel;
        }

        public TripsViewModel BuildValidTripsViewModel(IEnumerable<TripModel> tripModels,
            IEnumerable<RouteModel> routeModels)
        {
            var tripVMs = mapper.Map<IEnumerable<TripModel>, List<TripViewModel>>(tripModels);
            return new TripsViewModel
            {
                Trips = tripVMs,
                Routes = mapper.Map<IEnumerable<RouteModel>,
                    List<RouteViewModel>>(routeModels)
            };
        }

        public void RebuildNewInvalidTripsViewModel(TripsViewModel validModel, TripViewModel wrongInputModel)
        {
            wrongInputModel.HtmlFormatting = " colorforerror";
            validModel.WrongInputTripViewModel = wrongInputModel;
        }

        public void RebuildOldItemsInvalidTripsViewModel(TripsViewModel validModel, TripViewModel wrongOldModel)
        {
            validModel.Trips.Where(x => x.TripId == wrongOldModel.TripId).ToList().ForEach(x =>
            {
                x.ArrivalDate = wrongOldModel.ArrivalDate;
                x.ArrivalTime = wrongOldModel.ArrivalTime;
                x.DepartureDate = wrongOldModel.DepartureDate;
                x.DepartureTime = wrongOldModel.DepartureTime;
                x.FreeSeatNumber = wrongOldModel.FreeSeatNumber;
                x.Price = wrongOldModel.Price;
                x.HtmlFormatting = " colorforerror";
                x.FreeSeatNumber = wrongOldModel.FreeSeatNumber;
            });
        }
    }
}