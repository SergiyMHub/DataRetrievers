﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DataRetrievers.Internal
{
    public class PredicateGuard
    {
        public static IEnumerable<ExpressionType> SupportedBinaryOperators = new[] 
        {
            ExpressionType.GreaterThan,
            ExpressionType.GreaterThanOrEqual,
            ExpressionType.LessThan,
            ExpressionType.LessThanOrEqual,
            ExpressionType.Equal,
            ExpressionType.NotEqual,
            ExpressionType.OrElse,
            ExpressionType.AndAlso,
            
        };

        public static IEnumerable<ExpressionType> SupportedComparisonOperators = new[]
        {
            ExpressionType.GreaterThan,
            ExpressionType.GreaterThanOrEqual,
            ExpressionType.LessThan,
            ExpressionType.LessThanOrEqual,
            ExpressionType.Equal,
            ExpressionType.NotEqual,
        };

        public static IEnumerable<ExpressionType> SupportedUnaryOperators = new[]
        {
            ExpressionType.Convert,
            /*ExpressionType.Not,
            ExpressionType.Constant*/
        };


        /*
          grouping operator
                comparison operator | unary logical operator | binary logical operator | grouping operator | boolean property
         
          comparison operator
                property
                constant 
            
          unary logical operator
                boolean property

          binary logical operator
          
            
                
             */

        public static void PredicateIsSupported<T>(Expression<Func<T, bool>> predicate, string argumentName)
        {
            if (IsBooleanConstant(predicate.Body))
            {
                return;
            }
            else if (IsSupportedUnaryOperation(predicate.Body.NodeType))
            {

            }
            else if (IsSupportedComparison(predicate.Body.NodeType))
            {
                var binaryOperation = predicate.Body as BinaryExpression;
                if (IsSupportedComparisonOperand(binaryOperation.Left) 
                    && IsSupportedComparisonOperand(binaryOperation.Right))
                {
                    return;
                }
            }

            throw new ArgumentException("Not supported predicate type.", argumentName);
        }

        private static bool IsSupportedUnaryOperation(ExpressionType nodeType)
        {
            return SupportedUnaryOperators.Contains(nodeType);
        }

        private static bool IsBooleanConstant(Expression expr)
        {
            return expr.NodeType == ExpressionType.Constant && expr.Type == typeof(bool);
        }

        private static bool IsSupportedComparisonOperand(Expression expr)
        {
            return IsSimpleTypeProperty(expr) || IsSimpleTypeConstant(expr);
        }

        private static bool IsSimpleTypeConstant(Expression expr)
        {
            if (expr.NodeType == ExpressionType.Convert)
            {
                var convertExpr = expr as UnaryExpression;
                return IsSimpleTypeConstant(convertExpr.Operand);
            }

            if (expr is ConstantExpression constantExpr)
            {
                if (IsSimpleType(constantExpr.Type) || IsSimpleNullableType(constantExpr.Type))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsSimpleTypeProperty(Expression expr)
        {
            const MemberTypes fieldOrProperty = MemberTypes.Field | MemberTypes.Property;
            if (expr is MemberExpression memberExpr)
            {
                if (
                    (memberExpr.Member.MemberType & fieldOrProperty) != 0
                    && (IsSimpleType(memberExpr.Type) || IsSimpleNullableType(memberExpr.Type)))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsSimpleNullableType(Type type)
        {
            return
                type.IsGenericType 
                && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                && IsSimpleType(type.GetGenericArguments().Single());
        }

        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive || type == typeof(string);
        }



        private static bool IsSupportedComparison(ExpressionType nodeType)
        {
            return SupportedComparisonOperators.Contains(nodeType);
        }
    }
}