using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzBilling.Database
{
    public interface IRepository<T>
    {
        IQueryable<T> All { get; }
        
        IEnumerable<T> Find(Func<T, bool> predicate);
        void InsertOrUpdate(T entity);
        void Save();
        void Delete(T entity);
        T First(Func<T, bool> predicate);
    }
}
