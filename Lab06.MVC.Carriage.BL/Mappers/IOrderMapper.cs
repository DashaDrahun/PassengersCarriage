using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.DAL.Entities;

namespace Lab06.MVC.Carriage.BL.Mappers
{
    public interface IOrderMapper
    {
        Order MapEntity(OrderModel sourceModel);
        OrderModel MapModel(Order sourceModel);
        IEnumerable<OrderModel> MapCollectionModels(IEnumerable<Order> sourceOrders);
    }
}
