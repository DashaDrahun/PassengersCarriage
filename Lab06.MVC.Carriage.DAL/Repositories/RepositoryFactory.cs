using Autofac;
using Lab06.MVC.Carriage.DAL.Context;
using Lab06.MVC.Carriage.DAL.Interfaces;

namespace Lab06.MVC.Carriage.DAL.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public IRepository<T> CreateRepository<T>(PassengersContext context, ILifetimeScope lifetimeScope)
        {
            return lifetimeScope.Resolve<IRepository<T>>(new NamedParameter("context", context));
        }
    }
}
