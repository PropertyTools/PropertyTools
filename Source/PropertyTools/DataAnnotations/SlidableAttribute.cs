// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SlidableAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifes that the property can be edited by a slider.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class SlidableAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SlidableAttribute" /> class.
        /// </summary>
        public SlidableAttribute()
        {
            this.Minimum = 0;
            this.Maximum = 100;
            this.SmallChange = 1;
            this.LargeChange = 10;
            this.SnapToTicks = false;
            this.TickFrequency = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidableAttribute"/> class.
        /// </summary>
        /// <param name="minimum">
        /// The minimum.
        /// </param>
        /// <param name="maximum">
        /// The maximum.
        /// </param>
        public SlidableAttribute(double minimum, double maximum)
            : this(minimum, maximum, 1, 10)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidableAttribute"/> class.
        /// </summary>
        /// <param name="minimum">
        /// The minimum.
        /// </param>
        /// <param name="maximum">
        /// The maximum.
        /// </param>
        /// <param name="smallChange">
        /// The small change.
        /// </param>
        /// <param name="largeChange">
        /// The large change.
        /// </param>
        public SlidableAttribute(double minimum, double maximum, double smallChange, double largeChange)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.SmallChange = smallChange;
            this.LargeChange = largeChange;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidableAttribute"/> class.
        /// </summary>
        /// <param name="minimum">
        /// The minimum.
        /// </param>
        /// <param name="maximum">
        /// The maximum.
        /// </param>
        /// <param name="smallChange">
        /// The small change.
        /// </param>
        /// <param name="largeChange">
        /// The large change.
        /// </param>
        /// <param name="snapToTicks">
        /// if set to <c>true</c> [snap to ticks].
        /// </param>
        /// <param name="tickFrequency">
        /// The tick frequency.
        /// </param>
        public SlidableAttribute(
            double minimum,
            double maximum,
            double smallChange,
            double largeChange,
            bool snapToTicks,
            double tickFrequency)
            : this(minimum, maximum, smallChange, largeChange)
        {
            this.SnapToTicks = snapToTicks;
            this.TickFrequency = tickFrequency;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the large change.
        /// </summary>
        /// <value>The large change.</value>
        public double LargeChange { get; set; }

        /// <summary>
        ///   Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public double Maximum { get; set; }

        /// <summary>
        ///   Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public double Minimum { get; set; }

        /// <summary>
        ///   Gets or sets the small change.
        /// </summary>
        /// <value>The small change.</value>
        public double SmallChange { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [snap to ticks].
        /// </summary>
        /// <value><c>true</c> if [snap to ticks]; otherwise, <c>false</c>.</value>
        public bool SnapToTicks { get; set; }

        /// <summary>
        ///   Gets or sets the tick frequency.
        /// </summary>
        /// <value>The tick frequency.</value>
        public double TickFrequency { get; set; }

        #endregion

        // public TickPlacement TickPlacement { get; set; }
    }
}