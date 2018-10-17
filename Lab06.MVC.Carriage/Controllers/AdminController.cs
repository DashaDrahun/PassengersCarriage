using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.Models;
using Lab06.MVC.Carriage.ViewModelsForViews.Admin;

namespace Lab06.MVC.Carriage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly IMapper mapper;

        public AdminController(IAdminService adminService, IMapper mapper)
        {
            this.adminService = adminService;
            this.mapper = mapper;
        }

        private RoutesViewModel GetAllRoutes()
        {
            IEnumerable<RouteModel> routeModels = adminService.GetAllRoutes();
            var routeVMs = mapper.Map<IEnumerable<RouteModel>, List<RouteViewModel>>(routeModels);

            return new RoutesViewModel
            {
                Routes = routeVMs
            };
        }

        private TripsViewModel GetAllTrips()
        {
            IEnumerable<TripModel> tripModels = adminService.GetAllTrips();
            var tripVMs = mapper.Map<IEnumerable<TripModel>, List<TripViewModel>>(tripModels);

            return new TripsViewModel
            {
                Trips = tripVMs,
                Routes = mapper.Map<IEnumerable<RouteModel>,
                      List<RouteViewModel>>(adminService.GetAllRoutes())
            };
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
                    routeVm.HtmlFormatting = " colorforerror";
                    allRoutesVm.WrongInputRouteViewModel = routeVm;
                }
                else
                {
                    allRoutesVm.Routes.Where(x => x.RouteId == routeVm.RouteId).ToList().ForEach(x =>
                    {
                        x.CityArr = routeVm.CityArr;
                        x.CityDepart = routeVm.CityDepart;
                        x.HtmlFormatting = " colorforerror";
                        x.Kilometres = routeVm.Kilometres;
                    });
                }
            }

            return View(allRoutesVm);
        }

        [HttpPost]
        public ActionResult TripsWork(TripViewModel tripVm, string submitButton)
        {
            if (ModelState.IsValid)
            {
                var tripModel = mapper.Map<TripViewModel, TripModel>(tripVm);
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
                    tripVm.HtmlFormatting = " colorforerror";
                    allTripsVm.WrongInputTripViewModel = tripVm;
                }
                else
                {
                    allTripsVm.Trips.Where(x => x.TripId == tripVm.TripId).ToList().ForEach(x =>
                    {
                        x.ArrivalDate = tripVm.ArrivalDate;
                        x.ArrivalTime = tripVm.ArrivalTime;
                        x.DepartureDate = tripVm.DepartureDate;
                        x.DepartureTime = tripVm.DepartureTime;
                        x.FreeSeetsNumber = tripVm.FreeSeetsNumber;
                        x.Price = tripVm.Price;
                        x.HtmlFormatting = " colorforerror";
                    });
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

