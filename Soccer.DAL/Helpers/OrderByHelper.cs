using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Soccer.DAL.Helpers
{
    public static class OrderByHelper
    {
        public static IOrderedMongoQueryable<T> OrderBy<T>(this IMongoQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        public static IOrderedMongoQueryable<T> OrderByDescending<T>(this IMongoQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
    }
}
