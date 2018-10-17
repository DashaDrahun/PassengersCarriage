using System;
using Autofac;
using Lab06.MVC.Carriage.DAL.Context;
using Lab06.MVC.Carriage.DAL.Interfaces;

namespace Lab06.MVC.Carriage.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PassengersContext context;
        private readonly ILifetimeScope lifetimeScope;
        private readonly IRepositoryFactory repositoryFactory;
        private bool disposed;

        public UnitOfWork(
            PassengersContext context,
            ILifetimeScope lifetimeScope,
            IRepositoryFactory repositoryFactory
        )
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.lifetimeScope = lifetimeScope ?? throw new ArgumentNullException(nameof(lifetimeScope));
            this.repositoryFactory = repositoryFactory;
        }

        public IRepository<T> GetRepository<T>()
        {
            return repositoryFactory.CreateRepository<T>(context, lifetimeScope);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                context.Dispose();
            }

            disposed = true;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
