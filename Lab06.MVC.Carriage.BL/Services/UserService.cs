using System;
using System.Collections.Generic;
using System.Linq;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.BL.Mappers;
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
        private readonly ITripMapper tripMapper;
        private readonly IRouteMapper routeMapper;
        private readonly IOrderMapper orderMapper;

        public UserService(IUnitOfWork unitOfWork, ITripMapper tripMapper, IRouteMapper routeMapper, IOrderMapper orderMapper)
        {
            this.unitOfWork = unitOfWork
                ?? throw new ArgumentNullException(nameof(unitOfWork));
            tripRepository = this.unitOfWork.GetRepository<Trip>()
                             ?? throw new ArgumentNullException(nameof(tripRepository));
            routeRepository = this.unitOfWork.GetRepository<Route>()
                              ?? throw new ArgumentNullException(nameof(routeRepository));
            orderRepository = this.unitOfWork.GetRepository<Order>()
                              ?? throw new ArgumentNullException(nameof(orderRepository));
            this.tripMapper = tripMapper
                              ?? throw new ArgumentNullException(nameof(tripMapper));
            this.routeMapper = routeMapper
                               ?? throw new ArgumentNullException(nameof(routeMapper));
            this.orderMapper = orderMapper
                               ?? throw new ArgumentNullException(nameof(orderMapper));
        }

        public TripModel GetTripById(int tripId)
        {
            return tripMapper.MapModel(tripRepository.GetById(tripId));
        }

        public IEnumerable<TripModel> GetAllTrips()
        {
            return tripMapper.MapCollectionModels(tripRepository.GetAll());
        }

        public IEnumerable<RouteModel> GetAllRoutes()
        {
            return routeMapper.MapCollectionModels(routeRepository.GetAll().ToList());
        }

        public bool SaveOrder(OrderModel orderModel)
        {
            var order = orderMapper.MapEntity(orderModel);
            orderRepository.Update(order);
            DecreaseSeatNumbersForTrip(tripRepository.GetById(orderModel.TripId), orderModel.SeatNumber);
            unitOfWork.Save();

            return true;
        }

        private bool DecreaseSeatNumbersForTrip(Trip trip, int seatNumber)
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
