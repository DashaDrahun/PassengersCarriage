using System.Collections.Generic;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.DAL.Entities;

namespace Lab06.MVC.Carriage.BL.Mappers
{
    public class RouteMapper: IWrapMapper<RouteModel, Route>
    {
        private readonly IMapper mapper;

        public RouteMapper()
        {
            mapper =
                new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Route, RouteModel>();
                        cfg.CreateMap<RouteModel, Route>();
                    })
                    .CreateMapper();
        }

        public Route MapEntity(RouteModel sourceModel)
        {
            return mapper.Map<Route>(sourceModel);
        }

        public IEnumerable<RouteModel> MapCollectionModels(IEnumerable<Route> sourceRoutes)
        {
            return mapper.Map<IEnumerable<Route>, List<RouteModel>>(sourceRoutes);
        }

        public RouteModel MapModel(Route sourceRoute)
        {
            return mapper.Map<Route, RouteModel>(sourceRoute);
        }
    }
}
