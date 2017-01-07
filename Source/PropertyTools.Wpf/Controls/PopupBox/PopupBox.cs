// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PopupBox.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a popup control that provides a data template for the popup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a popup control that provides a data template for the popup.
    /// </summary>
    public class PopupBox : ComboBox
    {
        /// <summary>
        /// Identifies the <see cref="PopupTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PopupTemplateProperty = DependencyProperty.Register(
            nameof(PopupTemplate),
            typeof(DataTemplate),
            typeof(PopupBox),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Initializes static members of the <see cref="PopupBox" /> class.
        /// </summary>
        static PopupBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupBox), new FrameworkPropertyMetadata(typeof(PopupBox)));
        }

        /// <summary>
        /// Gets or sets the popup template.
        /// </summary>
        /// <value>The popup template.</value>
        public DataTemplate PopupTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(PopupTemplateProperty);
            }

            set
            {
                this.SetValue(PopupTemplateProperty, value);
            }
        }
    }
}