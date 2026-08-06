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
    /// The collapsed header display is driven by the <see cref="Value"/> property.
    /// <see cref="System.Windows.Controls.Primitives.Selector.SelectedValue"/> (inherited from
    /// <see cref="System.Windows.Controls.ComboBox"/>) is not used by this control's default template.
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
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(object),
            typeof(PopupBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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

        /// <summary>
        /// Gets or sets the current value displayed and edited by this control.
        /// Bind this property (two-way) to the data source; the <see cref="PopupTemplate"/> content
        /// should bind its own value property back to <see cref="Value"/> so that edits propagate
        /// through this property and trigger the <see cref="System.Windows.Data.Binding.SourceUpdatedEvent"/>.
        /// </summary>
        public object Value
        {
            get => this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }
    }
}