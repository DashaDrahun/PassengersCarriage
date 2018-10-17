using System;

namespace Lab06.MVC.Carriage.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>();
        void Save();
    }
}
