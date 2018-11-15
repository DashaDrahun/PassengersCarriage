using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.BL.Model;
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

        private OrderViewModel GetOrder(int orderId)
        {
            var order = userService.GetOrderById(orderId);

            return mapper.Map<OrderModel, OrderViewModel>(order);
        }

        public ActionResult Trips()
        {
            var model = GetAllTrips();

            return View(model);
        }

        public ActionResult Orders()
        {
            var orders = userService.GetOrders(User.Identity.GetUserId());
            var mappedOrders = mapper.Map<IEnumerable<OrderModel>, List<OrderViewModel>>(orders);

            return View(mappedOrders);
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
                TempData["message"] = result.Message;

                return RedirectToAction(result.Property);
            }

            return View(order);
        }

        public ActionResult EditOrder(int orderId)
        {
            var order = GetOrder(orderId);

            return View(order);
        }

        public ActionResult DetailsOrder(int orderId)
        {
            var order = GetOrder(orderId);
           
            return View(order);
        }

        public ActionResult DeleteOrder(int orderId)
        {
            var result = userService.DeleteOrder(orderId);
            TempData["message"] = result.Message;
            var orders = mapper.Map<IEnumerable<OrderModel>, List<OrderViewModel>>(userService.GetOrders(User.Identity.GetUserId()));

            return View("Orders", orders);
        }

        public PartialViewResult RenderHelloUser()
        {
            var userName = User.Identity;

            return PartialView("_RenderHelloUser", userName);
        }
    }
}