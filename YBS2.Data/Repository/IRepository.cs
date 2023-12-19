using System.Linq.Expressions;

namespace YBS2.Data.Repository
{
    public interface IGenericRepositoty<T> where T : class
    {
        #region CRUD Database
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(T entity);
        #endregion

        #region Query Database
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> expression);
        Task<T> GetByID(int id);
        void RemoveRange(IEnumerable<T> entities);
        #endregion
    }
}