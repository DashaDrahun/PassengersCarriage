using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IWrapMapper<TripModel, Trip> tripMapper;
        private readonly IWrapMapper<RouteModel, Route> routeMapper;
        private readonly IWrapMapper<OrderModel, Order> orderMapper;

        public UserService(IUnitOfWork unitOfWork, IWrapMapper<TripModel, Trip> tripMapper, IWrapMapper<RouteModel, Route> routeMapper, IWrapMapper<OrderModel, Order> orderMapper)
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

        public OrderModel GetOrderById(int orderId)
        {
            return orderMapper.MapModel(orderRepository.GetById(orderId));
        }

        public OperationDetails DeleteOrder(int orderId)
        {
            var order = orderRepository.Delete(orderRepository.GetById(orderId));
            AttachSeatNumberToTrip(tripRepository.GetById(order.TripId), order.SeatNumber);
            unitOfWork.Save();

            return new OperationDetails(true, $"Order with id {order.Id} was successfully deleted", "");
        }

        public IEnumerable<TripModel> GetAllTrips()
        {
            var trips = tripMapper.MapCollectionModels(tripRepository.GetAll());
            return trips.Where(x => x.NumbersOfFreeSeats.Count > 0);
        }

        public IEnumerable<RouteModel> GetAllRoutes()
        {
            return routeMapper.MapCollectionModels(routeRepository.GetAll().ToList());
        }

        public OperationDetails SaveOrder(OrderModel orderModel)
        {
            var userMessage = "";
            var view = "";

            if (orderModel.Id != 0)
            {
                var oldOrder = orderRepository.GetById(orderModel.Id);
                AttachSeatNumberToTrip(oldOrder.Trip, oldOrder.SeatNumber);
                userMessage = !(orderModel.SeatNumber == oldOrder.SeatNumber)
                    ? $"Order with id {orderModel.Id} was successfully updated." +
                              $"Your seat number is changed from {oldOrder.SeatNumber} on {orderModel.SeatNumber}"
                                    : $"Your seat number is stayed the same: {oldOrder.SeatNumber}";
                view = "Orders";
            }

            DetachSeatNumberFromTrip(tripRepository.GetById(orderModel.TripId), orderModel.SeatNumber);
            orderRepository.Update(orderMapper.MapEntity(orderModel));
            unitOfWork.Save();

            return new OperationDetails(true,
                String.IsNullOrEmpty(userMessage) ? $"Order was successfully created. " +
                                                    $"Your seat number is {orderModel.SeatNumber}"
                    : userMessage, String.IsNullOrEmpty(view) ? "Trips" : view);
        }

        public IEnumerable<OrderModel> GetOrders(string userId)
        {
            return orderMapper.MapCollectionModels(orderRepository.Get(x => x.UserId == userId));
        }

        private void DetachSeatNumberFromTrip(Trip trip, int seatNumber)
        {
            var seatsArray = trip.FreeSeetsNumbers.Split(' ').Select(x => Int32.Parse(x)).ToList();
            trip.FreeSeetsNumbers = seatsArray.Remove(seatNumber)
                ? String.Join(" ", seatsArray.Select(x => x.ToString()))
                : throw new PassengersCarriageValidationException($"Seat № {seatNumber} not found in trip № {trip.Id}");
            tripRepository.Update(trip);
        }

        private void AttachSeatNumberToTrip(Trip trip, int seatNumber)
        {
            var seatsArray = !String.IsNullOrEmpty(trip.FreeSeetsNumbers)
                ? trip.FreeSeetsNumbers.Split(' ').Select(x => Int32.Parse(x)).ToList()
                : new List<int>();
            seatsArray.Add(seatNumber);
            seatsArray.Sort();
            trip.FreeSeetsNumbers = String.Join(" ", seatsArray.Select(x => x.ToString()));
            tripRepository.Update(trip);
        }
    }
}
