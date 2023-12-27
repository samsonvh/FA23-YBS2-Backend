using System.Linq.Expressions;
using System.Reflection;
using YBS2.Service.Utils;

namespace YBS.Service.Utils
{
    public static class LinQUtil
    {
        public static IQueryable<TEntity> SortBy<TEntity>(this IQueryable<TEntity> source, string sortProperty, bool isDescending)
        {
            var type = typeof(TEntity);
            var property = type.GetTypeInfo().GetDeclaredProperty(TextUtils.Capitalize(sortProperty));
            //var property = type.GetProperties().FirstOrDefault(q => q.Name.ToLower() == orderByProperty?.ToLower());
            if (property == null)
            {
                property = type.GetProperties().FirstOrDefault(q => q.Name.ToLower() == "id");
            }
            string methodName = isDescending ? "OrderByDescending" : "OrderBy";

            var parameter = Expression.Parameter(type, "entity"); //entity
            var propertyAccess = Expression.MakeMemberAccess(parameter, property); //entity.property
            var orderByExpression = Expression.Lambda(propertyAccess, parameter); //entity => entity.property
            var typeArguments = new Type[] { type, property.PropertyType };

            var resultExpression = Expression.Call(typeof(Queryable), methodName, typeArguments, source.Expression, Expression.Quote(orderByExpression)); //OrderBy/OrderByDescending(entity => entity.property)
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
