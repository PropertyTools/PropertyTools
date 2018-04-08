// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppBootstrapper.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace UndoRedoDemo
{
    using System.Windows;
    using Caliburn.Micro;

    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            this.Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<ShellViewModel>();
        }
    }
}