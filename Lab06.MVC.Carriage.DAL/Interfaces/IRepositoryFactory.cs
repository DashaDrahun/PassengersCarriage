using Autofac;
using Lab06.MVC.Carriage.DAL.Context;

namespace Lab06.MVC.Carriage.DAL.Interfaces
{
    public interface IRepositoryFactory
    {
        IRepository<T> CreateRepository<T>(PassengersContext context, ILifetimeScope lifetimeScope);
    }
}
