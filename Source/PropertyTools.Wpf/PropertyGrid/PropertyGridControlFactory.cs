// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGridControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides a control factory for the PropertyGrid control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Provides a control factory for the <see cref="PropertyGrid" /> control.
    /// </summary>
    public class PropertyGridControlFactory : IPropertyGridControlFactory
    {
        /// <summary>
        /// The font family converter
        /// </summary>
        private static readonly FontFamilyConverter FontFamilyConverter = new FontFamilyConverter();

        /// <summary>
        /// The cached font families.
        /// </summary>
        private static FontFamily[] cachedFontFamilies;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGridControlFactory" /> class.
        /// </summary>
        public PropertyGridControlFactory()
        {
            this.Converters = new List<PropertyConverter>();
            this.Editors = new List<TypeEditor>();
        }

        /// <summary>
        /// Gets or sets the list of converters.
        /// </summary>
        /// <value>The converters.</value>
        public List<PropertyConverter> Converters { get; set; }

        /// <summary>
        /// Gets or sets the list of type editors.
        /// </summary>
        /// <value>The editors.</value>
        public List<TypeEditor> Editors { get; set; }

        /// <summary>
        /// Gets or sets the file dialog service.
        /// </summary>
        /// <value>The file dialog service.</value>
        public IFileDialogService FileDialogService { get; set; }

        /// <summary>
        /// Gets or sets the folder browser dialog service.
        /// </summary>
        /// <value>The folder browser dialog service.</value>
        public IFolderBrowserDialogService FolderBrowserDialogService { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the DatePicker control for DateTime values.
        /// </summary>
        public bool UseDatePicker { get; set; }

        /// <summary>
        /// Creates the control for a property.
        /// </summary>
        /// <param name="property">The property item.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// A element.
        /// </returns>
        public virtual FrameworkElement CreateControl(PropertyItem property, PropertyControlFactoryOptions options)
        {
            this.UpdateConverter(property);

            foreach (var editor in this.Editors)
            {
                if (editor.IsAssignable(property.Descriptor.PropertyType))
                {
                    return this.CreateEditorControl(property, editor);
                }
            }

            if (property.Is(typeof(bool)))
            {
                return this.CreateBoolControl(property);
            }

            if (property.Is(typeof(Enum)))
            {
                return this.CreateEnumControl(property, options);
            }

            if (property.Is(typeof(Color)))
            {
                return this.CreateColorControl(property);
            }

            if (property.Is(typeof(Brush)))
            {
                return this.CreateBrushControl(property);
            }

            if (property.Is(typeof(FontFamily)) || property.IsFontFamilySelector)
            {
                return this.CreateFontFamilyControl(property);
            }

            if (property.Is(typeof(ImageSource)) || property.DataTypes.Contains(DataType.ImageUrl))
            {
                return this.CreateImageControl(property);
            }

            if (property.Is(typeof(ICommand)))
            {
                return this.CreateCommandControl(property);
            }

            if (property.DataTypes.Contains(DataType.Html))
            {
                return this.CreateHtmlControl(property);
            }

            if (property.Is(typeof(Uri)))
            {
                return this.CreateLinkControl(property);
            }

            if (property.ItemsSourceDescriptor != null || property.ItemsSource != null)
            {
                return this.CreateComboBoxControl(property);
            }

            if (property.Is(typeof(SecureString)))
            {
                return this.CreateSecurePasswordControl(property);
            }

            if (this.UseDatePicker && property.Is(typeof(DateTime)))
            {
                return this.CreateDateTimeControl(property);
            }

            if (property.IsFilePath)
            {
                return this.CreateFilePathControl(property);
            }

            if (property.IsDirectoryPath)
            {
                return this.CreateDirectoryPathControl(property);
            }

            if (property.PreviewFonts)
            {
                return this.CreateFontPreview(property);
            }

            if (property.IsComment)
            {
                return this.CreateCommentControl(property);
            }

            if (property.IsContent)
            {
                return this.CreateContentControl(property);
            }

            if (property.IsPassword)
            {
                return this.CreatePasswordControl(property);
            }

            if (property.IsSlidable)
            {
                return this.CreateSliderControl(property);
            }

            if (property.IsSpinnable)
            {
                return this.CreateSpinControl(property);
            }

            if (property.CheckableItemsIsCheckedPropertyName != null)
            {
                return this.CreateCheckableItems(property);
            }

            if (property.Is(typeof(IDictionary)) || property.Is(typeof(IDictionary<,>)))
            {
                return this.CreateDictionaryControl(property);
            }

            if (property.Is(typeof(ICollection)) || property.Is(typeof(ICollection<>)))
            {
                return this.CreateGridControl(property);
            }

            return this.CreateDefaultControl(property);
        }

        /// <summary>
        /// Creates the error control.
        /// </summary>
        public virtual ContentControl CreateErrorControl(PropertyItem pi, object instance, Tab tab, PropertyControlFactoryOptions options)
        {
            var dataErrorInfoInstance = instance as IDataErrorInfo;
            var notifyDataErrorInfoInstance = instance as INotifyDataErrorInfo;

            var errorControl = new ContentControl
            {
                ContentTemplate = options.ValidationErrorTemplate,
                Focusable = false
            };
            IValueConverter errorConverter;
            string propertyPath;
            object source = null;
            if (dataErrorInfoInstance != null)
            {
                errorConverter = new DataErrorInfoConverter(dataErrorInfoInstance, pi.PropertyName);
                propertyPath = pi.PropertyName;
                source = instance;
            }
            else
            {
                errorConverter = new NotifyDataErrorInfoConverter(notifyDataErrorInfoInstance, pi.PropertyName);
                propertyPath = nameof(tab.HasErrors);
                source = tab;
                notifyDataErrorInfoInstance.ErrorsChanged += (s, e) =>
                {
                    tab.UpdateHasErrors(notifyDataErrorInfoInstance);
                };
            }

            var visibilityBinding = new Binding(propertyPath)
            {
                Converter = errorConverter,
                NotifyOnTargetUpdated = true,
#if !NET40
                ValidatesOnNotifyDataErrors = false,
#endif
                Source = source,
            };

            var contentBinding = new Binding(propertyPath)
            {
                Converter = errorConverter,
#if !NET40
                ValidatesOnNotifyDataErrors = false,
#endif
                Source = source,
            };

            errorControl.SetBinding(UIElement.VisibilityProperty, visibilityBinding);

            // When the visibility of the error control is changed, updated the HasErrors of the tab
            errorControl.TargetUpdated += (s, e) =>
            {
                if (dataErrorInfoInstance != null)
                    tab.UpdateHasErrors(dataErrorInfoInstance);
            };
            errorControl.SetBinding(ContentControl.ContentProperty, contentBinding);
            return errorControl;
        }

        /// <summary>
        /// sets error style for control
        /// </summary>
        /// <param name="control"></param>
        /// <param name="options"></param>
        public virtual void SetValidationErrorStyle(FrameworkElement control, PropertyControlFactoryOptions options)
        {
            if (options.ValidationErrorStyle != null)
            {
                control.Style = options.ValidationErrorStyle;
            }
            //return control;
        }

        /// <summary>
        /// Updates the tab for validation results.
        /// </summary>
        /// <param name="tab">The tab.</param>
        /// <param name="errorInfo">The error information.</param>
        public virtual void UpdateTabForValidationResults(Tab tab, object errorInfo)
        {
            if (errorInfo is INotifyDataErrorInfo ndei)
            {
                tab.HasErrors = tab.Groups.Any(g => g.Properties.Any(p => ndei.HasErrors));
            }
            else if (errorInfo is IDataErrorInfo dei)
            {
                tab.HasErrors = tab.Groups.Any(g => g.Properties.Any(p => !string.IsNullOrEmpty(dei[p.PropertyName])));
            }
        }

        /// <summary>
        /// Converts the horizontal alignment.
        /// </summary>
        /// <param name="a">The alignment to convert.</param>
        /// <returns>
        /// A <see cref="HorizontalAlignment" />.
        /// </returns>
        protected static HorizontalAlignment ConvertHorizontalAlignment(DataAnnotations.HorizontalAlignment a)
        {
            switch (a)
            {
                case DataAnnotations.HorizontalAlignment.Center:
                    return HorizontalAlignment.Center;
                case DataAnnotations.HorizontalAlignment.Right:
                    return HorizontalAlignment.Right;
                default:
                    return HorizontalAlignment.Left;
            }
        }

        /// <summary>
        /// Creates the checkbox control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateBoolControl(PropertyItem property)
        {
            if (property.Descriptor.IsReadOnly())
            {
                var cm = new CheckMark
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                cm.SetBinding(CheckMark.IsCheckedProperty, property.CreateBinding());
                return cm;
            }

            var c = new CheckBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            c.SetBinding(ToggleButton.IsCheckedProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the command control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateCommandControl(PropertyItem property)
        {
            var button = new Button
            {
                Content = "Execute",
                Width = 100,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            button.SetBinding(Button.CommandProperty, property.CreateOneWayBinding());
            return button;
        }

        /// <summary>
        /// Creates a control based on a template from a a <see cref="TypeEditor" />.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="editor">The editor.</param>
        /// <returns>
        /// A <see cref="ContentControl" />.
        /// </returns>
        protected virtual ContentControl CreateEditorControl(PropertyItem property, TypeEditor editor)
        {
            var c = new ContentControl
            {
                ContentTemplate = editor.EditorTemplate,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            c.SetBinding(FrameworkElement.DataContextProperty, property.CreateOneWayBinding());
            return c;
        }

        /// <summary>
        /// Creates the brush control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateBrushControl(PropertyItem property)
        {
            var c = new ColorPicker();
            c.SetBinding(ColorPicker.SelectedColorProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the color control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateColorControl(PropertyItem property)
        {
            var c = new ColorPicker();
            c.SetBinding(ColorPicker.SelectedColorProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the combo box control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateComboBoxControl(PropertyItem property)
        {
            var c = new ComboBox { IsEditable = property.IsEditable, ItemsSource = property.ItemsSource, VerticalContentAlignment = VerticalAlignment.Center };
            if (property.ItemsSourceDescriptor != null)
            {
                c.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(property.ItemsSourceDescriptor.Name));
            }

            c.SelectedValuePath = property.SelectedValuePath;
            c.DisplayMemberPath = property.DisplayMemberPath;

            c.SetBinding(property.IsEditable ? ComboBox.TextProperty : Selector.SelectedValueProperty, property.CreateBinding());

            return c;
        }

        /// <summary>
        /// Creates a comment control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateCommentControl(PropertyItem property)
        {
            var tb = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(4),
                Focusable = false,
                TextWrapping = TextWrapping.Wrap
            };
            ScrollViewer.SetHorizontalScrollBarVisibility(tb, ScrollBarVisibility.Hidden);
            ScrollViewer.SetVerticalScrollBarVisibility(tb, ScrollBarVisibility.Hidden);
            tb.SetBinding(TextBlock.TextProperty, property.CreateBinding());
            return tb;
        }

        /// <summary>
        /// Creates a content control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// A <see cref="ContentControl" />.
        /// </returns>
        protected virtual FrameworkElement CreateContentControl(PropertyItem property)
        {
            var b = new ContentControl
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(4),
                Focusable = false,
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };
            b.SetBinding(ContentControl.ContentProperty, property.CreateBinding());
            return b;
        }

        /// <summary>
        /// Creates the date time control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateDateTimeControl(PropertyItem property)
        {
            var c = new DatePicker();
            c.SetBinding(DatePicker.SelectedDateProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the default control (text box).
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateDefaultControl(PropertyItem property)
        {
            var trigger = property.AutoUpdateText ? UpdateSourceTrigger.PropertyChanged : UpdateSourceTrigger.Default;
            var c = new TextBoxEx
            {
                AcceptsReturn = property.AcceptsReturn,
                MaxLength = property.MaxLength,
                IsReadOnly = property.IsReadOnly,
                TextWrapping = property.TextWrapping,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalContentAlignment = property.AcceptsReturn ? VerticalAlignment.Top : VerticalAlignment.Center
            };

            if (property.FontFamily != null)
            {
                c.FontFamily = new FontFamily(property.FontFamily);
            }

            if (!double.IsNaN(property.FontSize))
            {
                c.FontSize = property.FontSize;
            }

            if (property.IsReadOnly)
            {
                c.Foreground = Brushes.RoyalBlue;
            }

            var binding = property.CreateBinding(trigger);
            if (property.ActualPropertyType != typeof(string) && IsNullable(property.ActualPropertyType))
            {
                // Empty values should set the source to null
                // Set the value that is used in the target when the value of the source is null.
                binding.TargetNullValue = string.Empty;
            }

            c.SetBinding(TextBox.TextProperty, binding);

            return c;
        }

        /// <summary>
        /// Creates a dictionary control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateDictionaryControl(PropertyItem property)
        {
            var c = new ComboBox();
            c.SetBinding(ItemsControl.ItemsSourceProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the directory path control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateDirectoryPathControl(PropertyItem property)
        {
            var c = new DirectoryPicker { FolderBrowserDialogService = this.FolderBrowserDialogService };
            var trigger = property.AutoUpdateText ? UpdateSourceTrigger.PropertyChanged : UpdateSourceTrigger.Default;
            c.SetBinding(DirectoryPicker.DirectoryProperty, property.CreateBinding(trigger));
            return c;
        }

        /// <summary>
        /// Gets the values for the specified enumeration type.
        /// </summary>
        /// <param name="enumType">The enumeration type.</param>
        /// <returns>A sequence of values.</returns>
        protected virtual IEnumerable<object> GetEnumValues(Type enumType)
        {
            var ult = Nullable.GetUnderlyingType(enumType);
            var isNullable = ult != null;
            if (isNullable)
            {
                enumType = ult;
            }

            var enumValues = Enum.GetValues(enumType).FilterOnBrowsableAttribute().ToList();
            if (isNullable)
            {
                enumValues.Add(null);
            }

            return enumValues;
        }

        /// <summary>
        /// Creates the select control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateEnumControl(
            PropertyItem property, PropertyControlFactoryOptions options)
        {
            // TODO: use check boxes for bit fields
            //// var isBitField = property.Descriptor.PropertyType.GetTypeInfo().GetCustomAttributes<FlagsAttribute>().Any();

            var values = this.GetEnumValues(property.Descriptor.PropertyType).ToArray();
            var style = property.SelectorStyle;
            if (style == DataAnnotations.SelectorStyle.Auto)
            {
                style = values.Length > options.EnumAsRadioButtonsLimit
                            ? DataAnnotations.SelectorStyle.ComboBox
                            : DataAnnotations.SelectorStyle.RadioButtons;
            }

            switch (style)
            {
                case DataAnnotations.SelectorStyle.RadioButtons:
                    {
                        var c = new RadioButtonList { EnumType = property.Descriptor.PropertyType };
                        c.SetBinding(RadioButtonList.ValueProperty, property.CreateBinding());
                        return c;
                    }

                case DataAnnotations.SelectorStyle.ComboBox:
                    {
                        var c = new ComboBox { ItemsSource = values };
                        c.SetBinding(Selector.SelectedValueProperty, property.CreateBinding());
                        return c;
                    }

                case DataAnnotations.SelectorStyle.ListBox:
                    {
                        var c = new ListBox { ItemsSource = values };
                        c.SetBinding(Selector.SelectedValueProperty, property.CreateBinding());
                        return c;
                    }

                default:
                    return null;
            }
        }

        /// <summary>
        /// Creates the file path control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateFilePathControl(PropertyItem property)
        {
            var c = new FilePicker
            {
                Filter = property.FilePathFilter,
                DefaultExtension = property.FilePathDefaultExtension,
                UseOpenDialog = property.IsFileOpenDialog,
                FileDialogService = this.FileDialogService
            };

            if (property.RelativePathDescriptor != null)
            {
                c.SetBinding(FilePicker.BasePathProperty, new Binding(property.RelativePathDescriptor.Name));
            }

            if (property.FilterDescriptor != null)
            {
                c.SetBinding(FilePicker.FilterProperty, new Binding(property.FilterDescriptor.Name));
            }

            if (property.DefaultExtensionDescriptor != null)
            {
                c.SetBinding(FilePicker.DefaultExtensionProperty, new Binding(property.DefaultExtensionDescriptor.Name));
            }

            var trigger = property.AutoUpdateText ? UpdateSourceTrigger.PropertyChanged : UpdateSourceTrigger.Default;
            c.SetBinding(FilePicker.FilePathProperty, property.CreateBinding(trigger));
            return c;
        }

        /// <summary>
        /// Creates the font family control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateFontFamilyControl(PropertyItem property)
        {
            var c = new ComboBox { ItemsSource = GetFontFamilies() };

            if (property.PreviewFonts)
            {
                var dt = new DataTemplate { DataType = typeof(ComboBox) };
                var factory = new FrameworkElementFactory(typeof(TextBlock));
                factory.SetValue(TextBlock.FontSizeProperty, property.FontSize);
                factory.SetValue(TextBlock.FontWeightProperty, FontWeight.FromOpenTypeWeight(property.FontWeight));
                factory.SetBinding(TextBlock.TextProperty, new Binding());
                factory.SetBinding(TextBlock.FontFamilyProperty, new Binding { Converter = FontFamilyConverter });
                dt.VisualTree = factory;
                c.ItemTemplate = dt;
            }

            var binding = property.CreateBinding();
            if (property.ActualPropertyType == typeof(string))
            {
                binding.Converter = FontFamilyConverter;
            }

            c.SetBinding(Selector.SelectedValueProperty, binding);
            return c;
        }

        /// <summary>
        /// Creates the font preview.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateFontPreview(PropertyItem property)
        {
            var c = new TextBoxEx
            {
                Background = Brushes.Transparent,
                BorderBrush = null,
                AcceptsReturn = true,
                IsReadOnly = property.IsReadOnly,
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeight.FromOpenTypeWeight(property.FontWeight),
                FontSize = property.FontSize
            };
            TextOptions.SetTextFormattingMode(c, TextFormattingMode.Display);
            TextOptions.SetTextRenderingMode(c, TextRenderingMode.ClearType);
            c.SetBinding(TextBox.TextProperty, new Binding(property.Descriptor.Name) { Mode = BindingMode.OneWay });
            if (property.FontFamilyPropertyDescriptor != null)
            {
                c.SetBinding(Control.FontFamilyProperty, new Binding(property.FontFamilyPropertyDescriptor.Name));
            }

            return c;
        }

        /// <summary>
        /// Creates the grid control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateGridControl(PropertyItem property)
        {
            var c = new DataGrid
            {
                CanDelete = property.ListCanRemove,
                CanInsert = property.ListCanAdd,
                InputDirection = property.InputDirection,
                IsEasyInsertByMouseEnabled = property.IsEasyInsertByMouseEnabled,
                IsEasyInsertByKeyboardEnabled = property.IsEasyInsertByKeyboardEnabled,
                AutoGenerateColumns = property.Columns.Count == 0
            };

            foreach (var cd in property.Columns)
            {
                if (cd.PropertyName == string.Empty && property.ListItemItemsSource != null)
                {
                    cd.ItemsSource = property.ListItemItemsSource;
                }

                c.ColumnDefinitions.Add(cd);
            }

            c.SetBinding(DataGrid.ItemsSourceProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the HTML control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateHtmlControl(PropertyItem property)
        {
            var c = new WebBrowser();
            c.SetBinding(WebBrowserBehavior.NavigateToStringProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the image control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateImageControl(PropertyItem property)
        {
            var c = new Image { Stretch = Stretch.Uniform, HorizontalAlignment = HorizontalAlignment.Left };
            c.SetBinding(Image.SourceProperty, property.CreateOneWayBinding());
            return c;
        }

        /// <summary>
        /// Creates the link control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateLinkControl(PropertyItem property)
        {
            var c = new LinkBlock { VerticalAlignment = VerticalAlignment.Center };
            c.SetBinding(TextBlock.TextProperty, new Binding(property.Descriptor.Name));
            c.SetBinding(LinkBlock.NavigateUriProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the password control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreatePasswordControl(PropertyItem property)
        {
            var c = new PasswordBox();
            PasswordHelper.SetAttach(c, true);
            c.SetBinding(PasswordHelper.PasswordProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the secure password control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateSecurePasswordControl(PropertyItem property)
        {
            var c = new PasswordBox();

            // PasswordHelper.SetAttach(b, true);
            // b.SetBinding(PasswordHelper.PasswordProperty, pi.CreateBinding());

            return c;
        }

        /// <summary>
        /// Creates the slider control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateSliderControl(PropertyItem property)
        {
            var g = new Grid();
            g.ColumnDefinitions.Add(
                new System.Windows.Controls.ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            g.ColumnDefinitions.Add(
                new System.Windows.Controls.ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            var s = new Slider
            {
                Minimum = property.SliderMinimum,
                Maximum = property.SliderMaximum,
                SmallChange = property.SliderSmallChange,
                LargeChange = property.SliderLargeChange,
                TickFrequency = property.SliderTickFrequency,
                IsSnapToTickEnabled = property.SliderSnapToTicks
            };
            s.SetBinding(RangeBase.ValueProperty, property.CreateBinding());
            g.Children.Add(s);

            var trigger = property.AutoUpdateText ? UpdateSourceTrigger.PropertyChanged : UpdateSourceTrigger.Default;
            var c = new TextBoxEx { IsReadOnly = property.Descriptor.IsReadOnly };

            var formatString = property.FormatString;
            if (formatString != null && !formatString.StartsWith("{"))
            {
                formatString = "{0:" + formatString + "}";
            }

            var binding = property.CreateBinding();
            binding.StringFormat = formatString;
            binding.UpdateSourceTrigger = trigger;

            c.SetBinding(TextBox.TextProperty, binding);

            Grid.SetColumn(c, 1);
            g.Children.Add(c);

            return g;
        }

        /// <summary>
        /// Creates the spin control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateSpinControl(PropertyItem property)
        {
            var tb = new TextBoxEx
            {
                IsReadOnly = property.IsReadOnly,
                BorderThickness = new Thickness(0),
                HorizontalContentAlignment = ConvertHorizontalAlignment(property.HorizontalAlignment),
                VerticalContentAlignment = VerticalAlignment.Center
            };
            tb.SetBinding(TextBox.TextProperty, property.CreateBinding());
            var c = new SpinControl
            {
                Maximum = property.SpinMaximum,
                Minimum = property.SpinMinimum,
                SmallChange = property.SpinSmallChange,
                LargeChange = property.SpinLargeChange,
                Content = tb
            };

            // Note: Do not apply the converter to the SpinControl
            c.SetBinding(SpinControl.ValueProperty, property.CreateBinding(UpdateSourceTrigger.Default, false));
            return c;
        }

        /// <summary>
        /// Creates a sequence of checkboxes.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// A FrameworkElement.
        /// </returns>
        protected virtual FrameworkElement CreateCheckableItems(PropertyItem property)
        {
            var lb = new ItemsControl();
            var rectangleFactory = new FrameworkElementFactory(typeof(CheckBox));
            rectangleFactory.SetBinding(ToggleButton.IsCheckedProperty, new Binding(property.CheckableItemsIsCheckedPropertyName));
            rectangleFactory.SetBinding(ContentControl.ContentProperty, new Binding(property.CheckableItemsContentPropertyName));

            lb.ItemTemplate = new DataTemplate { VisualTree = rectangleFactory };
            lb.SetBinding(ItemsControl.ItemsSourceProperty, property.CreateBinding());
            lb.Margin = new Thickness(0, 6, 0, 6);

            return lb;
        }

        /// <summary>
        /// Updates the converter from the Converters collection.
        /// </summary>
        /// <param name="property">The property.</param>
        protected void UpdateConverter(PropertyItem property)
        {
            if (property.Converter == null)
            {
                foreach (var c in this.Converters)
                {
                    if (c.IsAssignable(property.Descriptor.PropertyType))
                    {
                        property.Converter = c.Converter;
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified type is a reference type or a <see cref="Nullable" />.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <c>true</c> if the specified type can be set to <c>null</c>; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsNullable(Type type)
        {
            if (!type.IsValueType)
            {
                // Reference types are nullable
                return true;
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                // Nullable value type
                return true;
            }

            // Value type
            return false;
        }

        /// <summary>
        /// Gets the font families.
        /// </summary>
        /// <returns>
        /// List of font families.
        /// </returns>
        private static IEnumerable<FontFamily> GetFontFamilies()
        {
            if (cachedFontFamilies == null)
            {
                var list = new List<FontFamily>();
                foreach (var fontFamily in Fonts.SystemFontFamilies)
                {
                    // Instantiate a TypeFace object with the font settings you want to use
                    var ltypFace = new Typeface(fontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

                    try
                    {
                        GlyphTypeface gtf;
                        if (ltypFace.TryGetGlyphTypeface(out gtf))
                        {
                            list.Add(fontFamily);
                        }
                    }
                    catch (FileFormatException)
                    {
                        Debug.WriteLine(fontFamily + " failed.");
                    }
                }

                cachedFontFamilies = list.OrderBy(f => f.ToString()).ToArray();
            }

            return cachedFontFamilies;
        }
    }
}