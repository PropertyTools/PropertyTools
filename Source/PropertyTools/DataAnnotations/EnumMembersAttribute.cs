// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumMembersAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Base class for all PropertyTools custom attributes.
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
        /// 
        /// </summary>
        public FilteringMode Mode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// May be NULL
        /// </remarks>
        public Enum[] Items { get; set; }

        public enum FilteringMode
        {
            Include,
            Exclude,
        }
    }
}