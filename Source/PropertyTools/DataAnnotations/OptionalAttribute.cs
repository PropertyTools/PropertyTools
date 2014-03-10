// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionalAttribute.cs" company="PropertyTools">
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
//   Specifies that the property is optional.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property is optional.
    /// </summary>
    /// <remarks>
    /// The name of a property that controls the state of the property can also be specified.
    /// Properties marked with [Optional] will have a checkbox as the label.
    /// The checkbox will enable/disable the property value editor.
    /// Example usage:
    /// [Optional]                    // requires a <see cref="Nullable"/> property type
    /// [Optional("HasSomething")]    // relates to other property HasSomething
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionalAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "OptionalAttribute" /> class.
        /// </summary>
        public OptionalAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        public OptionalAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }
    }
}