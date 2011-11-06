// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResettableAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property is resettable.
    /// </summary>
    /// <remarks>
    /// Properties marked with [Resettable] will have a reset button.
    ///   The button will reset the property to the configured reset value.
    ///   Example usage:
    ///   [Resettable]                  // Button label is "Reset"
    ///   [Resettable("Default")]       // Button label is "Default"
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class ResettableAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ResettableAttribute" /> class.
        /// </summary>
        public ResettableAttribute()
        {
            this.ButtonLabel = "Reset";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResettableAttribute"/> class.
        /// </summary>
        /// <param name="label">
        /// The label.
        /// </param>
        public ResettableAttribute(string label)
        {
            this.ButtonLabel = label;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the button label.
        /// </summary>
        /// <value>The button label.</value>
        public object ButtonLabel { get; set; }

        #endregion
    }
}