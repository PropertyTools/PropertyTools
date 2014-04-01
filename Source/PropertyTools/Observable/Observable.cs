// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Observable.cs" company="PropertyTools">
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
//   Provides a base class implementing INotifyPropertyChanged.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides a base class implementing INotifyPropertyChanged.
    /// </summary>
    public abstract class Observable : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when the specified property has been changed.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        /// <param name="oldValue">
        /// The old value.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            this.RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Raises the property changed event for the property specified by a property expression.
        /// </summary>
        /// <typeparam name="TProperty">
        /// The type of the property.
        /// </typeparam>
        /// <param name="propertyExpression">
        /// The property expression.
        /// </param>
        protected void RaisePropertyChanged<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            this.RaisePropertyChanged(ExpressionUtilities.GetName(propertyExpression));
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the property.
        /// </typeparam>
        /// <param name="field">
        /// The field.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        /// <returns>
        /// True if the property was set.
        /// </returns>
        /// <remarks>
        /// This method uses the CallerMemberNameAttribute to determine the property name.
        /// </remarks>
#if NET45
        protected bool SetValue<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
#else
        protected bool SetValue<T>(ref T field, T value, string propertyName)
#endif
        {
            // ReSharper disable once RedundantNameQualifier
            if (object.Equals(field, value))
            {
                return false;
            }

            this.VerifyProperty(propertyName);

            //// this.OnPropertyChanging(propertyName, field, value);
            T oldValue = field;
            field = value;
            this.OnPropertyChanged(propertyName, oldValue, value);
            return true;
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the property.
        /// </typeparam>
        /// <param name="field">
        /// The field.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="propertyExpression">
        /// The property expression.
        /// </param>
        /// <returns>
        /// True if the property was set.
        /// </returns>
        /// <remarks>
        /// This method uses the CallerMemberNameAttribute to determine the property name.
        /// </remarks>
        protected bool SetValue<T>(ref T field, T value, Expression<Func<T>> propertyExpression)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            return this.SetValue(ref field, value, ExpressionUtilities.GetName(propertyExpression));
        }

        /// <summary>
        /// Verifies the property name.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        [Conditional("DEBUG")]
        private void VerifyProperty(string propertyName)
        {
            var type = this.GetType();

            // Look for a public property with the specified name.
            var propertyInfo = type.GetProperty(propertyName);

            Debug.Assert(propertyInfo != null, string.Format(CultureInfo.InvariantCulture, "{0} is not a property of {1}", propertyName, type.FullName));
        }
    }
}