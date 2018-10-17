using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.DAL.Entities;
using Lab06.MVC.Carriage.DAL.Interfaces;

namespace Lab06.MVC.Carriage.BL.Services
{
    public class AdminService:IAdminService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Trip> tripRepository;
        private readonly IRepository<Route> routeRepository;

        public AdminService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork
                              ?? throw new ArgumentNullException(nameof(unitOfWork));
            tripRepository = this.unitOfWork.GetRepository<Trip>()
                ??throw new ArgumentNullException(nameof(tripRepository));
            routeRepository = this.unitOfWork.GetRepository<Route>()
                             ?? throw new ArgumentNullException(nameof(routeRepository));
        }

        public IEnumerable<TripModel> GetAllTrips()
        {
            var mapper =
                new MapperConfiguration(cfg =>
                    cfg.CreateMap<Trip, TripModel>().ForSourceMember(x => x.Orders, y => y.Ignore())).CreateMapper();
            return mapper.Map<IEnumerable<Trip>, List<TripModel>>(tripRepository.GetAll());
        }

        public void CreateTrip(TripModel item)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TripModel, Trip>().IgnoreAllVirtual()).CreateMapper();
            var tripPoco = mapper.Map<Trip>(item);
            tripRepository.Create(tripPoco);
            unitOfWork.Save();
        }

        public void UpdateTrip(TripModel item)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TripModel, Trip>().IgnoreAllVirtual()).CreateMapper();
            var tripPoco = mapper.Map<Trip>(item);
            tripRepository.Update(tripPoco);
            unitOfWork.Save();
        }

        public void DeleteTrip(TripModel item)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TripModel, Trip>().IgnoreAllVirtual()).CreateMapper();
            var tripPoco = mapper.Map<Trip>(item);
            tripRepository.Delete(tripPoco);
            unitOfWork.Save();
        }

        public IEnumerable<RouteModel> GetAllRoutes()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Route, RouteModel>().IgnoreAllVirtual()).CreateMapper();
            return mapper.Map<IEnumerable<Route>, List<RouteModel>>(routeRepository.GetAll());
        }

        public void CreateRoute(RouteModel item)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RouteModel, Route>().IgnoreAllVirtual()).CreateMapper();
            var routePoco = mapper.Map<Route>(item);
            if (GetExistedRoute(item)==null)
            {
                routeRepository.Create(routePoco);
                unitOfWork.Save();
            }
            else
            {
                throw new PassengersCarriageValidationException("This route already exists", String.Empty);
            }
        }

        public void UpdateRoute(RouteModel item)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RouteModel, Route>().IgnoreAllVirtual()).CreateMapper();
            var routePoco = mapper.Map<Route>(item);
            if (GetExistedRoute(item) == null || GetExistedRoute(item).RouteId == item.RouteId)
            {
                routeRepository.Update(routePoco);
                unitOfWork.Save();
            }
            else
            {
                throw new PassengersCarriageValidationException("This route already exists", String.Empty);
            }
        }

        public void DeleteRoute(RouteModel item)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RouteModel, Route>().IgnoreAllVirtual()).CreateMapper();
            var routePoco = mapper.Map<Route>(item);
            routeRepository.Delete(routePoco);
            try
            {
                unitOfWork.Save();
            }
            catch
            {
                throw new PassengersCarriageValidationException("This route can't be deleted cause it's used in trips", String.Empty);
            }
        }

        public Route GetExistedRoute(RouteModel route) => routeRepository.Get(r => r.CityArr == route.CityArr && r.CityDepart == route.CityDepart).FirstOrDefault();
    }
}
