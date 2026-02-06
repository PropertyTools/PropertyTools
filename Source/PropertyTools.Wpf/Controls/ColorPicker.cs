// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPicker.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace PropertyTools.Wpf.Controls;

/// <summary>
/// Represents a color picker control for Avalonia.
/// </summary>
public class ColorPicker : UserControl
{
    /// <summary>
    /// Defines the <see cref="SelectedColor"/> property.
    /// </summary>
    public static readonly StyledProperty<Color> SelectedColorProperty =
        AvaloniaProperty.Register<ColorPicker, Color>(nameof(SelectedColor), Colors.White);

    /// <summary>
    /// Gets or sets the selected color.
    /// </summary>
    public Color SelectedColor
    {
        get => GetValue(SelectedColorProperty);
        set => SetValue(SelectedColorProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorPicker"/> class.
    /// </summary>
    public ColorPicker()
    {
        // Initialize control
        var textBlock = new TextBlock
        {
            Text = "ColorPicker Control",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
        };
        
        Content = textBlock;
    }
}
