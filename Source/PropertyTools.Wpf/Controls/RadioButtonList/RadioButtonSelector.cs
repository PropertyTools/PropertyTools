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
    using PropertyTools.DataAnnotations;
    using PropertyTools.Wpf.Common;
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    /// <summary>
    /// Represents a control that shows a list of radio buttons.
    /// </summary>
    [TemplatePart(Name = PartPanel, Type = typeof(StackPanel))]
    public class RadioButtonSelector : ItemsControl, ISelectorDefinition
    {
        /// <summary>
        /// Identifies the <see cref="ItemMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
            nameof(ItemMargin),
            typeof(Thickness),
            typeof(RadioButtonSelector),
            new UIPropertyMetadata(new Thickness(0, 4, 0, 4)));

        /// <summary>
        /// Identifies the <see cref="ItemPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(
            nameof(ItemPadding),
            typeof(Thickness),
            typeof(RadioButtonSelector),
            new UIPropertyMetadata(new Thickness(4, 0, 0, 0)));

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation),
            typeof(Orientation),
            typeof(RadioButtonSelector),
            new UIPropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(object),
            typeof(RadioButtonSelector),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChanged));

        /// <summary>
        /// The part panel.
        /// </summary>
        protected const string PartPanel = "PART_Panel";

        /// <summary>
        /// The panel.
        /// </summary>
        protected StackPanel panel;

        /// <summary>
        /// Initializes static members of the <see cref="RadioButtonSelector" /> class.
        /// </summary>
        static RadioButtonSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RadioButtonSelector), new FrameworkPropertyMetadata(typeof(RadioButtonSelector)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButtonSelector" /> class.
        /// </summary>
        public RadioButtonSelector()
        {
            this.DataContextChanged += this.HandleDataContextChanged;
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
        /// Prepopulated Enum property's metadata
        /// </summary>
        /// <remarks>
        /// Available only when <see cref="EnumType"/> is Enum or Nullable enum
        /// </remarks>
        public EnumPropertyMetadata EnumMetadata { get; set; }

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
        protected static void ValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((RadioButtonSelector)sender).UpdateContent();
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
        protected virtual void UpdateContent()
        {
            if (this.panel == null)
            {
                return;
            }

            this.panel.Children.Clear();

            IEnumerable items = PopulateItems();
            if (items == null)
            {
                return;
            }

            var converter = CreateConverter();

            foreach (var item in items)
            {
                object content;
                if (item == null)
                {
                    content = "-";
                }
                else if (string.IsNullOrWhiteSpace(this.DisplayMemberPath))
                {
                    content = item?.ToString();
                }
                else if (!ReflectionExtensions.TryGetFieldOrPropertyValue(item, this.DisplayMemberPath, out content))
                {
                    content = "-";
                }

                var ctrl = CreateControl();
                ctrl.Content = content;
                ctrl.Padding = this.ItemPadding;

                var isEnabledBinding = CreateBindingFromOptionEnableByAttribute(item, this.EnumMetadata?.EnumType);
                if (isEnabledBinding != null)
                {
                    ctrl.SetBinding(UIElement.IsEnabledProperty, isEnabledBinding);
                }

                var isCheckedBinding = new Binding(nameof(this.Value))
                {
                    Converter = converter,
                    ConverterParameter = item,
                    Source = this,
                    Mode = BindingMode.TwoWay
                };

                ctrl.SetBinding(ToggleButton.IsCheckedProperty, isCheckedBinding);

                ctrl.SetBinding(MarginProperty, new Binding(nameof(this.ItemMargin)) { Source = this });

                this.panel.Children.Add(ctrl);
            }
        }

        protected virtual ToggleButton CreateControl()
        {
            return new RadioButton();
        }

        protected virtual IValueConverter CreateConverter()
        {
            return new SingleStateSelectorItemToBooleanConverter() { SelectorDefinition = this };
        }

        protected virtual IEnumerable PopulateItems()
        {
            IEnumerable itemValues = null;

            if (this.ItemsSource != null)
            {
                itemValues = this.ItemsSource;
            }
            else if (this.ItemsSourcePropertyName != null)
            {
                var instance = this.DataContext;
                if (instance != null)
                {
                    // use instance.GetType to be able to fetch static properties also
                    var p = instance.GetType().GetProperties().FirstOrDefault(x => x.Name == this.ItemsSourcePropertyName);
                    itemValues = p?.GetValue(instance) as IEnumerable;
                }
            }

            return itemValues;
        }

        /// <summary>
        /// Creates a data binding for a WPF control based on the <see cref="OptionEnableByAttribute"/> applied to an enum value.
        /// </summary>
        /// <param name="itemValue">The value of the enum item to create the binding for.</param>
        /// <param name="enumType">The type of the enum containing the item.</param>
        /// <returns>
        /// A data binding instance if the enum item has an <see cref="OptionEnableByAttribute"/>; otherwise, null.
        /// </returns>
        protected Binding CreateBindingFromOptionEnableByAttribute(object itemValue, Type enumType)
        {
            if (enumType == null || !enumType.IsEnum || itemValue == null)
                return null;

            string enumMember = null;

            if (itemValue.GetType().IsEnum && enumType.IsEnumDefined(itemValue))
            {
                enumMember = itemValue.ToString();
            }
            else if (ReflectionExtensions.TryGetFieldOrPropertyValue(itemValue, this.SelectedValuePath, out object enumValue)
                && enumType.IsEnumDefined(enumValue))
            {
                enumMember = enumValue.ToString();
            }

            if (enumMember == null)
            {
                return null;
            }

            var fieldInfo = enumType.GetField(enumMember);
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

        /// <inheritdoc/>        
        public string ItemsSourcePropertyName { get; set; }

        /// <inheritdoc/>
        public string SelectedValuePath { get; set; }

        /// <inheritdoc/>
        public bool DisplayTextForNullItem { get; set; }
    }
}
