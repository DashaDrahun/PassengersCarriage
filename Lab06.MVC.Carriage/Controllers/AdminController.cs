using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.ModelBuilders;
using Lab06.MVC.Carriage.Models;
using Lab06.MVC.Carriage.Models.Admin;

namespace Lab06.MVC.Carriage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly IMapper mapper;
        private readonly IModelBuilder modelBuilder;

        public AdminController(IAdminService adminService, IMapper mapper, IModelBuilder modelBuilder)
        {
            this.adminService = adminService
                                ?? throw new ArgumentNullException(nameof(adminService));
            this.mapper = mapper
                          ?? throw new ArgumentNullException(nameof(mapper));
            this.modelBuilder = modelBuilder
                                ?? throw new ArgumentNullException(nameof(modelBuilder));
        }

        private RoutesViewModel GetAllRoutes()
        {
            IEnumerable<RouteModel> routeModels = adminService.GetAllRoutes();

            return modelBuilder.BuildValidViewModel(routeModels);
        }

        private TripsViewModel GetAllTrips()
        {
            var tripModels = adminService.GetAllTrips();
            var routeModels = adminService.GetAllRoutes();

            return modelBuilder.BuildValidViewModel(tripModels, routeModels);
        }

        [HttpPost]
        public ActionResult RoutesWork(RouteViewModel routeVm, string submitButton)
        {
            RoutesViewModel model;

            if (ModelState.IsValid)
            {
                var routeModel = mapper.Map<RouteViewModel, RouteModel>(routeVm);
                var userMessage = "";

                try
                {
                    switch (submitButton)
                    {
                        case "Create":
                            userMessage = adminService.CreateRoute(routeModel).Message;
                            break;
                        case "Update":
                            userMessage = adminService.UpdateRoute(routeModel).Message;
                            break;
                        case "Delete":
                            userMessage = adminService.DeleteRoute(routeModel).Message;
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    TempData["message"] = userMessage;
                }
                catch (PassengersCarriageValidationException e)
                {
                    ModelState.AddModelError(String.Empty, e.Message);
                }

                model = GetAllRoutes();
            }

            else
            {
                model = GetAllRoutes();

                if (routeVm.Id == 0)
                {
                    modelBuilder.RebuildNewInvalidViewModel(model, routeVm);
                }
                else
                {
                    modelBuilder.RebuildOldItemsInvalidViewModel(model, routeVm);
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult TripsWork(TripViewModel tripVm, string submitButton)
        {
            TripsViewModel model;

            if (ModelState.IsValid)
            {
                var tripModel = modelBuilder.BuildTripModel(tripVm);
                var userMessage = "";

                try
                {
                    switch (submitButton)
                    {
                        case "Create":
                            userMessage = adminService.CreateTrip(tripModel).Message;
                            break;
                        case "Update":
                            userMessage = adminService.UpdateTrip(tripModel).Message;
                            break;
                        case "Delete":
                            userMessage = adminService.DeleteTrip(tripModel).Message;
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    TempData["message"] = userMessage;
                }
                catch (PassengersCarriageValidationException e)
                {
                    ModelState.AddModelError(String.Empty, e.Message);
                }

                model = GetAllTrips();
            }
            else
            {
                model = GetAllTrips();

                if (tripVm.Id == 0)
                {
                    modelBuilder.RebuildNewInvalidViewModel(model, tripVm);
                }
                else
                {
                    modelBuilder.RebuildOldItemsInvalidViewModel(model, tripVm);
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult RoutesWork()
        {
            var model = GetAllRoutes();

            return View(model);
        }

        [HttpGet]
        public ActionResult TripsWork()
        {
            var model = GetAllTrips();

            return View(model);
        }

        public PartialViewResult RenderHelloUser()
        {
            var user = User.Identity;

            return PartialView("_RenderHelloUser", user);
        }
    }
}

