// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePathAttribute.cs" company="PropertyTools">
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
//   Specifies that the property is a file path.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property is a file path.
    /// </summary>
    /// <remarks>
    /// Note that this can be combined with the BasePathAttribute.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilePathAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilePathAttribute"/> class.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="defaultExt">
        /// The default extension.
        /// </param>
        /// <param name="useOpenDialog">
        /// if set to <c>true</c> [use open dialog].
        /// </param>
        public FilePathAttribute(string filter, string defaultExt, bool useOpenDialog = true)
        {
            this.Filter = filter;
            this.DefaultExtension = defaultExt;
            this.UseOpenDialog = useOpenDialog;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePathAttribute"/> class.
        /// </summary>
        /// <param name="defaultExt">
        /// The default extension.
        /// </param>
        /// <param name="useOpenDialog">
        /// if set to <c>true</c> [use open dialog].
        /// </param>
        public FilePathAttribute(string defaultExt, bool useOpenDialog = true)
        {
            this.DefaultExtension = defaultExt;
            this.UseOpenDialog = useOpenDialog;
        }

        /// <summary>
        /// Gets or sets the default extension.
        /// </summary>
        /// <value>The default extension.</value>
        public string DefaultExtension { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use open dialog].
        /// </summary>
        /// <value><c>true</c> if [use open dialog]; otherwise, <c>false</c>.</value>
        public bool UseOpenDialog { get; set; }

    }
}