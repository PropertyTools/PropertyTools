// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateCommandBinding{T}.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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