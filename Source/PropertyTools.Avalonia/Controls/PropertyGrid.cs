// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGrid.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using System;
using System.ComponentModel;

namespace PropertyTools.Avalonia.Controls;

/// <summary>
/// Represents a property grid control for Avalonia.
/// A control that shows properties of an object or a collection of objects.
/// </summary>
public class PropertyGrid : UserControl
{
    /// <summary>
    /// Defines the <see cref="SelectedObject"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> SelectedObjectProperty =
        AvaloniaProperty.Register<PropertyGrid, object?>(nameof(SelectedObject), null);

    private StackPanel? propertiesPanel;
    private ScrollViewer? scrollViewer;

    /// <summary>
    /// Gets or sets the selected object whose properties should be displayed.
    /// </summary>
    public object? SelectedObject
    {
        get => GetValue(SelectedObjectProperty);
        set => SetValue(SelectedObjectProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyGrid"/> class.
    /// </summary>
    public PropertyGrid()
    {
        propertiesPanel = new StackPanel
        {
            Spacing = 5,
            Margin = new Thickness(10)
        };

        scrollViewer = new ScrollViewer
        {
            Content = propertiesPanel
        };

        Content = scrollViewer;
    }

    /// <summary>
    /// Called when a property value changes.
    /// </summary>
    /// <param name="change">The property change event args.</param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SelectedObjectProperty)
        {
            OnSelectedObjectChanged(change.NewValue);
        }
    }

    private void OnSelectedObjectChanged(object? obj)
    {
        UpdateProperties();
    }

    private void UpdateProperties()
    {
        if (propertiesPanel == null)
            return;

        propertiesPanel.Children.Clear();

        if (SelectedObject == null)
        {
            propertiesPanel.Children.Add(new TextBlock
            {
                Text = "No object selected",
                FontStyle = global::Avalonia.Media.FontStyle.Italic
            });
            return;
        }

        var properties = TypeDescriptor.GetProperties(SelectedObject);
        
        foreach (PropertyDescriptor prop in properties)
        {
            if (!prop.IsBrowsable)
                continue;

            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("Auto,*"),
                Margin = new Thickness(0, 5, 0, 5)
            };

            var label = new TextBlock
            {
                Text = prop.DisplayName + ":",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0)
            };
            Grid.SetColumn(label, 0);

            var valueText = new TextBlock
            {
                Text = prop.GetValue(SelectedObject)?.ToString() ?? "(null)",
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(valueText, 1);

            grid.Children.Add(label);
            grid.Children.Add(valueText);
            propertiesPanel.Children.Add(grid);
        }
    }
}
