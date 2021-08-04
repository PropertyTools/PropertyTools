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
        public CommandExample()
        {
            BasicCommand = new DelegateCommand(() => { this.ExecuteCount++; });
            Command = new DelegateCommand(() => { this.ExecuteCount++; }, () => CanExecute);
        }

        public ICommand BasicCommand { get; }

        public ICommand Command { get; }

        public bool CanExecute { get; set; }

        public int ExecuteCount { get; set; }
    }
}