using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using Lab06.MVC.Carriage.DAL.Context;
using Lab06.MVC.Carriage.DAL.Interfaces;

namespace Lab06.MVC.Carriage.DAL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly PassengersContext context;
        private readonly DbSet<T> entities;

        public BaseRepository(PassengersContext context)
        {
            this.context = context ??
                           throw new ArgumentNullException(nameof(context));
            entities = this.context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return entities.ToList();
        }

        public T GetById(int id)
        {
            return entities.Find(id);
        }

        public IEnumerable<T> Get(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = entities;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            else
            {
                GetAll();
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public T Create(T item)
        {
            return entities.Add(item);
        }

        public void Update(T item)
        {
            context.Set<T>().AddOrUpdate(item);
        }

        public T Delete(T item)
        {
            if (context.Entry(item).State == EntityState.Detached)
            {
                entities.Attach(item);
            }

            return entities.Remove(item);
        }
    }
}


