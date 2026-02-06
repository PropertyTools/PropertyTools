// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace DemoLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Always create MainWindow with command line arguments
            var mainWindow = new MainWindow(e.Args);
            mainWindow.Show();
        }
    }
}
