// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionUtilities.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides utility methods for lambda expressions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides utility methods for lambda expressions.
    /// </summary>
    public static class ExpressionUtilities
    {
        /// <summary>
        /// Gets the name of the property specified by an expression.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="memberAccessExpression">The member access expression.</param>
        /// <returns>
        /// The name of the property.
        /// </returns>
        public static string GetName<T>(Expression<Func<T>> memberAccessExpression)
        {
            var expression = memberAccessExpression.Body;
            switch (memberAccessExpression.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expression).Member.Name;
                case ExpressionType.Call:
                    return ((MethodCallExpression)expression).Method.Name;
                case ExpressionType.Convert:
                    var memberExpression = ((UnaryExpression)expression).Operand as MemberExpression;
                    if (memberExpression != null)
                    {
                        return memberExpression.Member.Name;
                    }

                    break;
            }

            return string.Empty;
        }
    }
}