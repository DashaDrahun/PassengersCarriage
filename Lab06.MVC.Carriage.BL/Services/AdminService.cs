using System;
using System.Collections.Generic;
using System.Linq;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.BL.Mappers;
using Lab06.MVC.Carriage.DAL.Entities;
using Lab06.MVC.Carriage.DAL.Interfaces;

namespace Lab06.MVC.Carriage.BL.Services
{
    public class AdminService:IAdminService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Trip> tripRepository;
        private readonly IRepository<Route> routeRepository;
        private readonly ITripMapper tripMapper;
        private readonly IRouteMapper routeMapper;

        public AdminService(IUnitOfWork unitOfWork, ITripMapper tripMapper, IRouteMapper routeMapper)
        {
            this.unitOfWork = unitOfWork
                              ?? throw new ArgumentNullException(nameof(unitOfWork));
            tripRepository = this.unitOfWork.GetRepository<Trip>()
                ??throw new ArgumentNullException(nameof(tripRepository));
            routeRepository = this.unitOfWork.GetRepository<Route>()
                             ?? throw new ArgumentNullException(nameof(routeRepository));
            this.tripMapper = tripMapper
                ?? throw new ArgumentNullException(nameof(tripMapper));
            this.routeMapper = routeMapper
                              ?? throw new ArgumentNullException(nameof(routeMapper));
        }

        public IEnumerable<TripModel> GetAllTrips()
        {
            return tripMapper.MapCollectionModels(tripRepository.GetAll());
        }

        public void CreateTrip(TripModel item)
        {
            var tripPoco = tripMapper.MapEntity(item);
            tripRepository.Create(tripPoco);
            unitOfWork.Save();
        }

        public void UpdateTrip(TripModel item)
        {
            // todo: проверка на существование заказа, если да - отменить обновление, выдать сообщение
            var tripPoco = tripMapper.MapEntity(item);
            tripRepository.Update(tripPoco);
            unitOfWork.Save();
        }

        public void DeleteTrip(TripModel item)
        {
            // todo: проверка на существование заказа, если да - отменить удаление, выдать сообщение
            var tripPoco = tripMapper.MapEntity(item);
            tripRepository.Delete(tripPoco);
            unitOfWork.Save();
        }

        public IEnumerable<RouteModel> GetAllRoutes()
        {
            return routeMapper.MapCollectionModels(routeRepository.GetAll());
        }

        public void CreateRoute(RouteModel item)
        {
            var routePoco = routeMapper.MapEntity(item);

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
            var routePoco = routeMapper.MapEntity(item);

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
            var routePoco = routeMapper.MapEntity(item);
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

        public Route GetExistedRoute(RouteModel route) => 
            routeRepository.Get(r => r.CityArr == route.CityArr 
                                     && r.CityDepart == route.CityDepart)
                .FirstOrDefault();
    }
}
