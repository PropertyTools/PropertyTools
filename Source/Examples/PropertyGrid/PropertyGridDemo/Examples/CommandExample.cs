// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Windows.Input;
    using PropertyTools.Wpf;

    [PropertyGridExample]
    public class CommandExample : Example
    {
        private bool canExecute;
        private int executeCount;

        public CommandExample()
        {
            BasicCommand = new DelegateCommand(() => { this.ExecuteCount++; });
            Command = new DelegateCommand(() => { this.ExecuteCount++; }, () => CanExecute);
        }

        public ICommand BasicCommand { get; }

        public ICommand Command { get; }

        public bool CanExecute { get => this.canExecute; set { this.canExecute = value; this.RaisePropertyChanged(nameof(CanExecute)); } }

        public int ExecuteCount { get => this.executeCount; set { this.executeCount = value; this.RaisePropertyChanged(nameof(ExecuteCount)); } }
    }
}