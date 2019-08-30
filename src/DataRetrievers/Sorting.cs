using DataRetrievers.Internal;
using System;
using System.Linq.Expressions;

namespace DataRetrievers
{
    public class Sorting
    {
        public string FieldName { get; set; }
        public bool IsDescending { get; set; }
    }

    public class Sorting<TProjection>
    {
        public static Sorting Descending<T>(Expression<Func<TProjection, T>> property)
        {
            Guard.ArgumentIsPropertyAccessExpression(property, nameof(property));

            return new Sorting
            {
                FieldName = ExtractFieldName<T>(property),
                IsDescending = true
            };
        }

        public static Sorting Ascending<T>(Expression<Func<TProjection, T>> property)
        {
            Guard.ArgumentIsPropertyAccessExpression(property, nameof(property));

            return new Sorting
            {
                FieldName = ExtractFieldName<T>(property),
                IsDescending = false
            };
        }

        private static string ExtractFieldName<T>(Expression<Func<TProjection, T>> property)
        {
            return (property.Body as MemberExpression).Member.Name;
        }
    }
}