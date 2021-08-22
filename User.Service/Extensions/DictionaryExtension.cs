using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace User.Service.Extensions
{
    public static class DictionaryExtension
    {
        public static ExpressionStarter<T> GetExpression<T>(this Dictionary<string, string> dict)
        {
            var parameter = Expression.Parameter(typeof(T), "user");

            var predicate = PredicateBuilder.New<T>();

            predicate = predicate.And(_ => true);

            if (dict is null) return predicate;

            foreach (var pair in dict)
            {
                var key = pair.Key.FirstCharToUpper();
                Expression<Func<T, bool>> filterExpression = null;
                switch (typeof(T).GetProperty(key).PropertyType.Name.ToLower())
                {
                    case "int":
                        if (int.TryParse(pair.Value, out int intValue))
                        {
                            var intComparison = Expression.Call(Expression.Property(parameter, typeof(T).GetProperty(key)),
                                   typeof(string).GetMethod("Equals", new[] { typeof(string) }),
                                   Expression.Constant(intValue));
                            filterExpression = Expression.Lambda<Func<T, bool>>(intComparison, parameter);
                        }
                        break;
                    case "string":
                        var stringComparison = Expression.Call(Expression.Property(parameter, typeof(T).GetProperty(key)),
                                   typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                                   Expression.Constant(pair.Value));
                        filterExpression = Expression.Lambda<Func<T, bool>>(stringComparison, parameter);
                        break;
                    case "boolean":
                        if (bool.TryParse(pair.Value, out bool boolValue))
                        {
                            var booleanComparison = Expression.Call(Expression.Property(parameter, typeof(T).GetProperty(key)),
                                        typeof(bool).GetMethod("Equals", new[] { typeof(bool) }),
                                        Expression.Constant(boolValue));
                            filterExpression = Expression.Lambda<Func<T, bool>>(booleanComparison, parameter);
                        }
                        break;
                    default:

                        break;
                }
                // var comparison = Expression.Equal(Expression.Property(parameter, typeof(T).GetProperty(key)), Expression.Constant(pair.Value));

                // comparison = Expression.Call(Expression.Property(parameter, typeof(T).GetProperty(key)),
                //typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                //Expression.Constant(pair.Value));
                // var filterExpression = Expression.Lambda<Func<T, bool>>(comparison, parameter);
                if (filterExpression !=  null) predicate = predicate.And(filterExpression);

            }
            //var comparison = Expression.Equal(Expression.Property(parameter, typeof(UserInfo).GetProperty("firstName")), Expression.Constant(""));

            //var filterExpression = Expression.Lambda<Func<UserInfo, bool>>(comparison, parameter);

            // var discountedAlbums = albums.Where(discountFilterExpression);
            return predicate;
        }
    }
}
