﻿namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the input direction for the decorated property.
    /// </summary>
    public class InputDirectionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the input direction.
        /// </summary>
        /// <value>The input direction.</value>
        public InputDirection InputDirection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDirectionAttribute" /> class.
        /// </summary>
        /// <param name="inputDirection">The input direction.</param>
        public InputDirectionAttribute(InputDirection inputDirection)
        {
            this.InputDirection = inputDirection;
        }
    }
}