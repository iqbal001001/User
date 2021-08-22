using User.Domain;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace User.RepositoryInterface
{
    public interface IRepositoryBase<T> where T : class, IData
    {
        IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> path, Expression<Func<T, bool>> filter);
        IQueryable<T> Includes(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
        IQueryable<T> Filter(Expression<Func<T, bool>> filter);
        public IQueryable<T> Get(Expression<Func<T, bool>> filter);
        IQueryable<T> Get();
        void Add(T entity);
        void Delete(object id);
        void Delete(T entityToDelete);
        void DeleteChild(T entityToUpdate);
        void Update(T entityToUpdate);

        bool Any(Expression<Func<T, bool>> filter);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
        Task<List<T>> GetAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
        Task<List<T>> GetAsync(string sort, int skip, int take, Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(object id, CancellationToken cancellationToken = default);
    }
}
