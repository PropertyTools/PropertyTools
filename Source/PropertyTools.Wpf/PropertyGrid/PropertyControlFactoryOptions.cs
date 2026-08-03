// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyControlFactoryOptions.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents options for the property control factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;

    /// <summary>
    /// Represents options for the property control factory.
    /// </summary>
    public class PropertyControlFactoryOptions
    {
        /// <summary>
        /// Gets or sets the limiting number of values for <c>enum</c> properties that can shown with radio buttons.
        /// </summary>
        /// <value>The limit. If the number of values exceeds the limit, a selector control will be used.</value>
        public int EnumAsRadioButtonsLimit { get; set; }

        /// <summary>
        /// Gets or sets the style applied to read-only property controls.
        /// </summary>
        /// <value>The style for read-only controls, or <c>null</c> to leave the control unstyled.</value>
        public Style ReadOnlyControlStyle { get; set; }

        /// <summary>
        /// Gets or sets the validation error template.
        /// </summary>
        /// <value>
        /// The validation error template.
        /// </value>
        public DataTemplate ValidationErrorTemplate { get; set; }

        /// <summary>
        /// Gets or sets the validation error style.
        /// </summary>
        /// <value>
        /// The validation error style.
        /// </value>
        public Style ValidationErrorStyle { get; set; }
    }
}