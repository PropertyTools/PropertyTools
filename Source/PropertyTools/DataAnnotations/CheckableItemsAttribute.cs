// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckableItemsAttribute.cs" company="PropertyTools">
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
//   Specifies the name of the properties that controls the IsChecked and Content of checkable items.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the name of the properties that controls the IsChecked and Content of checkable items.
    /// </summary>
    public class CheckableItemsAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableItemsAttribute" /> class.
        /// </summary>
        /// <param name="isCheckedPropertyName">Name of the IsChecked property.</param>
        /// <param name="contentPropertyName">Name of the Content property.</param>
        public CheckableItemsAttribute(string isCheckedPropertyName, string contentPropertyName = "")
        {
            this.IsCheckedPropertyName = isCheckedPropertyName;
            this.ContentPropertyName = contentPropertyName;
        }

        /// <summary>
        /// Gets or sets the name of the IsChecked property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string IsCheckedPropertyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the content property.
        /// </summary>
        /// <value>The name of the content property.</value>
        public string ContentPropertyName { get; set; }
    }
}