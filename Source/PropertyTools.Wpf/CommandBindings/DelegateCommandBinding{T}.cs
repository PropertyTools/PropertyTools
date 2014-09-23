// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateCommandBinding{T}.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides a command binding that passes to command parameter to delegates.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Provides a command binding that passes to command parameter to delegates.
    /// </summary>
    /// <typeparam name="T">The type of the command parameter.</typeparam>
    public class DelegateCommandBinding<T> : CommandBinding
    {
        /// <summary>
        /// The execute action.
        /// </summary>
        private readonly Action<T> execute;

        /// <summary>
        /// The can execute function.
        /// </summary>
        private readonly Func<T, bool> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommandBinding{T}" /> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="execute">The execute action.</param>
        /// <param name="canExecute">The can execute function.</param>
        public DelegateCommandBinding(ICommand command, Action<T> execute, Func<T, bool> canExecute = null)
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
            this.execute((T)e.Parameter);
        }

        /// <summary>
        /// Determines whether this command can execute.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs" /> instance containing the event data.</param>
        private void CanExecuteDelegate(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.canExecute((T)e.Parameter);
            e.Handled = true;
        }
    }
}