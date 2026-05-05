// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumFilterAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines the enum filter attribute
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class EnumFilterAttribute : System.Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumFilterAttribute"/> class.
        /// </summary>        
        public EnumFilterAttribute(FilteringMode mode)
        {
            this.Mode = mode;            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumFilterAttribute"/> class.
        /// </summary>        
        public EnumFilterAttribute(FilteringMode mode, Enum value1)
        {
            this.Mode = mode;
            this.Items = new[] { value1 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumFilterAttribute"/> class.
        /// </summary>        
        public EnumFilterAttribute(FilteringMode mode, Enum value1, Enum value2)
        {
            this.Mode = mode;
            this.Items = new[] { value1, value2 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumFilterAttribute"/> class.
        /// </summary>        
        public EnumFilterAttribute(FilteringMode mode, Enum value1, Enum value2, Enum value3)
        {
            this.Mode = mode;
            this.Items = new[] { value1, value2, value3 };
        }

        /// <summary>
        /// The filtering mode
        /// </summary>
        public FilteringMode Mode { get; set; }

        /// <summary>
        /// Array of items for enum filtering criteria
        /// </summary>
        /// <remarks>
        /// May be NULL
        /// </remarks>
        public Enum[] Items { get; set; }

        /// <summary>
        /// The enum filtering mode enumeration
        /// </summary>
        public enum FilteringMode
        {
            Include,
            Exclude,
        }
    }
}