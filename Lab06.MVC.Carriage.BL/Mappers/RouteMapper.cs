using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.DAL.Entities;

namespace Lab06.MVC.Carriage.BL.Mappers
{
    public class RouteMapper: IRouteMapper
    {
        private readonly IMapper mapper;

        public RouteMapper()
        {
            mapper =
                new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Route, RouteModel>().IgnoreAllVirtual();
                        cfg.CreateMap<RouteModel, Route>().IgnoreAllVirtual();
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
