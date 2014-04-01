﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionUtilities.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
        /// <typeparam name="T">
        /// The type of the property.
        /// </typeparam>
        /// <param name="memberAccessExpression">
        /// The member access expression.
        /// </param>
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