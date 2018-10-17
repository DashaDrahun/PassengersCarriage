using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.DAL.Entities;
using Lab06.MVC.Carriage.DAL.Interfaces;

namespace Lab06.MVC.Carriage.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Trip> tripRepository;
        private readonly IRepository<Route> routeRepository;
        private readonly IRepository<Order> orderRepository;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork
                ?? throw new ArgumentNullException(nameof(unitOfWork));
            tripRepository = this.unitOfWork.GetRepository<Trip>()
                             ?? throw new ArgumentNullException(nameof(tripRepository));
            routeRepository = this.unitOfWork.GetRepository<Route>()
                              ?? throw new ArgumentNullException(nameof(routeRepository));
            orderRepository = this.unitOfWork.GetRepository<Order>()
                              ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public TripModel GetTripById(int tripId)
        {
            var mapper =
                new MapperConfiguration(cfg =>
                    cfg.CreateMap<Trip, TripModel>().ForSourceMember(x => x.Orders, y => y.Ignore())).CreateMapper();
            return mapper.Map<Trip, TripModel>(tripRepository.GetById(tripId));
        }

        public IEnumerable<TripModel> GetAllTrips()
        {
            var mapper =
                new MapperConfiguration(cfg =>
                    cfg.CreateMap<Trip, TripModel>().ForSourceMember(x => x.Orders, y => y.Ignore())).CreateMapper();
            return mapper.Map<IEnumerable<Trip>, List<TripModel>>(tripRepository.Get(x => x.Departure.CompareTo(DateTime.Now) > 0 && x.FreeSeetsNumber>0));
        }

        public IEnumerable<RouteModel> GetAllRoutes()
        {
            var mapper =
                new MapperConfiguration(cfg =>
                    cfg.CreateMap<Route, RouteModel>().ForSourceMember(x => x.Trips, y => y.Ignore())).CreateMapper();
            return mapper.Map<List<Route>, List<RouteModel>>(routeRepository.GetAll().ToList());
        }

        public bool SaveOrder(OrderModel orderModel)
        {
            var mapper =
                new MapperConfiguration(cfg =>
                    cfg.CreateMap<OrderModel, Order>()).CreateMapper();

            var order = mapper.Map<OrderModel, Order>(orderModel);

            orderRepository.Update(order);
            DecreaseSeatsNumberForTrip(tripRepository.GetById(orderModel.TripId));
            unitOfWork.Save();

            return true;
        }

        private bool DecreaseSeatsNumberForTrip(Trip trip)
        {
            trip.FreeSeetsNumber -= 1;
            tripRepository.Update(trip);
            return true;
        }
    }
}
