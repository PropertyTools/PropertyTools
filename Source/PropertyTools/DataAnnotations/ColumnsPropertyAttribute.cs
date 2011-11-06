// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnsPropertyAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the name of a property that provides columns for a data grid.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnsPropertyAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnsPropertyAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        public ColumnsPropertyAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }

        #endregion
    }
}