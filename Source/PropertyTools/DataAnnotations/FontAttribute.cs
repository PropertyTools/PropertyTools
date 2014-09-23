// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FontAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the font family, size and weight.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the font family, size and weight.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FontAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FontAttribute" /> class.
        /// </summary>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The size.</param>
        /// <param name="fontWeight">The weight.</param>
        public FontAttribute(string fontFamily, double fontSize = double.NaN, int fontWeight = 500)
        {
            this.FontFamily = fontFamily;
            this.FontSize = fontSize;
            this.FontWeight = fontWeight;
        }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>The font family.</value>
        public string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        /// <value>The size.</value>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        /// <value>The weight.</value>
        public int FontWeight { get; set; }
    }
}