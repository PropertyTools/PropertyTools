// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.axaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia.Controls;
using System.ComponentModel;

namespace AvaloniaDemo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // Set up PropertyGrid example data
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
        Email = "john.doe@example.com"
    };
}

/// <summary>
/// Example person class for PropertyGrid demonstration.
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
}
