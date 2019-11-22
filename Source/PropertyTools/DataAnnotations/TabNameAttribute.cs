﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the name of the category in which to group the property or event when displayed in a PropertyGrid control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the name of the category in which to group the property or event when displayed in a PropertyGrid control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class TabNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAttribute"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        public TabNameAttribute(string tabname)
        {
            this.TabName = tabname;
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>The category.</value>
        public virtual string TabName { get; private set; }
    }
}