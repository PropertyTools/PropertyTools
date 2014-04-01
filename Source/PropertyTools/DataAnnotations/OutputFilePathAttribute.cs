// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OutputFilePathAttribute.cs" company="PropertyTools">
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
//   Specifies that the decorated property is an output file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the decorated property is an output file.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OutputFilePathAttribute : InputFilePathAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFilePathAttribute" /> class.
        /// </summary>
        /// <param name="defaultExtension">The default extension.</param>
        /// <param name="filter">The filter.</param>
        public OutputFilePathAttribute(string defaultExtension, string filter)
            : base(defaultExtension, filter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFilePathAttribute" /> class.
        /// </summary>
        /// <param name="defaultExtension">The default extension.</param>
        public OutputFilePathAttribute(string defaultExtension)
            : base(defaultExtension)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFilePathAttribute" /> class.
        /// </summary>
        public OutputFilePathAttribute()
        {
        }
    }
}