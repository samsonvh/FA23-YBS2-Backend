using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Service.Utils
{
    public static class LinQUtil
    {
        public static IQueryable<TEntity> SortBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool isAscending)
        {
            var type = typeof(TEntity);
            var property = type.GetProperties().FirstOrDefault(q => q.Name.ToLower() == orderByProperty?.ToLower());
            if (property == null)
            {
                property = type.GetProperties().FirstOrDefault(q => q.Name.ToLower() == "id");
            }
            string command;
            if (isAscending)
            {
                command = "OrderBy";
            }
            else 
            {
                command = "OrderByDescending";
            }
            var parameter = Expression.Parameter(type, "entity"); //entity
            var propertyAccess = Expression.MakeMemberAccess(parameter, property); //entity.property
            var orderByExpression = Expression.Lambda(propertyAccess, parameter); //entity => entity.property
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[]
            {
                type,property.PropertyType
            }, source.Expression, Expression.Quote(orderByExpression)); //OrderBy/OrderByDescending(entity => entity.property)
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
