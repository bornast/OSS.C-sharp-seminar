using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> Query();

        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> whereExpression);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereExpression);

        Task<T> FindAsync(object id);

        Task<IEnumerable<T>> GetAllAsync();

        T Single(int id);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entity);

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Save();

        Task SaveAsync();
    }
}
