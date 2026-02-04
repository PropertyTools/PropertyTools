// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyTabAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the name of the tab in which to display the property in a PropertyGrid control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the name of the tab in which to display the property in a PropertyGrid control.
    /// </summary>
    /// <remarks>
    /// This attribute is similar to System.ComponentModel.PropertyTabAttribute but simplified for PropertyTools.
    /// It allows you to organize properties into different tabs in the PropertyGrid.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyTabAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTabAttribute"/> class.
        /// </summary>
        /// <param name="tabName">The name of the tab.</param>
        public PropertyTabAttribute(string tabName)
        {
            this.TabName = tabName;
            this.Scope = PropertyTabScope.Component;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTabAttribute"/> class.
        /// </summary>
        /// <param name="tabName">The name of the tab.</param>
        /// <param name="scope">The scope of the tab.</param>
        public PropertyTabAttribute(string tabName, PropertyTabScope scope)
        {
            this.TabName = tabName;
            this.Scope = scope;
        }

        /// <summary>
        /// Gets the name of the tab.
        /// </summary>
        /// <value>The name of the tab.</value>
        public string TabName { get; private set; }

        /// <summary>
        /// Gets the scope of the tab.
        /// </summary>
        /// <value>The scope of the tab.</value>
        public PropertyTabScope Scope { get; private set; }
    }
}
