// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EasyInsertAttribute.cs" company="PropertyTools">
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
//   Specify that it should be easy to insert new items in a List property. When the ItemsGrid control is used, the EasyInsert property will be set.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specify that it should be easy to insert new items in a List property. When the ItemsGrid control is used, the EasyInsert property will be set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EasyInsertAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EasyInsertAttribute" /> class.
        /// </summary>
        /// <param name="easyInsert">if set to <c>true</c> [easy insert].</param>
        public EasyInsertAttribute(bool easyInsert)
        {
            this.EasyInsert = easyInsert;
        }

        /// <summary>
        /// Gets or sets a value indicating whether easy insert is enabled.
        /// </summary>
        /// <value><c>true</c> if [easy insert]; otherwise, <c>false</c>.</value>
        public bool EasyInsert { get; set; }
    }
}