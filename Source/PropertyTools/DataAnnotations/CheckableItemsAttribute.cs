// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckableItemsAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the name of the properties that controls the IsChecked and Content of checkable items.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the name of the properties that controls the IsChecked and Content of checkable items.
    /// </summary>
    public class CheckableItemsAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableItemsAttribute" /> class.
        /// </summary>
        /// <param name="isCheckedPropertyName">Name of the IsChecked property.</param>
        /// <param name="contentPropertyName">Name of the Content property.</param>
        public CheckableItemsAttribute(string isCheckedPropertyName, string contentPropertyName = "")
        {
            this.IsCheckedPropertyName = isCheckedPropertyName;
            this.ContentPropertyName = contentPropertyName;
        }

        /// <summary>
        /// Gets or sets the name of the IsChecked property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string IsCheckedPropertyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the content property.
        /// </summary>
        /// <value>The name of the content property.</value>
        public string ContentPropertyName { get; set; }
    }
}