using System;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using Autofac;
using AutoMapper;
using Lab06.MVC.Carriage.Models;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.Filters;

namespace Lab06.MVC.Carriage
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterModule(new AutofacConfig());

            IMapper mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RouteViewModel, RouteModel>();
                cfg.CreateMap<RouteModel, RouteViewModel>()
                    .ForMember(
                        x => x.HtmlFormatting,
                        opt => opt.MapFrom(src => String.Empty));
                cfg.CreateMap<TripViewModel, TripModel>().ForMember(
                    v => v.Arrival,
                    opt => opt.MapFrom(
                        source => new DateTime(
                            source.ArrivalDate.Year,
                            source.ArrivalDate.Month,
                        source.ArrivalDate.Day,
                            source.ArrivalTime.Hour,
                            source.ArrivalTime.Minute,
                            0))).ForMember(
                    v => v.Departure,
                    opt => opt.MapFrom(
                        source => new DateTime(
                            source.DepartureDate.Year,
                            source.DepartureDate.Month,
                        source.DepartureDate.Day,
                            source.DepartureTime.Hour,
                            source.DepartureTime.Minute,
                            0)));
                cfg.CreateMap<TripModel, TripViewModel>()
                    .ForMember(
                        v => v.ArrivalDate,
                        opts => opts.MapFrom(src => src.Arrival.Date))
                    .ForMember(
                        v => v.ArrivalTime,
                        opts => opts.MapFrom(src => src.Arrival))
                    .ForMember(
                        v => v.DepartureTime,
                        opts => opts.MapFrom(src => src.Departure))
                    .ForMember(
                        v => v.DepartureDate,
                        opts => opts.MapFrom(src => src.Departure.Date))
                    .ForMember(
                        v => v.Route,
                        opt => opt.MapFrom(src => src.Route))
                    .ForMember(
                        v => v.HtmlFormatting,
                        opt => opt.MapFrom(src => String.Empty))
                    .ForMember(
                        v => v.FreeSeatNumber,
                        // todo: надо ли так делать?
                        opts => opts.MapFrom(src => src.NumbersOfFreeSeats.Count));
            }).CreateMapper();

            builder.RegisterInstance(mapper).As<IMapper>().SingleInstance();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new HandleExceptionAttribute());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
