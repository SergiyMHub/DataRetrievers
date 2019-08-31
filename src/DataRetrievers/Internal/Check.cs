using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DataRetrievers.Internal
{
    public class Check
    {
        public static bool IsPropertyAccessExpression<TType, T>(Expression<Func<TType, T>> expr)
        {
            Guard.ArgumentNotNull(expr, nameof(expr));

            return expr.Body.NodeType == ExpressionType.MemberAccess && (expr.Body as MemberExpression).Member.MemberType == MemberTypes.Property;
        }

        public static bool HasProperty<TType>(string fieldName)
        {
            return typeof(TType).GetProperties().Any(p => p.Name == fieldName);
        }
    }
}
