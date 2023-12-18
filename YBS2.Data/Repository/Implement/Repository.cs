using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YBS2.Data.Context;

namespace YBS2.Data.Repository.Implement
{
    public class GenericRepository<T> : IGenericRepositoty<T> where T : class
    {
        private readonly YBS2Context _context;
        public GenericRepository(YBS2Context context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }
        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task<T> GetByID(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);
        }
    }
}