using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.DAL.Entities;
using Lab06.MVC.Carriage.ModelBuilders;
using Lab06.MVC.Carriage.Models;
using Microsoft.AspNet.Identity;

namespace Lab06.MVC.Carriage.Controllers
{
    [Authorize]
    public class RegisteredUserController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IModelBuilder modelBuilder;

        public RegisteredUserController(IUserService userService, IMapper mapper, IModelBuilder modelBuilder)
        {
            this.userService = userService
                               ?? throw new ArgumentNullException(nameof(userService));
            this.mapper = mapper
                          ?? throw new ArgumentNullException(nameof(mapper));
            this.modelBuilder = modelBuilder
                                ?? throw new ArgumentNullException(nameof(modelBuilder));
        }


        private List<TripViewModel> GetAllTrips()
        {
            IEnumerable<TripModel> tripModels = userService.GetAllTrips();
            return mapper.Map<IEnumerable<TripModel>, List<TripViewModel>>(tripModels);
        }

        public ActionResult Trips()
        {
            var allTripsVm = GetAllTrips();
            return View(allTripsVm);
        }

        public ActionResult CreateOrder(int tripId)
        {
            var trip = userService.GetTripById(tripId);
            var model = modelBuilder.BuilOrderViewModel(trip);

            return View("EditOrder", model);
        }

        [HttpPost]
        public ActionResult EditOrder(OrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                var orderModel = modelBuilder.BuildOrderModel(order, User.Identity.GetUserId());
                var result = userService.SaveOrder(orderModel);

                if (result)
                {
                    var trip = userService.GetTripById(order.TripId);
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