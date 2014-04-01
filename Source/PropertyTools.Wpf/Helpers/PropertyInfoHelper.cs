// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyInfoHelper.cs" company="PropertyTools">
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
//   The property info helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The property info helper.
    /// </summary>
    public class PropertyInfoHelper
    {
        /// <summary>
        /// Gets the value of the specified property of the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>
        /// The get property.
        /// </returns>
        public static object GetPropertyValue(object instance, string propertyName)
        {
            var pi = instance.GetType().GetProperty(propertyName);
            return pi.GetValue(instance, null);
        }

        /// <summary>
        /// The set property.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The value.</param>
        public static void SetPropertyValue(object instance, string propertyName, object value)
        {
            var pi = instance.GetType().GetProperty(propertyName);
            pi.SetValue(instance, value, null);
        }
    }
}