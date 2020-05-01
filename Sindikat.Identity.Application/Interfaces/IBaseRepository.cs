using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IBaseRepository<T> where T: class
    {
        IQueryable<T> Query();

        T Single(int id);

        void Persist(T entity);

        void PersistRange(IEnumerable<T> entities);

        void Flush();

        void FlushAsync();
    }
}
