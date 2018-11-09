using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
                    cfg.CreateMap<OrderModel, Order>()).CreateMapper();
        }

        public Order MapEntity(OrderModel sourceModel)
        {
            return mapper.Map<OrderModel, Order>(sourceModel);
        }
    }
}
