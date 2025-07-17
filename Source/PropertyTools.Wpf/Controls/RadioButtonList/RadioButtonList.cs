// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadioButtonList.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a control that shows a list of radio buttons.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Represents a control that shows a list of radio buttons.
    /// </summary>
    [TemplatePart(Name = PartPanel, Type = typeof(StackPanel))]
    public class RadioButtonList : Control
    {
        /// <summary>
        /// Identifies the <see cref="DescriptionConverter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionConverterProperty = DependencyProperty.Register(
            nameof(DescriptionConverter),
            typeof(IValueConverter),
            typeof(RadioButtonList),
            new UIPropertyMetadata(new EnumDescriptionConverter()));

        /// <summary>
        /// Identifies the <see cref="EnumType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EnumTypeProperty = DependencyProperty.Register(
            nameof(EnumType),
            typeof(Type),
            typeof(RadioButtonList),
            new UIPropertyMetadata(null, ValueChanged));

        /// <summary>
        /// Identifies the <see cref="ItemMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
            nameof(ItemMargin),
            typeof(Thickness),
            typeof(RadioButtonList),
            new UIPropertyMetadata(new Thickness(0, 4, 0, 4)));

        /// <summary>
        /// Identifies the <see cref="ItemPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(
            nameof(ItemPadding),
            typeof(Thickness),
            typeof(RadioButtonList),
            new UIPropertyMetadata(new Thickness(4, 0, 0, 0)));

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation),
            typeof(Orientation),
            typeof(RadioButtonList),
            new UIPropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(object),
            typeof(RadioButtonList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChanged));

        /// <summary>
        /// The part panel.
        /// </summary>
        private const string PartPanel = "PART_Panel";

        /// <summary>
        /// The panel.
        /// </summary>
        private StackPanel panel;

        /// <summary>
        /// Initializes static members of the <see cref="RadioButtonList" /> class.
        /// </summary>
        static RadioButtonList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RadioButtonList), new FrameworkPropertyMetadata(typeof(RadioButtonList)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButtonList" /> class.
        /// </summary>
        public RadioButtonList()
        {
            this.DataContextChanged += this.HandleDataContextChanged;
        }

        /// <summary>
        /// Gets or sets the description converter.
        /// </summary>
        /// <value>The description converter.</value>
        public IValueConverter DescriptionConverter
        {
            get
            {
                return (IValueConverter)this.GetValue(DescriptionConverterProperty);
            }

            set
            {
                this.SetValue(DescriptionConverterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the type of the enumeration.
        /// </summary>
        /// <value>The type of the enumeration.</value>
        public Type EnumType
        {
            get
            {
                return (Type)this.GetValue(EnumTypeProperty);
            }

            set
            {
                this.SetValue(EnumTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the item margin.
        /// </summary>
        /// <value>The item margin.</value>
        public Thickness ItemMargin
        {
            get
            {
                return (Thickness)this.GetValue(ItemMarginProperty);
            }

            set
            {
                this.SetValue(ItemMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the item padding.
        /// </summary>
        /// <value>The item padding.</value>
        public Thickness ItemPadding
        {
            get
            {
                return (Thickness)this.GetValue(ItemPaddingProperty);
            }

            set
            {
                this.SetValue(ItemPaddingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get
            {
                return (Orientation)this.GetValue(OrientationProperty);
            }

            set
            {
                this.SetValue(OrientationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get
            {
                return this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see
        /// cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> .
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (this.panel == null)
            {
                this.panel = this.Template.FindName(PartPanel, this) as StackPanel;
            }

            this.UpdateContent();
        }

        /// <summary>
        /// Called when the <see cref="Value" /> has changed or the <see cref="EnumType" /> has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private static void ValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((RadioButtonList)sender).UpdateContent();
        }

        /// <summary>
        /// Handles data context changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.UpdateContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        private void UpdateContent()
        {
            if (this.panel == null)
            {
                return;
            }

            this.panel.Children.Clear();

            var enumType = this.EnumType;
            if (enumType != null)
            {
                var ult = Nullable.GetUnderlyingType(enumType);
                if (ult != null)
                {
                    enumType = ult;
                }
            }

            if (this.Value != null)
            {
                enumType = this.Value.GetType();
            }

            if (enumType == null || !typeof(Enum).IsAssignableFrom(enumType))
            {
                return;
            }

            var enumValues = Enum.GetValues(enumType).FilterOnBrowsableAttribute().ToList();

            // if the type is nullable, add the null value
            if (Nullable.GetUnderlyingType(enumType) != null)
            {
                enumValues.Add(null);
            }

            var converter = new EnumToBooleanConverter { EnumType = enumType };

            foreach (var itemValue in enumValues)
            {
                object content;
                if (itemValue != null)
                {
                    content = this.DescriptionConverter.Convert(
                        itemValue,
                        typeof(string),
                        null,
                        CultureInfo.CurrentUICulture);
                }
                else
                {
                    content = "-";
                }

                var rb = new RadioButton
                {
                    Content = content,
                    Padding = this.ItemPadding,
                };

                var binding = CreateBindingFromOptionEnableByAttribute(itemValue, enumType);
                if (binding != null)
                {
                    rb.SetBinding(UIElement.IsEnabledProperty, binding);
                }

                var isCheckedBinding = new Binding(nameof(this.Value))
                {
                    Converter = converter,
                    ConverterParameter = itemValue,
                    Source = this,
                    Mode = BindingMode.TwoWay
                };

                rb.SetBinding(ToggleButton.IsCheckedProperty, isCheckedBinding);

                rb.SetBinding(MarginProperty, new Binding(nameof(this.ItemMargin)) { Source = this });

                this.panel.Children.Add(rb);
            }
        }

        /// <summary>
        /// Creates a data binding for a WPF control based on the <see cref="OptionEnableByAttribute"/> applied to an enum value.
        /// </summary>
        /// <param name="itemValue">The value of the enum item to create the binding for.</param>
        /// <param name="enumType">The type of the enum containing the item.</param>
        /// <returns>
        /// A data binding instance if the enum item has an <see cref="OptionEnableByAttribute"/>; otherwise, null.
        /// </returns>
        private Binding CreateBindingFromOptionEnableByAttribute(object itemValue, Type enumType)
        {
            var itemName = itemValue?.ToString();
            if (itemName == null)
            {
                return null;
            }

            var fieldInfo = enumType.GetField(itemName);
            if (fieldInfo == null)
            {
                return null;
            }

            var attribute = fieldInfo.GetCustomAttribute<OptionEnableByAttribute>();
            if (attribute != null)
            {
                // Create and return the binding using the property name from the attribute
                return new Binding(attribute.PropertyName)
                {
                    Source = this.DataContext
                };
            }

            return null;
        }
    }
}
