using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using EzBilling.Models;

namespace EzBilling.Database
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {

        protected DbContext Model;

        protected Repository(DbContext model)
        {
            Model = model;
        }
    
        public IQueryable<T> All
        {
            get
            {
                return Model.Set<T>();
            }
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return Model.Set<T>().Where(predicate).ToList();
        }

        abstract public void InsertOrUpdate(T entity);

        public void Save()
        {
            Model.SaveChanges();
        }

        public void Delete(T entity)
        {
            Model.Set<T>().Remove(entity);
        }

        public T First(Func<T, bool> predicate)
        {
            return Find(predicate).FirstOrDefault(predicate);
        }
    }
}
