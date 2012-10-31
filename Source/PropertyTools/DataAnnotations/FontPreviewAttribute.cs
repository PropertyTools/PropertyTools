// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FontPreviewAttribute.cs" company="PropertyTools">
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
//   Specifies the font size, weight and the name of a property that contains the font family.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the font size, weight and the name of a property that contains the font family.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FontPreviewAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FontPreviewAttribute"/> class.
        /// </summary>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="weight">
        /// The weight.
        /// </param>
        public FontPreviewAttribute(double size, int weight = 500)
        {
            this.Size = size;
            this.Weight = weight;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontPreviewAttribute"/> class.
        /// </summary>
        /// <param name="fontFamilyPropertyName">
        /// Name of the font family property.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="weight">
        /// The weight.
        /// </param>
        public FontPreviewAttribute(string fontFamilyPropertyName, double size, int weight = 500)
        {
            this.FontFamilyPropertyName = fontFamilyPropertyName;
            this.Size = size;
            this.Weight = weight;
        }

        /// <summary>
        /// Gets or sets the name of the font family property.
        /// </summary>
        /// <value>The name of the font family property.</value>
        public string FontFamilyPropertyName { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public double Size { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>The weight.</value>
        public int Weight { get; set; }

    }
}