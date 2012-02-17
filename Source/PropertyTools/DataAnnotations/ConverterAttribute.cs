// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies a converter that should be used for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ConverterAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterAttribute"/> class.
        /// </summary>
        /// <param name="converterType">
        /// Type of the converter.
        /// </param>
        public ConverterAttribute(Type converterType)
        {
            this.ConverterType = converterType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the type of the converter.
        /// </summary>
        /// <value>The type of the converter.</value>
        public Type ConverterType { get; set; }

        #endregion
    }
}