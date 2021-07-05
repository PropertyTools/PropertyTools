// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumsExample.cs" company="PropertyTools">
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
            BasicCommand = new DelegateCommand(() => { });
            Command = new DelegateCommand(() => { }, () => CanExecute);
        }

        public ICommand BasicCommand { get; }

        public ICommand Command { get; }

        public bool CanExecute { get; set; }    
    }
}