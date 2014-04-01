// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeHelper.cs" company="PropertyTools">
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
//   The attribute helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// The attribute helper.
    /// </summary>
    public static class AttributeHelper
    {
        /// <summary>
        /// Check if an attribute collection contains an attribute of the given type
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="attributeType">The type to check for.</param>
        /// <returns>
        /// The contains attribute of type.
        /// </returns>
        public static bool ContainsAttributeOfType(AttributeCollection attributes, Type attributeType)
        {
            // return attributes.Cast<object>().Any(a => attributeType.IsAssignableFrom(a.GetType()));))))
            foreach (var a in attributes)
            {
                if (attributeType.IsAssignableFrom(a.GetType()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Return the first attribute of a given type for the specified property descriptor.
        /// </summary>
        /// <typeparam name="T">An attribute type.</typeparam>
        /// <param name="descriptor">The property descriptor.</param>
        /// <returns>
        /// The first attribute of the specified type.
        /// </returns>
        public static T GetFirstAttribute<T>(PropertyDescriptor descriptor) where T : Attribute
        {
            return descriptor.Attributes.OfType<T>().FirstOrDefault();
        }
    }
}