// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckBoxList.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a control that shows a list of check boxes for a [Flags] enum.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Represents a control that shows a list of check boxes for a <see cref="FlagsAttribute"/> enum.
    /// </summary>
    [TemplatePart(Name = PartPanel, Type = typeof(StackPanel))]
    public class CheckBoxList : Control
    {
        /// <summary>
        /// Identifies the <see cref="DescriptionConverter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionConverterProperty = DependencyProperty.Register(
            nameof(DescriptionConverter),
            typeof(IValueConverter),
            typeof(CheckBoxList),
            new UIPropertyMetadata(new EnumDescriptionConverter()));

        /// <summary>
        /// Identifies the <see cref="EnumType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EnumTypeProperty = DependencyProperty.Register(
            nameof(EnumType),
            typeof(Type),
            typeof(CheckBoxList),
            new UIPropertyMetadata(null, EnumTypeChanged));

        /// <summary>
        /// Identifies the <see cref="EnumFilter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EnumFilterProperty = DependencyProperty.Register(
            nameof(EnumFilter),
            typeof(EnumFilterAttribute),
            typeof(CheckBoxList),
            new UIPropertyMetadata(null, EnumTypeChanged));

        /// <summary>
        /// Identifies the <see cref="ItemMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
            nameof(ItemMargin),
            typeof(Thickness),
            typeof(CheckBoxList),
            new UIPropertyMetadata(new Thickness(0, 0, 10, 0)));

        /// <summary>
        /// Identifies the <see cref="ItemPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(
            nameof(ItemPadding),
            typeof(Thickness),
            typeof(CheckBoxList),
            new UIPropertyMetadata(new Thickness(4, 0, 0, 0)));

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation),
            typeof(Orientation),
            typeof(CheckBoxList),
            new UIPropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(object),
            typeof(CheckBoxList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChanged));

        /// <summary>
        /// The name of the panel template part.
        /// </summary>
        private const string PartPanel = "PART_Panel";

        /// <summary>
        /// The stack panel that contains the check boxes.
        /// </summary>
        private StackPanel panel;

        /// <summary>
        /// Indicates whether the control is currently updating, to prevent recursive callbacks.
        /// </summary>
        private bool isUpdating;

        /// <summary>
        /// Initializes static members of the <see cref="CheckBoxList"/> class.
        /// </summary>
        static CheckBoxList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CheckBoxList), new FrameworkPropertyMetadata(typeof(CheckBoxList)));
        }

        /// <summary>
        /// Gets or sets the description converter used to display enum value labels.
        /// </summary>
        public IValueConverter DescriptionConverter
        {
            get => (IValueConverter)this.GetValue(DescriptionConverterProperty);
            set => this.SetValue(DescriptionConverterProperty, value);
        }

        /// <summary>
        /// Gets or sets the type of the flags enumeration.
        /// </summary>
        public Type EnumType
        {
            get => (Type)this.GetValue(EnumTypeProperty);
            set => this.SetValue(EnumTypeProperty, value);
        }

        /// <summary>
        /// Gets or sets the optional filter that restricts which enum values are shown.
        /// </summary>
        public EnumFilterAttribute EnumFilter
        {
            get => (EnumFilterAttribute)this.GetValue(EnumFilterProperty);
            set => this.SetValue(EnumFilterProperty, value);
        }

        /// <summary>
        /// Gets or sets the margin applied to each check box item.
        /// </summary>
        public Thickness ItemMargin
        {
            get => (Thickness)this.GetValue(ItemMarginProperty);
            set => this.SetValue(ItemMarginProperty, value);
        }

        /// <summary>
        /// Gets or sets the padding applied to each check box item.
        /// </summary>
        public Thickness ItemPadding
        {
            get => (Thickness)this.GetValue(ItemPaddingProperty);
            set => this.SetValue(ItemPaddingProperty, value);
        }

        /// <summary>
        /// Gets or sets the orientation of the check box list.
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)this.GetValue(OrientationProperty);
            set => this.SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Gets or sets the current flags enum value (as a boxed enum or integer).
        /// </summary>
        public object Value
        {
            get => this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.panel = this.Template.FindName(PartPanel, this) as StackPanel;
            this.RebuildItems();
        }

        /// <summary>
        /// Called when <see cref="Value"/> changes. Refreshes the checked state of all check boxes.
        /// </summary>
        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CheckBoxList)d).UpdateCheckedStates();
        }

        /// <summary>
        /// Called when <see cref="EnumType"/> or <see cref="EnumFilter"/> changes. Rebuilds the item list.
        /// </summary>
        private static void EnumTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CheckBoxList)d).RebuildItems();
        }

        /// <summary>
        /// Resolves the actual enum type from <see cref="EnumType"/> or the current <see cref="Value"/>.
        /// </summary>
        private Type ResolveEnumType()
        {
            var enumType = this.EnumType;
            if (enumType != null)
            {
                var ult = Nullable.GetUnderlyingType(enumType);
                if (ult != null)
                {
                    enumType = ult;
                }
            }

            if (enumType == null && this.Value != null)
            {
                enumType = this.Value.GetType();
            }

            return enumType != null && typeof(Enum).IsAssignableFrom(enumType) ? enumType : null;
        }

        /// <summary>
        /// Rebuilds the check box items from the current enum type.
        /// </summary>
        private void RebuildItems()
        {
            if (this.panel == null)
            {
                return;
            }

            this.panel.Children.Clear();

            var enumType = this.ResolveEnumType();
            if (enumType == null)
            {
                return;
            }

            var enumValues = Enum.GetValues(enumType)
                .FilterOnBrowsableAttribute()
                .FilterOnEnumFilterAttribute(this.EnumFilter)
                .Cast<object>()
                .ToList();

            foreach (var itemValue in enumValues)
            {
                var flagValue = Convert.ToInt64(itemValue);

                // Skip composite values (e.g. "All = A | B | C") and zero values
                if (flagValue == 0 || (flagValue & (flagValue - 1)) != 0)
                {
                    continue;
                }

                var label = this.DescriptionConverter.Convert(
                    itemValue,
                    typeof(string),
                    null,
                    CultureInfo.CurrentUICulture) as string ?? itemValue.ToString();

                var cb = new CheckBox
                {
                    Content = label,
                    Tag = itemValue,
                    Padding = this.ItemPadding,
                    Margin = this.ItemMargin,
                };

                cb.Checked += this.OnCheckBoxChanged;
                cb.Unchecked += this.OnCheckBoxChanged;

                this.panel.Children.Add(cb);
            }

            this.UpdateCheckedStates();
        }

        /// <summary>
        /// Refreshes the <see cref="CheckBox.IsChecked"/> state for each item based on the current <see cref="Value"/>.
        /// </summary>
        private void UpdateCheckedStates()
        {
            if (this.panel == null || this.isUpdating)
            {
                return;
            }

            var currentLong = this.Value != null ? Convert.ToInt64(this.Value) : 0L;

            foreach (CheckBox cb in this.panel.Children.OfType<CheckBox>())
            {
                var flag = Convert.ToInt64(cb.Tag);
                cb.IsChecked = (currentLong & flag) == flag;
            }
        }

        /// <summary>
        /// Called when a check box is checked or unchecked. Composes the new flags value and sets <see cref="Value"/>.
        /// </summary>
        private void OnCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if (this.isUpdating)
            {
                return;
            }

            this.isUpdating = true;
            try
            {
                var enumType = this.ResolveEnumType();
                if (enumType == null)
                {
                    return;
                }

                long result = 0;
                foreach (CheckBox cb in this.panel.Children.OfType<CheckBox>())
                {
                    if (cb.IsChecked == true)
                    {
                        result |= Convert.ToInt64(cb.Tag);
                    }
                }

                this.Value = Enum.ToObject(enumType, result);
            }
            finally
            {
                this.isUpdating = false;
            }
        }
    }
}
