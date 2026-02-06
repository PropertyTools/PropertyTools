// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.axaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AvaloniaExample;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // Set up data context
        DataContext = new MainWindowViewModel();
    }
}

/// <summary>
/// View model for the main window.
/// </summary>
public class MainWindowViewModel
{
    /// <summary>
    /// Gets the example person object for PropertyGrid demonstration.
    /// </summary>
    public Person ExamplePerson { get; } = new Person
    {
        FirstName = "John",
        LastName = "Doe",
        Age = 30,
        Email = "john.doe@example.com",
        IsActive = true
    };

    /// <summary>
    /// Gets the collection of people for DataGrid demonstration.
    /// </summary>
    public ObservableCollection<Person> People { get; } = new ObservableCollection<Person>
    {
        new Person { FirstName = "John", LastName = "Doe", Age = 30, Email = "john@example.com", IsActive = true },
        new Person { FirstName = "Jane", LastName = "Smith", Age = 28, Email = "jane@example.com", IsActive = true },
        new Person { FirstName = "Bob", LastName = "Johnson", Age = 35, Email = "bob@example.com", IsActive = false },
        new Person { FirstName = "Alice", LastName = "Williams", Age = 42, Email = "alice@example.com", IsActive = true },
        new Person { FirstName = "Charlie", LastName = "Brown", Age = 25, Email = "charlie@example.com", IsActive = false }
    };

    /// <summary>
    /// Gets or sets the selected color for ColorPicker demonstration.
    /// </summary>
    public Color SelectedColor { get; set; } = Colors.CornflowerBlue;
}

/// <summary>
/// Example person class for PropertyGrid and DataGrid demonstration.
/// </summary>
public class Person
{
    [DisplayName("First Name")]
    [Description("The person's first name")]
    public string FirstName { get; set; } = string.Empty;

    [DisplayName("Last Name")]
    [Description("The person's last name")]
    public string LastName { get; set; } = string.Empty;

    [DisplayName("Age")]
    [Description("The person's age in years")]
    public int Age { get; set; }

    [DisplayName("Email")]
    [Description("The person's email address")]
    public string Email { get; set; } = string.Empty;

    [DisplayName("Is Active")]
    [Description("Whether the person is currently active")]
    public bool IsActive { get; set; }
}
