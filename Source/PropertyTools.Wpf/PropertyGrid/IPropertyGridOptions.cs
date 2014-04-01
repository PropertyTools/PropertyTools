// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyGridOptions.cs" company="PropertyTools">
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
//   Specifies options for the PropertyGrid
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;

    /// <summary>
    /// Specifies options for the PropertyGrid
    /// </summary>
    public interface IPropertyGridOptions
    {
        /// <summary>
        /// Gets the required (opt-in) attribute.
        /// </summary>
        /// <value> The required attribute. </value>
        Type RequiredAttribute { get; }

        /// <summary>
        /// Gets a value indicating whether to show declared properties only.
        /// </summary>
        /// <value> <c>true</c> if only declared properties should be shown; otherwise, <c>false</c> . </value>
        bool ShowDeclaredOnly { get; }

        /// <summary>
        /// Gets a value indicating whether to show read only properties.
        /// </summary>
        /// <value> <c>true</c> if read only properties should be shown; otherwise, <c>false</c> . </value>
        bool ShowReadOnlyProperties { get; }
    }
}