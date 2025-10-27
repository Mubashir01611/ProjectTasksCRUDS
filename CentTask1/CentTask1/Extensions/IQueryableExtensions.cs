using System.Linq.Expressions;

namespace CentTask1.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string propertyName, bool ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, propertyName);
            var exp = Expression.Lambda(prop, param);
            string method = ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { query.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
            return query.Provider.CreateQuery<T>(mce);
        }
    }
}
