using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace DataRetrievers.Internal
{
    public class Guard
    {
        public static void ArgumentNotNull<T>(Nullable<T> argument, string argumentName)
            where T: struct
        {
            if (!argument.HasValue)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void ArgumentNotNull<T>(T argument, string argumentName)
            where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void ArgumentNotNullOrEmpty(string argument, string argumentName)
        {
            Guard.ArgumentConformsPredicate(() => String.IsNullOrEmpty(argument), argumentName, "Argument is null or empty.");
        }

        public static void ArgumentConformsPredicate(Func<bool> predicate, string argumentName)
        {
            ArgumentConformsPredicate(predicate, argumentName, "Argument does not conform predicate.");
        }

        public static void ArgumentConformsPredicate(Func<bool> predicate, string argumentName, string message)
        {
            Guard.ArgumentNotNull(predicate, nameof(predicate));
            Guard.ArgumentNotNull(argumentName, nameof(argumentName));
            Guard.ArgumentNotNull(message, nameof(message));

            if (!predicate())
            {
                throw new ArgumentException(message, argumentName);
            }
        }

        public static void ArgumentIsPropertyAccessExpression<TType, T>(Expression<Func<TType, T>> expr, string argumentName)
        {
            ArgumentIsPropertyAccessExpression(expr, argumentName, "Argument should be a property access expression.");
        }

        public static void ArgumentIsPropertyAccessExpression<TType, T>(Expression<Func<TType, T>> expr, string argumentName, string message)
        {
            Guard.ArgumentNotNull(expr, nameof(expr));

            if (!Check.IsPropertyAccessExpression(expr))
            {
                throw new ArgumentException(message, argumentName);
            }

        }
    }
}
