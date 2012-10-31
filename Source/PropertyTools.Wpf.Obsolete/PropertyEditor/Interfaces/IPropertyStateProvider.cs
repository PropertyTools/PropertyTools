// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyStateProvider.cs" company="PropertyTools">
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
//   Return Enabled, Visible, Error and Warning states for a given component and property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.ComponentModel;

    /// <summary>
    /// Return Enabled, Visible, Error and Warning states for a given component and property.
    /// </summary>
    /// <remarks>
    /// Used in PropertyEditor.
    /// </remarks>
    public interface IPropertyStateProvider
    {
        /// <summary>
        /// The get error.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The get error.
        /// </returns>
        string GetError(object component, PropertyDescriptor descriptor);

        /// <summary>
        /// The get warning.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The get warning.
        /// </returns>
        string GetWarning(object component, PropertyDescriptor descriptor);

        /// <summary>
        /// The is enabled.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The is enabled.
        /// </returns>
        bool IsEnabled(object component, PropertyDescriptor descriptor);

        /// <summary>
        /// The is visible.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The is visible.
        /// </returns>
        bool IsVisible(object component, PropertyDescriptor descriptor);

    }
}