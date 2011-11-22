// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpinnableAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifes that the property can be edited by a spin control.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-us/library/windows/desktop/aa511491.aspx
    /// </remarks>
    [AttributeUsage(AttributeTargets.All)]
    public class SpinnableAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinnableAttribute"/> class. 
        ///   Initializes a new instance of the <see cref="SlidableAttribute"/> class.
        /// </summary>
        public SpinnableAttribute()
        {
            this.Minimum = 0;
            this.Maximum = 100;
            this.SmallChange = 1;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SpinnableAttribute"/> class.
        /// Initializes a new instance of the <see cref="SlidableAttribute"/> class.
        /// </summary>
        /// <param name="smallChange">The small change.</param>
        public SpinnableAttribute(object smallChange)
        {
            this.SmallChange = smallChange;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinnableAttribute"/> class.
        /// Initializes a new instance of the <see cref="SlidableAttribute"/> class.
        /// </summary>
        /// <param name="smallChange">The small change.</param>
        /// <param name="largeChange">The large change.</param>
        public SpinnableAttribute(object smallChange, object largeChange)
        {
            this.SmallChange = smallChange;
            this.LargeChange = largeChange;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinnableAttribute"/> class.
        /// Initializes a new instance of the <see cref="SlidableAttribute"/> class.
        /// </summary>
        /// <param name="smallChange">The small change.</param>
        /// <param name="largeChange">The large change.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        public SpinnableAttribute(object smallChange, object largeChange, object minimum, object maximum)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.SmallChange = smallChange;
            this.LargeChange = largeChange;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the small change.
        /// </summary>
        /// <value>The small change.</value>
        public object SmallChange { get; set; }

        /// <summary>
        /// Gets or sets the large change.
        /// </summary>
        /// <value>The large change.</value>
        public object LargeChange { get; set; }

        /// <summary>
        ///   Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public object Maximum { get; set; }

        /// <summary>
        ///   Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public object Minimum { get; set; }

        #endregion
    }
}