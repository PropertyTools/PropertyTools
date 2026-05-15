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
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    using PropertyTools.DataAnnotations;
    using PropertyTools.Wpf.Common;

    /// <summary>
    /// Represents a control that shows a list of radio buttons.
    /// </summary>
    [TemplatePart(Name = PartPanel, Type = typeof(StackPanel))]
    public class RadioButtonList : RadioButtonSelector
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
        /// 
        /// </summary>
        /// <remarks>May contain NULL when <see cref="EnumType"/> is Nullable enum </remarks>
        public object[] EnumValues { get; set; }

        /// <summary>
        /// Updates the content.
        /// </summary>
        protected override void UpdateContent()
        {
            if (this.panel == null)
            {
                return;
            }

            this.panel.Children.Clear();

            Type enumType = null;
            if (this.EnumMetadata != null)
            {
                enumType = this.EnumMetadata.EnumType;
            }
            else if (this.EnumType != null)
            {
                enumType = Nullable.GetUnderlyingType(this.EnumType) ?? this.EnumType;
            }
            else if (this.Value != null)
            {
                enumType = this.Value.GetType();
            }

            if (enumType == null || !typeof(Enum).IsAssignableFrom(enumType))
            {
                return;
            }

            if (this.Value != null && this.Value.GetType() != enumType)
            {
                throw new ArgumentOutOfRangeException($"Value type '{Value.GetType().FullName}' is different than enum type not '{enumType.FullName}'.");
            }

            List<object> enumValues;
            if (this.EnumValues != null)
            {
                enumValues = this.EnumValues.ToList();
            }
            else
            {
                enumValues = Enum.GetValues(enumType).Cast<Enum>().FilterOnBrowsableAttribute()
                    .Cast<object>().ToList();

                // if the type is nullable, add the null value
                if (Nullable.GetUnderlyingType(enumType) != null)
                {
                    enumValues.Add(null);
                }
            }

            var converter = new EnumToBooleanConverter { EnumType = enumType };

            foreach (var itemValue in enumValues)
            {
                object content;
                if (itemValue != null)
                {
                    content = this.EnumMetadata?.EnumDisplayNames?.TryGetValue((Enum)itemValue, out string enumMemberDisplayText) == true
                            ? enumMemberDisplayText
                            : this.DescriptionConverter.Convert(itemValue, typeof(string), null, CultureInfo.CurrentUICulture);
                }
                else
                {
                    content = this.EnumMetadata?.EnumDisplayNull ?? "-";
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
    }
}