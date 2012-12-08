// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyItem.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents a property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    using PropertyTools.DataAnnotations;

    using HorizontalAlignment = PropertyTools.DataAnnotations.HorizontalAlignment;

    /// <summary>
    /// Represents a property.
    /// </summary>
    public class PropertyItem : Observable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyItem"/> class.
        /// </summary>
        /// <param name="propertyDescriptor">
        /// The property descriptor.
        /// </param>
        public PropertyItem(PropertyDescriptor propertyDescriptor, PropertyDescriptorCollection propertyDescriptors)
        {
            this.Descriptor = propertyDescriptor;
            this.Properties = propertyDescriptors;

            this.Width = double.NaN;
            this.Height = double.NaN;
            this.MaximumHeight = double.PositiveInfinity;
            this.MaxLength = int.MaxValue;
            this.MoveFocusOnEnter = true;
            this.HeaderPlacement = HeaderPlacement.Left;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.DataTypes = new List<DataType>();
            this.Columns = new List<ColumnAttribute>();

            this.ListCanAdd = true;
            this.ListCanRemove = true;
            this.ListMaximumNumberOfItems = int.MaxValue;
            this.InputDirection = InputDirection.Vertical;            
        }

        /// <summary>
        /// Gets or sets the input direction.
        /// </summary>
        /// <value>The input direction.</value>
        public InputDirection InputDirection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to accept return.
        /// </summary>
        public bool AcceptsReturn { get; set; }

        /// <summary>
        /// Gets the actual type of the property.
        /// </summary>
        /// <remarks>
        /// If a converter is defined, the target type will be set.
        /// </remarks>
        public Type ActualPropertyType
        {
            get
            {
                return this.GetConverterTargetType() ?? this.Descriptor.PropertyType;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to auto update text.
        /// </summary>
        public bool AutoUpdateText { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value> The category. </value>
        public string Category { get; set; }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value> The columns. </value>
        public List<ColumnAttribute> Columns { get; private set; }

        /// <summary>
        /// Gets or sets the converter.
        /// </summary>
        /// <value> The converter. </value>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// Gets or sets the converter culture.
        /// </summary>
        /// <value> The converter culture. </value>
        public CultureInfo ConverterCulture { get; set; }

        /// <summary>
        /// Gets or sets the converter parameter.
        /// </summary>
        /// <value> The converter parameter. </value>
        public object ConverterParameter { get; set; }

        /// <summary>
        /// Gets the data types.
        /// </summary>
        /// <value> The data types. </value>
        public List<DataType> DataTypes { get; private set; }

        /// <summary>
        /// Gets or sets the tool tip.
        /// </summary>
        /// <value> The tool tip. </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the property descriptor.
        /// </summary>
        /// <value> The descriptor. </value>
        public PropertyDescriptor Descriptor { get; set; }

        /// <summary>
        /// Gets a value indicating whether this property is read only.
        /// </summary>
        /// <value><c>true</c> if this property is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly
        {
            get
            {
                return this.Descriptor.IsReadOnly;
            }
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value> The display name. </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the file path default extension.
        /// </summary>
        /// <value> The file path default extension. </value>
        public string FilePathDefaultExtension { get; set; }

        /// <summary>
        /// Gets or sets the file path filter.
        /// </summary>
        /// <value> The file path filter. </value>
        public string FilePathFilter { get; set; }

        /// <summary>
        /// Gets or sets the filter descriptor.
        /// </summary>
        /// <value> The filter descriptor. </value>
        public PropertyDescriptor FilterDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the default extension descriptor.
        /// </summary>
        /// <value>The default extension descriptor.</value>
        public PropertyDescriptor DefaultExtensionDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the font family property descriptor.
        /// </summary>
        /// <value> The font family property descriptor. </value>
        public PropertyDescriptor FontFamilyPropertyDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value> The size of the font. </value>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the open type weight.
        /// </summary>
        /// <value> The font weight. </value>
        public int FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        /// <value> The format string. </value>
        public string FormatString { get; set; }

        /// <summary>
        /// Gets or sets the header placement.
        /// </summary>
        /// <value> The header placement. </value>
        public HeaderPlacement HeaderPlacement { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment.
        /// </summary>
        public PropertyTools.DataAnnotations.HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to stretch to the available tab size.
        /// </summary>
        /// <value><c>true</c> if fill is enabled; otherwise, <c>false</c>.</value>
        public bool FillTab { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value> The height. </value>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property is a comment.
        /// </summary>
        public bool IsComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property is a directory path.
        /// </summary>
        public bool IsDirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property is editable.
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// Gets or sets the is enabled descriptor.
        /// </summary>
        /// <value> The is enabled descriptor. </value>
        public PropertyDescriptor IsEnabledDescriptor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property is a file open dialog.
        /// </summary>
        public bool IsFileOpenDialog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property is a file path.
        /// </summary>
        public bool IsFilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property is a font family selector.
        /// </summary>
        public bool IsFontFamilySelector { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property is optional.
        /// </summary>
        public bool IsOptional { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property is a password.
        /// </summary>
        public bool IsPassword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property should show a slider.
        /// </summary>
        public bool IsSlidable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property should show spin buttons.
        /// </summary>
        public bool IsSpinnable { get; set; }

        /// <summary>
        /// Gets or sets the is visible descriptor.
        /// </summary>
        /// <value> The is visible descriptor. </value>
        public PropertyDescriptor IsVisibleDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value> The items source. </value>
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        /// Gets or sets the values descriptor.
        /// </summary>
        /// <value> The values descriptor. </value>
        public PropertyDescriptor ItemsSourceDescriptor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether items can be added to the list.
        /// </summary>
        public bool ListCanAdd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether items can be removed from the list.
        /// </summary>
        public bool ListCanRemove { get; set; }

        /// <summary>
        /// Gets or sets the list maximum number of items.
        /// </summary>
        /// <value> The list maximum number of items. </value>
        public int ListMaximumNumberOfItems { get; set; }

        /// <summary>
        /// Gets or sets the max length.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum height.
        /// </summary>
        /// <value> The maximum height. </value>
        public double MaximumHeight { get; set; }

        /// <summary>
        /// Gets or sets the minimum height.
        /// </summary>
        /// <value> The minimum height. </value>
        public double MinimumHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to move focus on enter.
        /// </summary>
        public bool MoveFocusOnEnter { get; set; }

        /// <summary>
        /// Gets or sets the optional descriptor.
        /// </summary>
        /// <value> The optional descriptor. </value>
        public PropertyDescriptor OptionalDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the optional value.
        /// </summary>
        /// <value> The optional value. </value>
        public object OptionalValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to preview fonts.
        /// </summary>
        /// <value> <c>true</c> if fonts should be previewed; otherwise, <c>false</c> . </value>
        public bool PreviewFonts { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value> The properties. </value>
        public PropertyDescriptorCollection Properties { get; set; }

        /// <summary>
        /// Gets or sets the property icon.
        /// </summary>
        /// <value> The property icon. </value>
        public ImageSource PropertyIcon { get; set; }

        /// <summary>
        /// Gets or sets the relative path descriptor.
        /// </summary>
        /// <value> The relative path descriptor. </value>
        public PropertyDescriptor RelativePathDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the reset command.
        /// </summary>
        /// <value> The reset command. </value>
        public ICommand ResetCommand { get; set; }

        /// <summary>
        /// Gets or sets the reset header.
        /// </summary>
        /// <value> The reset header. </value>
        public string ResetHeader { get; set; }

        /// <summary>
        /// Gets or sets the slider large change.
        /// </summary>
        public double SliderLargeChange { get; set; }

        /// <summary>
        /// Gets or sets the slider maximum.
        /// </summary>
        public double SliderMaximum { get; set; }

        /// <summary>
        /// Gets or sets the slider minimum.
        /// </summary>
        public double SliderMinimum { get; set; }

        /// <summary>
        /// Gets or sets the slider small change.
        /// </summary>
        public double SliderSmallChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the slider snaps to the ticks.
        /// </summary>
        public bool SliderSnapToTicks { get; set; }

        /// <summary>
        /// Gets or sets the slider tick frequency.
        /// </summary>
        public double SliderTickFrequency { get; set; }

        /// <summary>
        /// Gets or sets the index of the sort.
        /// </summary>
        /// <value> The index of the sort. </value>
        public int SortIndex { get; set; }

        /// <summary>
        /// Gets or sets the spin control large change.
        /// </summary>
        public object SpinLargeChange { get; set; }

        /// <summary>
        /// Gets or sets the spin control maximum.
        /// </summary>
        public object SpinMaximum { get; set; }

        /// <summary>
        /// Gets or sets the spin control minimum.
        /// </summary>
        public object SpinMinimum { get; set; }

        /// <summary>
        /// Gets or sets the spin control small change.
        /// </summary>
        public object SpinSmallChange { get; set; }

        /// <summary>
        /// Gets or sets the tab.
        /// </summary>
        /// <value> The tab. </value>
        public string Tab { get; set; }

        /// <summary>
        /// Gets or sets the text wrapping.
        /// </summary>
        /// <value> The text wrapping. </value>
        public TextWrapping TextWrapping { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property should use radio buttons.
        /// </summary>
        public bool UseRadioButtons { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value> The width. </value>
        public double Width { get; set; }

        /// <summary>
        /// Creates a binding.
        /// </summary>
        /// <param name="trigger">
        /// The trigger.
        /// </param>
        /// <returns>
        /// The binding.
        /// </returns>
        public virtual Binding CreateBinding(UpdateSourceTrigger trigger = UpdateSourceTrigger.Default)
        {
            var bindingMode = this.Descriptor.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
            var formatString = this.FormatString;
            if (formatString != null && !formatString.StartsWith("{"))
            {
                formatString = "{0:" + formatString + "}";
            }

            var binding = new Binding(this.Descriptor.Name)
                {
                    Mode = bindingMode,
                    Converter = this.Converter,
                    ConverterParameter = this.ConverterParameter,
                    StringFormat = formatString,
                    UpdateSourceTrigger = trigger,
                    ValidatesOnDataErrors = true,
                    ValidatesOnExceptions = true
                };
            if (this.ConverterCulture != null)
            {
                binding.ConverterCulture = this.ConverterCulture;
            }

            return binding;
        }

        /// <summary>
        /// Creates a one way binding.
        /// </summary>
        /// <returns> The binding. </returns>
        public Binding CreateOneWayBinding()
        {
            var b = this.CreateBinding();
            b.Mode = BindingMode.OneWay;
            return b;
        }

        /// <summary>
        /// Gets the first attribute of the specified type.
        /// </summary>
        /// <typeparam name="T"> Type of attribute. </typeparam>
        /// <returns> The attribute, or null. </returns>
        public T GetAttribute<T>() where T : Attribute
        {
            var type = typeof(T);
            foreach (var a in this.Descriptor.Attributes)
            {
                if (type.IsInstanceOfType(a))
                {
                    return a as T;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the attributes of the specified type.
        /// </summary>
        /// <typeparam name="T"> Type of attribute. </typeparam>
        /// <returns> Enumerable of T. </returns>
        public IEnumerable<T> GetAttributes<T>() where T : Attribute
        {
            var type = typeof(T);
            foreach (var a in this.Descriptor.Attributes)
            {
                if (type.IsAssignableFrom(a.GetType()))
                {
                    yield return a as T;
                }
            }
        }

        /// <summary>
        /// Gets the property descriptor for the specified property.
        /// </summary>
        /// <param name="name">
        /// The property name.
        /// </param>
        /// <returns>
        /// A property descriptor.
        /// </returns>
        public PropertyDescriptor GetDescriptor(string name)
        {
            if (name == null)
            {
                return null;
            }

            return this.Properties.Find(name, false);
        }

        /// <summary>
        /// Determines whether the specified type is assignable from this property type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// <c>true</c> if ok; otherwise, <c>false</c> .
        /// </returns>
        public bool Is(Type type)
        {
            var propertyType = this.ActualPropertyType;
            return propertyType.Is(type);
        }

        /// <summary>
        /// Gets the type of the converter target.
        /// </summary>
        /// <returns> The target type. </returns>
        private Type GetConverterTargetType()
        {
            if (this.Converter == null)
            {
                return null;
            }

            foreach (var a in TypeDescriptor.GetAttributes(this.Converter.GetType()))
            {
                var vca = a as ValueConversionAttribute;
                if (vca != null)
                {
                    return vca.TargetType;
                }
            }

            return null;
        }
    }
}