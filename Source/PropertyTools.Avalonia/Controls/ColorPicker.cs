// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPicker.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace PropertyTools.Avalonia.Controls;

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

    private ColorView? colorView;
    private Border? border;

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
        colorView = new ColorView
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        border = new Border
        {
            BorderThickness = new Thickness(1),
            BorderBrush = Brushes.Gray,
            Padding = new Thickness(5),
            Child = colorView
        };

        Content = border;
    }

    /// <summary>
    /// Called when a property value changes.
    /// </summary>
    /// <param name="change">The property change event args.</param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SelectedColorProperty)
        {
            OnSelectedColorChanged((Color)change.NewValue!);
        }
    }

    private void OnSelectedColorChanged(Color color)
    {
        if (colorView != null)
        {
            colorView.Color = color;
        }
    }
}
