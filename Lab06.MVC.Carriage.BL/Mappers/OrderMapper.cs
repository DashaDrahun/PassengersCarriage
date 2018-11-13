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
    public class OrderMapper: IOrderMapper
    {
        private readonly IMapper mapper;

        public OrderMapper()
        {
            mapper =
                new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Trip, TripModel>()
                            .ForMember(v => v.NumbersOfFreeSeats,
                                opts => opts.MapFrom(src =>
                                    !String.IsNullOrWhiteSpace(src.FreeSeetsNumbers)
                                        ? src.FreeSeetsNumbers.Split(' ').Select(x => Int32.Parse(x))
                                        : new List<int>()))
                            .ForSourceMember(x => x.Orders, y => y.Ignore());
                        cfg.CreateMap<TripModel, Trip>().IgnoreAllVirtual();
                        cfg.CreateMap<OrderModel, Order>();
                        cfg.CreateMap<Order, OrderModel>().ForSourceMember(x => x.User, y => y.Ignore());
                    })
                    .CreateMapper();
        }

        public Order MapEntity(OrderModel sourceModel)
        {
            return mapper.Map<OrderModel, Order>(sourceModel);
        }

        public IEnumerable<OrderModel> MapCollectionModels(IEnumerable<Order> sourceOrders)
        {
            return mapper.Map<IEnumerable<Order>, IEnumerable<OrderModel>>(sourceOrders);
        }
    }
}
