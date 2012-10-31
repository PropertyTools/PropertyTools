// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyItemFactory.cs" company="PropertyTools">
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
//   Interface for property item factories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Interface for property item factories.
    /// </summary>
    public interface IPropertyItemFactory
    {
        /// <summary>
        /// Creates a property item.
        /// </summary>
        /// <param name="pd">
        /// The property descriptor.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// A property item.
        /// </returns>
        PropertyItem CreatePropertyItem(PropertyDescriptor pd, object instance);

        /// <summary>
        /// Resets this factory.
        /// </summary>
        /// <remarks>
        /// The factory is reset before the CreatePropertyItem methods are called.
        /// This makes it possible to reset default values.
        /// </remarks>
        void Reset();

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="isEnumerable">if set to <c>true</c> enumerable types instances will use the enumerated objects instead of the instance itself.</param>
        /// <param name="options">The options.</param>
        /// <returns>The tabs.</returns>
        IEnumerable<Tab> CreateModel(object instance, bool isEnumerable, IPropertyControlOptions options);
    }
}