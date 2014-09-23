// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateCommandBinding.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides a CommandBinding based on delegates.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Provides a CommandBinding based on delegates.
    /// </summary>
    public class DelegateCommandBinding : CommandBinding
    {
        /// <summary>
        /// The execute action.
        /// </summary>
        private readonly Action execute;

        /// <summary>
        /// The can execute function.
        /// </summary>
        private readonly Func<bool> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommandBinding" /> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="execute">The execute action.</param>
        /// <param name="canExecute">The can execute function.</param>
        public DelegateCommandBinding(ICommand command, Action execute, Func<bool> canExecute = null)
            : base(command)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.Executed += this.ExecuteDelegate;
            if (canExecute != null)
            {
                this.PreviewCanExecute += this.CanExecuteDelegate;
            }
        }

        /// <summary>
        /// Executes the delegate.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs" /> instance containing the event data.</param>
        private void ExecuteDelegate(object sender, ExecutedRoutedEventArgs e)
        {
            this.execute();
        }

        /// <summary>
        /// Determines whether this command can execute.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs" /> instance containing the event data.</param>
        private void CanExecuteDelegate(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.canExecute();
            e.Handled = true;
        }
    }
}