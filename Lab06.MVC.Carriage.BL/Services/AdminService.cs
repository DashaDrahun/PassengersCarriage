using System;
using System.Collections.Generic;
using System.Linq;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.DAL.Entities;
using Lab06.MVC.Carriage.DAL.Interfaces;

namespace Lab06.MVC.Carriage.BL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Trip> tripRepository;
        private readonly IRepository<Route> routeRepository;
        private readonly IWrapMapper<TripModel, Trip> tripMapper;
        private readonly IWrapMapper<RouteModel, Route> routeMapper;

        public AdminService(IUnitOfWork unitOfWork, IWrapMapper<TripModel, Trip> tripMapper, IWrapMapper<RouteModel, Route> routeMapper)
        {
            this.unitOfWork = unitOfWork
                              ?? throw new ArgumentNullException(nameof(unitOfWork));
            tripRepository = this.unitOfWork.GetRepository<Trip>()
                ?? throw new ArgumentNullException(nameof(tripRepository));
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

        public OperationDetails CreateTrip(TripModel item)
        {
            var tripPoco = tripMapper.MapEntity(item);
            var result = tripRepository.Create(tripPoco);
            unitOfWork.Save();

            return new OperationDetails(true, $"Trip with id {result.Id} was successsfully created", "");
        }

        public OperationDetails UpdateTrip(TripModel item)
        {
            var tripPoco = tripMapper.MapEntity(item);

            if (tripPoco.Orders == null || tripPoco.Orders.Count == 0)
            {
                tripRepository.Update(tripPoco);
                unitOfWork.Save();

                return new OperationDetails(true, $"Trip with id {tripPoco.Id} was successsfully updated", "");
            }

            throw new PassengersCarriageValidationException("There are orders for this trip, can't be updated", String.Empty);
        }

        public OperationDetails DeleteTrip(TripModel item)
        {
            var tripPoco = tripMapper.MapEntity(item);

            if (tripPoco.Orders == null || tripPoco.Orders.Count == 0)
            {
                tripRepository.Delete(tripPoco);
                unitOfWork.Save();

                return new OperationDetails(true, $"Trip with id {tripPoco.Id} was successsfully deleted", "");
            }

            throw new PassengersCarriageValidationException("There are orders for this trip, can't be deleted", String.Empty);
        }

        public IEnumerable<RouteModel> GetAllRoutes()
        {
            return routeMapper.MapCollectionModels(routeRepository.GetAll());
        }

        public OperationDetails CreateRoute(RouteModel item)
        {
            var routePoco = routeMapper.MapEntity(item);

            if (GetExistedRoute(item) == null)
            {
                var result = routeRepository.Create(routePoco);
                unitOfWork.Save();

                return new OperationDetails(true, $"Route with id {result.Id} was successsfully created", "");
            }

            throw new PassengersCarriageValidationException("This route already exists", String.Empty);
        }

        public OperationDetails UpdateRoute(RouteModel item)
        {
            var routePoco = routeMapper.MapEntity(item);

            if (GetExistedRoute(item) == null || GetExistedRoute(item).Id == item.Id)
            {
                routeRepository.Update(routePoco);
                unitOfWork.Save();

                return new OperationDetails(true, $"Route with id {routePoco.Id} was successsfully updated", "");
            }

            throw new PassengersCarriageValidationException("This route already exists", String.Empty);
        }

        public OperationDetails DeleteRoute(RouteModel item)
        {
            var routePoco = routeMapper.MapEntity(item);
            var result = routeRepository.Delete(routePoco);
            try
            {
                unitOfWork.Save();

                return new OperationDetails(true, $"Route with id {result.Id} was successsfully deleted", "");
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
