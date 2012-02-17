// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionalAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property is optional.
    /// </summary>
    /// <remarks>
    /// The name of a property that controls the state of the property can also be specified.
    ///   Properties marked with [Optional] will have a checkbox as the label.
    ///   The checkbox will enable/disable the property value editor.
    ///   Example usage:
    ///   [Optional]                    // requires a nullable property type
    ///   [Optional("HasSomething")]    // relates to other property HasSomething
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionalAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "OptionalAttribute" /> class.
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