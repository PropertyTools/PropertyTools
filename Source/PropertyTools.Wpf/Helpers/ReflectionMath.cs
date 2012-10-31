// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionMath.cs" company="PropertyTools">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
//   Addtion, subctraction and multiplication for all kinds of objects (by reflection to invoke the operators).
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Linq;

    /// <summary>
    /// Addtion, subctraction and multiplication for all kinds of objects (by reflection to invoke the operators).
    /// </summary>
    public static class ReflectionMath
    {
        /// <summary>
        /// Performs addition with the op_Addition operator. A return value indicates whether the addition succeeded or failed.
        /// </summary>
        /// <param name="o1">
        /// The first object.
        /// </param>
        /// <param name="o2">
        /// The second object.
        /// </param>
        /// <param name="result">
        /// The sum.
        /// </param>
        /// <returns>
        /// True if the addition succeeded.
        /// </returns>
        public static bool TryAdd(object o1, object o2, out object result)
        {
            if (o1 is double && o2 is double)
            {
                result = (double)o1 + (double)o2;
                return true;
            }

            if (o1 is int && o2 is int)
            {
                result = (int)o1 + (int)o2;
                return true;
            }

            return TryInvoke("op_Addition", o1, o2, out result);
        }

        /// <summary>
        /// Performs multiplication with the op_Multiplication operator. A return value indicates whether the addition succeeded or failed.
        /// </summary>
        /// <param name="o1">
        /// The first object.
        /// </param>
        /// <param name="o2">
        /// The second object.
        /// </param>
        /// <param name="result">
        /// The product.
        /// </param>
        /// <returns>
        /// True if the multiplication succeeded.
        /// </returns>
        public static bool TryMultiply(object o1, object o2, out object result)
        {
            if (o1 is double && o2 is double)
            {
                result = (double)o1 * (double)o2;
                return true;
            }

            if (o1 is int && o2 is int)
            {
                result = (int)o1 * (int)o2;
                return true;
            }

            if (o1 is int && o2 is double)
            {
                result = (int)((int)o1 * (double)o2);
                return true;
            }

            // Implementation of the multiply operator for TimeSpan
            if (o1 is TimeSpan && o2 is double)
            {
                double seconds = ((TimeSpan)o1).TotalSeconds * (double)o2;
                result = TimeSpan.FromSeconds(seconds);
                return true;
            }

            return TryInvoke("op_Multiply", o1, o2, out result);
        }

        /// <summary>
        /// Performs subtraction with the op_Subtraction operator. A return value indicates whether the addition succeeded or failed.
        /// </summary>
        /// <param name="o1">
        /// The first object.
        /// </param>
        /// <param name="o2">
        /// The second object.
        /// </param>
        /// <param name="result">
        /// The difference.
        /// </param>
        /// <returns>
        /// True if the subtraction succeeded.
        /// </returns>
        public static bool TrySubtract(object o1, object o2, out object result)
        {
            if (o1 is double && o2 is double)
            {
                result = (double)o1 - (double)o2;
                return true;
            }

            if (o1 is int && o2 is int)
            {
                result = (int)o1 - (int)o2;
                return true;
            }

            return TryInvoke("op_Subtraction", o1, o2, out result);
        }

        /// <summary>
        /// The try invoke.
        /// </summary>
        /// <param name="methodName">
        /// The method name.
        /// </param>
        /// <param name="o1">
        /// The o 1.
        /// </param>
        /// <param name="o2">
        /// The o 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The try invoke.
        /// </returns>
        private static bool TryInvoke(string methodName, object o1, object o2, out object result)
        {
            try
            {
                var t1 = o1.GetType();
                var t2 = o2.GetType();
                var mi =
                    t1.GetMethods().FirstOrDefault(
                        m => m.Name == methodName && m.GetParameters()[1].ParameterType == t2);
                if (mi == null)
                {
                    result = null;
                    return false;
                }

                result = mi.Invoke(null, new[] { o1, o2 });
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

    }
}