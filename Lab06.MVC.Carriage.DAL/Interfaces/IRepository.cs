using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lab06.MVC.Carriage.DAL.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        IEnumerable<T> Get(Expression<Func<T, Boolean>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        void Create(T item);
        void Update(T item);
        void Delete(T item);
    }
}
