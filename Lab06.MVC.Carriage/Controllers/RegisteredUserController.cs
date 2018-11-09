using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.DAL.Entities;
using Lab06.MVC.Carriage.Models;
using Microsoft.AspNet.Identity;

namespace Lab06.MVC.Carriage.Controllers
{
    [Authorize]
    public class RegisteredUserController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;


        private List<TripViewModel> GetAllTrips()
        {
            IEnumerable<TripModel> tripModels = userService.GetAllTrips();
            return mapper.Map<IEnumerable<TripModel>, List<TripViewModel>>(tripModels);
        }

        public RegisteredUserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public ActionResult Start()
        {
            var model = GetAllTrips();
            return View("Trips", model);
        }

        public ActionResult Trips()
        {
            var allTripsVm = GetAllTrips();
            return View(allTripsVm);
        }

        public ActionResult CreateOrder(int tripId)
        {
            var trip = userService.GetTripById(tripId);
            var model = new OrderViewModel
            {
                TripId = tripId,
                Trip = mapper.Map<TripModel, TripViewModel>(trip),
                SeatNumber = trip.NumbersOfFreeSeats.Count > 0 
                    ? trip.NumbersOfFreeSeats.First() 
                    : throw new PassengersCarriageValidationException($"No free seats for trip with id {tripId}")
            };

            return View("EditOrder", model);
        }

        [HttpPost]
        public ActionResult EditOrder(OrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                var trip = userService.GetTripById(order.TripId);
                var orderModel = new OrderModel
                {
                    TripId = order.TripId,
                    SeatNumber = order.SeatNumber,
                    UserId = User.Identity.GetUserId(),
                };

                var result = userService.SaveOrder(orderModel);

                if (result)
                {
                    //TempData["message"] = $"Ticket to trip {order.Trip.Route.CityDepart}-{trip.Route.CityArr}" +
                    //                      $"was booked. Your seat number is {order.SeatNumber}";
                    TempData["message"] = $"Ticket to trip {trip.Route.CityDepart}-{trip.Route.CityArr}" +
                                          $" was booked. Your seat number is {order.SeatNumber}";
                }

                return RedirectToAction("Trips");
            }

            return View(order);
        }

        public PartialViewResult RenderHelloUser()
        {
            var userName = User.Identity;
            return PartialView("_RenderHelloUser", userName);
        }
    }
}