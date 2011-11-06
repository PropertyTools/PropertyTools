// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultPropertyControlFactory.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
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
        ///   Brush to color converter.
        /// </summary>
        private static readonly BrushToColorConverter BrushToColorConverter = new BrushToColorConverter();

        /// <summary>
        ///   The cached font families.
        /// </summary>
        private static FontFamily[] cachedFontFamilies;

        #endregion

        #region Public Properties

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the control for a property.
        /// </summary>
        /// <param name="pi">
        /// The property item.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// A element.
        /// </returns>
        public FrameworkElement CreateControl(PropertyItem pi, PropertyControlFactoryOptions options)
        {
            var bindingMode = pi.Descriptor.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;

            if (pi.Is(typeof(bool)))
            {
                var c = new CheckBox
                    {
                       VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left 
                    };
                c.SetBinding(
                    ToggleButton.IsCheckedProperty, 
                    new Binding(pi.Descriptor.Name)
                        {
                           Mode = bindingMode, ValidatesOnDataErrors = true, ValidatesOnExceptions = true 
                        });
                return c;
            }

            if (pi.Is(typeof(Enum)))
            {
                var isRadioButton = pi.UseRadioButtons;
                var enumType = TypeHelper.GetEnumType(pi.Descriptor.PropertyType);
                var values = Enum.GetValues(enumType);
                if (values.Length > options.EnumAsRadioButtonsLimit)
                {
                    isRadioButton = false;
                }

                if (isRadioButton)
                {
                    var c = new RadioButtonList { EnumType = pi.Descriptor.PropertyType };
                    c.SetBinding(
                        RadioButtonList.ValueProperty, 
                        new Binding(pi.Descriptor.Name)
                            {
                               Mode = bindingMode, ValidatesOnDataErrors = true, ValidatesOnExceptions = true 
                            });
                    return c;
                }
                else
                {
                    var c = new ComboBox { ItemsSource = Enum.GetValues(enumType) };
                    c.SetBinding(
                        Selector.SelectedValueProperty, 
                        new Binding(pi.Descriptor.Name)
                            {
                               Mode = bindingMode, ValidatesOnDataErrors = true, ValidatesOnExceptions = true 
                            });
                    return c;
                }
            }

            if (pi.Is(typeof(Color)))
            {
                var c = new ColorPicker();
                c.SetBinding(
                    ColorPicker.SelectedColorProperty, 
                    new Binding(pi.Descriptor.Name)
                        {
                           Mode = bindingMode, ValidatesOnDataErrors = true, ValidatesOnExceptions = true 
                        });
                return c;
            }

            if (pi.Is(typeof(Brush)))
            {
                var c = new ColorPicker();
                c.SetBinding(
                    ColorPicker.SelectedColorProperty, 
                    new Binding(pi.Descriptor.Name)
                        {
                            Mode = bindingMode, 
                            ValidatesOnDataErrors = true, 
                            ValidatesOnExceptions = true, 
                            Converter = BrushToColorConverter
                        });
                return c;
            }

            if (pi.Is(typeof(FontFamily)))
            {
                var c = new ComboBox { ItemsSource = GetFontFamilies() };

                if (pi.PreviewFonts)
                {
                    var dt = new DataTemplate { DataType = typeof(ComboBox) };
                    var factory = new FrameworkElementFactory(typeof(TextBlock));
                    factory.SetValue(TextBlock.FontSizeProperty, pi.FontSize);
                    factory.SetValue(TextBlock.FontWeightProperty, FontWeight.FromOpenTypeWeight(pi.FontWeight));
                    factory.SetBinding(TextBlock.TextProperty, new Binding());
                    factory.SetBinding(TextBlock.FontFamilyProperty, new Binding());
                    dt.VisualTree = factory;
                    c.ItemTemplate = dt;
                }

                c.SetBinding(
                    Selector.SelectedValueProperty, 
                    new Binding(pi.Descriptor.Name)
                        {
                           Mode = bindingMode, ValidatesOnDataErrors = true, ValidatesOnExceptions = true 
                        });
                return c;
            }

            if (pi.Is(typeof(ImageSource)) || pi.DataTypes.Contains(DataType.ImageUrl))
            {
                var c = new Image { Stretch = Stretch.Uniform, HorizontalAlignment = HorizontalAlignment.Left };
                c.SetBinding(Image.SourceProperty, new Binding(pi.Descriptor.Name));
                return c;
            }

            if (pi.DataTypes.Contains(DataType.Html))
            {
                var c = new WebBrowser();
                c.SetBinding(WebBrowserBehavior.NavigateToStringProperty, new Binding(pi.Descriptor.Name));
                return c;
            }

            if (pi.Is(typeof(Uri)))
            {
                var c = new LinkBlock { VerticalAlignment = VerticalAlignment.Center };
                c.SetBinding(TextBlock.TextProperty, new Binding(pi.Descriptor.Name));
                c.SetBinding(LinkBlock.NavigateUriProperty, new Binding(pi.Descriptor.Name));
                return c;
            }

            if (pi.ValuesDescriptor != null)
            {
                var c = new ComboBox();
                c.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(pi.ValuesDescriptor.Name));
                c.SetBinding(
                    Selector.SelectedValueProperty, 
                    new Binding(pi.Descriptor.Name)
                        {
                           Mode = bindingMode, ValidatesOnDataErrors = true, ValidatesOnExceptions = true 
                        });
                return c;
            }

            if (pi.Is(typeof(SecureString)))
            {
                // todo
                var c = new PasswordBox();

                // PasswordHelper.SetAttach(b, true);
                // b.SetBinding(PasswordHelper.PasswordProperty, new Binding(pi.Descriptor.Name) { Mode = bindingMode, ValidatesOnDataErrors = true });
                return c;
            }

            if (pi.Is(typeof(DateTime)))
            {
                var c = new DatePicker();
                c.SetBinding(
                    DatePicker.SelectedDateProperty, 
                    new Binding(pi.Descriptor.Name)
                        {
                           Mode = bindingMode, ValidatesOnDataErrors = true, ValidatesOnExceptions = true 
                        });
                return c;
            }

            if (pi.IsFilePath)
            {
                var c = new FilePicker
                    {
                        Filter = pi.FilePathFilter, 
                        DefaultExtension = pi.FilePathDefaultExtension, 
                        UseOpenDialog = pi.IsFileOpenDialog, 
                        FileDialogService = this.FileDialogService
                    };
                if (pi.RelativePathDescriptor != null)
                {
                    c.SetBinding(FilePicker.BasePathProperty, new Binding(pi.RelativePathDescriptor.Name));
                }

                if (pi.FilterDescriptor != null)
                {
                    c.SetBinding(FilePicker.FilterProperty, new Binding(pi.FilterDescriptor.Name));
                }

                c.SetBinding(
                    FilePicker.FilePathProperty, 
                    new Binding(pi.Descriptor.Name)
                        {
                           Mode = bindingMode, ValidatesOnDataErrors = true, ValidatesOnExceptions = true 
                        });
                return c;
            }

            if (pi.IsDirectoryPath)
            {
                var c = new DirectoryPicker { FolderBrowserDialogService = this.FolderBrowserDialogService };
                c.SetBinding(
                    DirectoryPicker.DirectoryProperty, 
                    new Binding(pi.Descriptor.Name)
                        {
                           Mode = bindingMode, ValidatesOnDataErrors = true, ValidatesOnExceptions = true 
                        });
                return c;
            }

            if (pi.PreviewFonts)
            {
                var c = new TextBox
                    {
                        Background = Brushes.Transparent, 
                        BorderBrush = null, 
                        AcceptsReturn = true, 
                        TextWrapping = TextWrapping.Wrap, 
                        FontWeight = FontWeight.FromOpenTypeWeight(pi.FontWeight), 
                        FontSize = pi.FontSize
                    };
                TextOptions.SetTextFormattingMode(c, TextFormattingMode.Display);
                TextOptions.SetTextRenderingMode(c, TextRenderingMode.ClearType);
                c.SetBinding(TextBox.TextProperty, new Binding(pi.Descriptor.Name) { Mode = BindingMode.OneWay });
                if (pi.FontFamilyPropertyDescriptor != null)
                {
                    c.SetBinding(Control.FontFamilyProperty, new Binding(pi.FontFamilyPropertyDescriptor.Name));
                }

                return c;
            }

            if (pi.IsComment)
            {
                var b = new ContentControl
                    {
                       VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(4), Focusable = false 
                    };
                b.SetBinding(
                    ContentControl.ContentProperty, 
                    new Binding(pi.Descriptor.Name)
                        {
                           Mode = bindingMode, ValidatesOnDataErrors = true, ValidatesOnExceptions = true 
                        });
                return b;
            }

            if (pi.IsPassword)
            {
                var c = new PasswordBox();
                PasswordHelper.SetAttach(c, true);
                c.SetBinding(
                    PasswordHelper.PasswordProperty, 
                    new Binding(pi.Descriptor.Name)
                        {
                           Mode = bindingMode, ValidatesOnDataErrors = true, ValidatesOnExceptions = true 
                        });
                return c;
            }

            if (pi.Is(typeof(IList)))
            {
                var c = new SimpleGrid();
                var glc = new GridLengthConverter();
                foreach (var ca in pi.Columns.OrderBy(cd => cd.ColumnIndex))
                {
                    var cd = new ColumnDefinition();
                    cd.DataField = ca.PropertyName;
                    cd.Header = ca.Header;
                    cd.FormatString = ca.FormatString;
                    cd.Width = (GridLength)glc.ConvertFromInvariantString(ca.Width);
                    switch (ca.Alignment)
                    {
                        case 'L':
                            cd.HorizontalAlignment = HorizontalAlignment.Left;
                            break;
                        case 'R':
                            cd.HorizontalAlignment = HorizontalAlignment.Right;
                            break;
                        case 'C':
                            cd.HorizontalAlignment = HorizontalAlignment.Center;
                            break;
                        case 'S':
                            cd.HorizontalAlignment = HorizontalAlignment.Stretch;
                            break;
                    }

                    c.ColumnDefinitions.Add(cd);
                }

                c.SetBinding(
                    SimpleGrid.ContentProperty, 
                    new Binding(pi.Descriptor.Name)
                        {
                           Mode = bindingMode, ValidatesOnDataErrors = true, ValidatesOnExceptions = true 
                        });
                return c;
            }

            if (pi.Is(typeof(IDictionary)))
            {
                // todo
                var c = new ComboBox();
                c.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(pi.Descriptor.Name));
                return c;
            }
            {
                // TextBox is the default control
                var trigger = pi.AutoUpdateText ? UpdateSourceTrigger.PropertyChanged : UpdateSourceTrigger.Default;
                var c = new TextBox
                    {
                        AcceptsReturn = pi.AcceptsReturn, 
                        MaxLength = pi.MaxLength, 
                        IsReadOnly = pi.Descriptor.IsReadOnly, 
                        TextWrapping = pi.TextWrapping, 
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                    };

                c.SetBinding(
                    TextBox.TextProperty, 
                    new Binding(pi.Descriptor.Name)
                        {
                            Mode = bindingMode, 
                            UpdateSourceTrigger = trigger, 
                            ValidatesOnDataErrors = true, 
                            ValidatesOnExceptions = true
                        });
                return c;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the font families.
        /// </summary>
        /// <returns>
        /// </returns>
        private static FontFamily[] GetFontFamilies()
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