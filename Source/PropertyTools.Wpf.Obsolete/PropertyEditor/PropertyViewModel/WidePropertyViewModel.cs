// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidePropertyViewModel.cs" company="PropertyTools">
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
//   Properties marked [WideProperty] are using the full width of the control
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Properties marked [WideProperty] are using the full width of the control
    /// </summary>
    public class WidePropertyViewModel : PropertyViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WidePropertyViewModel"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="showHeader">
        /// The show header.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public WidePropertyViewModel(
            object instance, PropertyDescriptor descriptor, bool showHeader, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
            this.HeaderVisibility = showHeader ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Gets HeaderVisibility.
        /// </summary>
        public Visibility HeaderVisibility { get; private set; }

    }
}