// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FontPreviewAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
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
        /// Initializes a new instance of the <see cref="FontPreviewAttribute" /> class.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="weight">The weight.</param>
        public FontPreviewAttribute(double size, int weight = 500)
        {
            this.Size = size;
            this.Weight = weight;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontPreviewAttribute" /> class.
        /// </summary>
        /// <param name="fontFamilyPropertyName">Name of the font family property.</param>
        /// <param name="size">The size.</param>
        /// <param name="weight">The weight.</param>
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