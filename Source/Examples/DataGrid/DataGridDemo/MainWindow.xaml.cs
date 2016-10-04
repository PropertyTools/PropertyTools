// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for the main window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for the main window.
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            this.DataContext = this;
            var windowType = typeof(Window);
            this.Examples.AddRange(
                typeof(MainWindow).Assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Example") && windowType.IsAssignableFrom(t))
                .Select(t => new ExampleWindow(t))
                .OrderBy(e => e.Title));
        }

        public List<ExampleWindow> Examples { get; } = new List<ExampleWindow>();

        public class ExampleWindow
        {
            public ExampleWindow(Type type)
            {
                this.Type = type;
                var instance = this.CreateInstance();
                this.Title = instance.Title;
                instance.Close();
            }

            public string Title { get; }

            public Type Type { get; }

            public Window CreateInstance() => (Window)Activator.CreateInstance(this.Type);

            public void Show()
            {
                this.CreateInstance().Show();
            }

            public override string ToString()
            {
                return this.Title;
            }
        }

        private void OpenExample(object sender, MouseButtonEventArgs e)
        {
            var listBox = (ListBox)sender;
            var example = (ExampleWindow)listBox?.SelectedItem;
            example?.Show();
        }
    }
}