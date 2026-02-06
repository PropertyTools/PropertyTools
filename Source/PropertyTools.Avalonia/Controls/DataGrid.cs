// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGrid.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PropertyTools.Avalonia.Controls;

/// <summary>
/// Represents a data grid control for Avalonia.
/// A data grid with an "Excel feel".
/// </summary>
public class DataGrid : UserControl
{
    /// <summary>
    /// Defines the <see cref="ItemsSource"/> property.
    /// </summary>
    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.Register<DataGrid, IEnumerable?>(nameof(ItemsSource), null);

    private global::Avalonia.Controls.DataGrid? dataGrid;
    private Border? border;

    /// <summary>
    /// Gets or sets the items source.
    /// </summary>
    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataGrid"/> class.
    /// </summary>
    public DataGrid()
    {
        dataGrid = new global::Avalonia.Controls.DataGrid
        {
            AutoGenerateColumns = true,
            IsReadOnly = false,
            GridLinesVisibility = DataGridGridLinesVisibility.All,
            HeadersVisibility = DataGridHeadersVisibility.All
        };

        border = new Border
        {
            BorderThickness = new Thickness(1),
            BorderBrush = global::Avalonia.Media.Brushes.Gray,
            Child = dataGrid
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

        if (change.Property == ItemsSourceProperty)
        {
            OnItemsSourceChanged(change.NewValue as IEnumerable);
        }
    }

    private void OnItemsSourceChanged(IEnumerable? items)
    {
        if (dataGrid == null)
            return;

        if (items == null)
        {
            dataGrid.ItemsSource = null;
            return;
        }

        // Convert to list for better data binding
        var itemsList = items.Cast<object>().ToList();
        dataGrid.ItemsSource = itemsList;
    }
}
