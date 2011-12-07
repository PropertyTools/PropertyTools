// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultPropertyControlFactory.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Provides a default property control factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Provides a default property control factory.
    /// </summary>
    public class DefaultPropertyControlFactory : IPropertyControlFactory
    {
        #region Constants and Fields

        /// <summary>
        ///   The cached font families.
        /// </summary>
        private static FontFamily[] cachedFontFamilies;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DefaultPropertyControlFactory" /> class.
        /// </summary>
        public DefaultPropertyControlFactory()
        {
            this.Converters = new List<PropertyConverter>();
            this.Editors = new List<TypeEditor>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the list of converters.
        /// </summary>
        /// <value>The converters.</value>
        public List<PropertyConverter> Converters { get; set; }

        /// <summary>
        ///   Gets or sets the list of type editors.
        /// </summary>
        /// <value>
        ///   The editors.
        /// </value>
        public List<TypeEditor> Editors { get; set; }

        /// <summary>
        ///   Gets or sets the file dialog service.
        /// </summary>
        /// <value>The file dialog service.</value>
        public IFileDialogService FileDialogService { get; set; }

        /// <summary>
        ///   Gets or sets the folder browser dialog service.
        /// </summary>
        /// <value>The folder browser dialog service.</value>
        public IFolderBrowserDialogService FolderBrowserDialogService { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use the DatePicker control for DateTime values.
        /// </summary>
        public bool UseDatePicker { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the control for a property.
        /// </summary>
        /// <param name="property">
        /// The property item.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
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
                    var c = new ContentControl
                        {
                            ContentTemplate = editor.EditorTemplate,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Left
                        };
                    c.SetBinding(FrameworkElement.DataContextProperty, property.CreateOneWayBinding());
                    return c;
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

            if (property.DataTypes.Contains(DataType.Html))
            {
                return this.CreateHtmlControl(property);
            }

            if (property.Is(typeof(Uri)))
            {
                return this.CreateLinkControl(property);
            }

            if (property.ItemsSourceDescriptor != null)
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

            if (property.Is(typeof(IList)))
            {
                return this.CreateGridControl(property);
            }

            if (property.Is(typeof(IDictionary)))
            {
                return this.CreateDictionaryControl(property);
            }

            return this.CreateDefaultControl(property);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the bool control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateBoolControl(PropertyItem property)
        {
            var c = new CheckBox
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
            c.SetBinding(ToggleButton.IsCheckedProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the brush control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
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
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateColorControl(PropertyItem property)
        {
            var c = new ColorPicker2();
            c.SetBinding(ColorPicker2.SelectedColorProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the combo box control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateComboBoxControl(PropertyItem property)
        {
            var c = new ComboBox { IsEditable = property.IsEditable };
            c.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(property.ItemsSourceDescriptor.Name));

            c.SetBinding(
                property.IsEditable ? ComboBox.TextProperty : Selector.SelectedValueProperty, property.CreateBinding());

            return c;
        }

        /// <summary>
        /// Creates the comment control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
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

            // var b = new ContentControl
            // {
            // VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(4), Focusable = false
            // };
            // b.SetBinding(ContentControl.ContentProperty, property.CreateBinding());
            // return b;
        }

        /// <summary>
        /// Creates the date time control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
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
        /// Creates the default control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateDefaultControl(PropertyItem property)
        {
            // TextBox is the default control
            var trigger = property.AutoUpdateText ? UpdateSourceTrigger.PropertyChanged : UpdateSourceTrigger.Default;
            var c = new TextBox
                {
                    AcceptsReturn = property.AcceptsReturn,
                    MaxLength = property.MaxLength,
                    IsReadOnly = property.Descriptor.IsReadOnly,
                    TextWrapping = property.TextWrapping,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                };

            c.SetBinding(TextBox.TextProperty, property.CreateBinding(trigger));
            return c;
        }

        /// <summary>
        /// Creates the dictionary control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateDictionaryControl(PropertyItem property)
        {
            // todo
            var c = new ComboBox();
            c.SetBinding(ItemsControl.ItemsSourceProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the directory path control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateDirectoryPathControl(PropertyItem property)
        {
            var c = new DirectoryPicker { FolderBrowserDialogService = this.FolderBrowserDialogService };
            c.SetBinding(DirectoryPicker.DirectoryProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the enum control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateEnumControl(
            PropertyItem property, PropertyControlFactoryOptions options)
        {
            var isRadioButton = true;
            var enumType = TypeHelper.GetEnumType(property.Descriptor.PropertyType);
            var values = Enum.GetValues(enumType);
            if (values.Length > options.EnumAsRadioButtonsLimit && !property.UseRadioButtons)
            {
                isRadioButton = false;
            }

            if (isRadioButton)
            {
                var c = new RadioButtonList { EnumType = property.Descriptor.PropertyType };
                c.SetBinding(RadioButtonList.ValueProperty, property.CreateBinding());
                return c;
            }
            else
            {
                var c = new ComboBox { ItemsSource = Enum.GetValues(enumType) };
                c.SetBinding(Selector.SelectedValueProperty, property.CreateBinding());
                return c;
            }
        }

        /// <summary>
        /// Creates the file path control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
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

            c.SetBinding(FilePicker.FilePathProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the font family control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
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
                factory.SetBinding(TextBlock.FontFamilyProperty, new Binding());
                dt.VisualTree = factory;
                c.ItemTemplate = dt;
            }

            c.SetBinding(Selector.SelectedValueProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the font preview.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateFontPreview(PropertyItem property)
        {
            var c = new TextBox
                {
                    Background = Brushes.Transparent,
                    BorderBrush = null,
                    AcceptsReturn = true,
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
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateGridControl(PropertyItem property)
        {
            var c = new SimpleGrid { CanDelete = property.ListCanRemove, CanInsert = property.ListCanAdd };

            var glc = new GridLengthConverter();
            foreach (var ca in property.Columns.OrderBy(cd => cd.ColumnIndex))
            {
                var cd = new ColumnDefinition
                    {
                        DataField = ca.PropertyName,
                        Header = ca.Header,
                        FormatString = ca.FormatString,
                        Width = (GridLength)(glc.ConvertFromInvariantString(ca.Width) ?? GridLength.Auto)
                    };
                switch (ca.Alignment.ToString().ToUpper())
                {
                    case "L":
                        cd.HorizontalAlignment = HorizontalAlignment.Left;
                        break;
                    case "R":
                        cd.HorizontalAlignment = HorizontalAlignment.Right;
                        break;
                    default:
                        cd.HorizontalAlignment = HorizontalAlignment.Center;
                        break;
                }

                c.ColumnDefinitions.Add(cd);
            }

            c.SetBinding(SimpleGrid.ContentProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the HTML control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
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
        /// <param name="property">
        /// The property.
        /// </param>
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
        /// <param name="property">
        /// The property.
        /// </param>
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
        /// <param name="property">
        /// The property.
        /// </param>
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
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateSecurePasswordControl(PropertyItem property)
        {
            // todoox
            var c = new PasswordBox();

            // PasswordHelper.SetAttach(b, true);
            // b.SetBinding(PasswordHelper.PasswordProperty, pi.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the slider control.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
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
            var c = new TextBox { IsReadOnly = property.Descriptor.IsReadOnly };

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
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateSpinControl(PropertyItem property)
        {
            var tb = new TextBox
                {
                    BorderThickness = new Thickness(0),
                    HorizontalContentAlignment = HorizontalAlignment.Right
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
            c.SetBinding(SpinControl.ValueProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Updates the converter from the Converters collection.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
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
                foreach (var ff in Fonts.SystemFontFamilies)
                {
                    // Instantiate a TypeFace object with the font settings you want to use
                    var ltypFace = new Typeface(ff, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

                    try
                    {
                        GlyphTypeface gtf;
                        if (ltypFace.TryGetGlyphTypeface(out gtf))
                        {
                            list.Add(ff);
                        }
                    }
                    catch (FileFormatException)
                    {
                        Debug.WriteLine(ff + " failed.");
                    }
                }

                cachedFontFamilies = list.OrderBy(f => f.ToString()).ToArray();
            }

            return cachedFontFamilies;
        }

        #endregion
    }
}