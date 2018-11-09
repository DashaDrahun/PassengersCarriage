using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Infrastructure;
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
                        cfg.CreateMap<Trip, TripModel>()
                            .ForMember(v => v.NumbersOfFreeSeats,
                                opts => opts.MapFrom(src => src.FreeSeetsNumbers.Split(' ').Select(x => Int32.Parse(x))))
                            .ForSourceMember(x => x.Orders, y => y.Ignore()))
                    .CreateMapper();
            return mapper.Map<Trip, TripModel>(tripRepository.GetById(tripId));
        }

        public IEnumerable<TripModel> GetAllTrips()
        {
            var mapper =
                new MapperConfiguration(cfg =>
                        cfg.CreateMap<Trip, TripModel>()
                            .ForMember(v => v.NumbersOfFreeSeats,
                                opts => opts.MapFrom(src => src.FreeSeetsNumbers.Split(' ').Select(x => Int32.Parse(x))))
                            .ForSourceMember(x => x.Orders, y => y.Ignore()))
                    .CreateMapper();
            return mapper.Map<IEnumerable<Trip>, List<TripModel>>(tripRepository.GetAll());
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
            DecreaseSeatsNumberForTrip(tripRepository.GetById(orderModel.TripId), orderModel.SeatNumber);
            unitOfWork.Save();

            return true;
        }

        private bool DecreaseSeatsNumberForTrip(Trip trip, int seatNumber)
        {
            var freeSeatsArrayUpdated = trip.FreeSeetsNumbers.Split(' ').Select(x => Int32.Parse(x)).ToList();
            trip.FreeSeetsNumbers = freeSeatsArrayUpdated.Remove(seatNumber) 
                ? String.Join(" ", freeSeatsArrayUpdated.Select(x => x.ToString())) 
                : throw new PassengersCarriageValidationException($"Seat № {seatNumber} not found in trip № {trip.TripId}");
            tripRepository.Update(trip);

            return true;
        }
    }
}
