// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Observable.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    ///   Provides a base class implementing INotifyPropertyChanged.
    /// </summary>
    public abstract class Observable : INotifyPropertyChanged
    {
        #region Public Events

        /// <summary>
        ///   Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

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
            PropertyChangedEventHandler handler = this.PropertyChanged;
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
            if (Equals(field, value))
            {
                return false;
            }

            this.VerifyProperty(propertyName);

            // this.OnPropertyChanging(propertyName, field, value);
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
            Type type = this.GetType();

            // Look for a public property with the specified name.
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            if (propertyInfo == null)
            {
                Debug.Fail(
                    string.Format(
                        CultureInfo.InvariantCulture, "{0} is not a property of {1}", propertyName, type.FullName));
            }
        }

        #endregion
    }
}