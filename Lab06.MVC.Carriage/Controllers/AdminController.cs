using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.DAL.Entities;
using Lab06.MVC.Carriage.ModelBuilders;
using Lab06.MVC.Carriage.Models;
using Lab06.MVC.Carriage.ViewModelsForViews.Admin;

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
            if (ModelState.IsValid)
            {
                var routeModel = mapper.Map<RouteViewModel, RouteModel>(routeVm);
                try
                {
                    switch (submitButton)
                    {
                        case "Create":
                            adminService.CreateRoute(routeModel);
                            break;
                        case "Update":
                            adminService.UpdateRoute(routeModel);
                            break;
                        case "Delete":
                            adminService.DeleteRoute(routeModel);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                catch (PassengersCarriageValidationException e)
                {
                    ModelState.AddModelError(String.Empty, e.Message);
                }
            }

            var allRoutesVm = GetAllRoutes();

            if (!ModelState.IsValid)
            {
                if (routeVm.RouteId == 0)
                {
                    modelBuilder.RebuildNewInvalidViewModel(allRoutesVm, routeVm);
                }
                else
                {
                    modelBuilder.RebuildOldItemsInvalidViewModel(allRoutesVm, routeVm);
                }
            }

            return View(allRoutesVm);
        }

        [HttpPost]
        public ActionResult TripsWork(TripViewModel tripVm, string submitButton)
        {
            if (ModelState.IsValid)
            {
                var tripModel = modelBuilder.BuildTripModel(tripVm);

                try
                {
                    switch (submitButton)
                    {
                        case "Create":
                            adminService.CreateTrip(tripModel);
                            break;
                        case "Update":
                            adminService.UpdateTrip(tripModel);
                            break;
                        case "Delete":
                            adminService.DeleteTrip(tripModel);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                catch (PassengersCarriageValidationException e)
                {
                    ModelState.AddModelError(String.Empty, e.Message);
                }
            }

            var allTripsVm = GetAllTrips();

            if (!ModelState.IsValid)
            {
                if (tripVm.TripId == 0)
                {
                    modelBuilder.RebuildNewInvalidViewModel(allTripsVm, tripVm);
                }
                else
                {
                    modelBuilder.RebuildOldItemsInvalidViewModel(allTripsVm, tripVm);
                }
            }
            return View(allTripsVm);
        }

        [HttpGet]
        public ActionResult RoutesWork()
        {
            var allRoutesVm = GetAllRoutes();
            return View(allRoutesVm);
        }

        [HttpGet]
        public ActionResult TripsWork()
        {
            var allTripsVm = GetAllTrips();
            return View(allTripsVm);
        }

        public PartialViewResult RenderHelloUser()
        {
            var user = User.Identity;
            return PartialView("_RenderHelloUser", user);
        }
    }
}

