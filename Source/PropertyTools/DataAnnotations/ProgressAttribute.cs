// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies that the property is a progress value.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property is a progress value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ProgressAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressAttribute" /> class.
        /// </summary>
        public ProgressAttribute() : this(0,1)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressAttribute" /> class.
        /// </summary>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        public ProgressAttribute(double minimum, double maxium)
        {
            this.Minimum = minimum;
            this.Maximum = maxium;
        }

        /// <summary>
        /// Gets or sets the minimum value for the progress bar control.
        /// </summary>
        /// <value>The minimum value, default is 0.</value>
        public double Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum value for the progress bar control.
        /// </summary>
        /// <value>The maximum value, default is 1.</value>
        public double Maximum { get; set; }
    }
}