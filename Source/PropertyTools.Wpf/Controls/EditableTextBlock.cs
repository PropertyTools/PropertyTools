// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableTextBlock.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;

namespace PropertyTools.Wpf.Controls;

/// <summary>
/// Represents an editable text block control for Avalonia.
/// A TextBlock that can be changed into a TextBox for in-place editing.
/// </summary>
public class EditableTextBlock : UserControl
{
    /// <summary>
    /// Defines the <see cref="Text"/> property.
    /// </summary>
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<EditableTextBlock, string>(nameof(Text), string.Empty);

    /// <summary>
    /// Defines the <see cref="IsEditing"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsEditingProperty =
        AvaloniaProperty.Register<EditableTextBlock, bool>(nameof(IsEditing), false);

    private TextBlock? textBlock;
    private TextBox? textBox;
    private Panel? container;

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the control is in editing mode.
    /// </summary>
    public bool IsEditing
    {
        get => GetValue(IsEditingProperty);
        set => SetValue(IsEditingProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditableTextBlock"/> class.
    /// </summary>
    public EditableTextBlock()
    {
        container = new Grid();
        
        textBlock = new TextBlock
        {
            VerticalAlignment = VerticalAlignment.Center,
            [!TextBlock.TextProperty] = this[!TextProperty]
        };
        
        textBox = new TextBox
        {
            VerticalAlignment = VerticalAlignment.Center,
            IsVisible = false,
            [!TextBox.TextProperty] = this[!TextProperty]
        };
        
        textBlock.DoubleTapped += OnTextBlockDoubleTapped;
        textBox.LostFocus += OnTextBoxLostFocus;
        
        container.Children.Add(textBlock);
        container.Children.Add(textBox);
        
        Content = container;
    }

    private void OnTextBlockDoubleTapped(object? sender, TappedEventArgs e)
    {
        IsEditing = true;
        UpdateVisibility();
        textBox?.Focus();
        textBox?.SelectAll();
    }

    private void OnTextBoxLostFocus(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsEditing = false;
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (textBlock != null)
            textBlock.IsVisible = !IsEditing;
        if (textBox != null)
            textBox.IsVisible = IsEditing;
    }
}
