// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidatableObject.cs" company="PropertyTools">
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
//   Provides an implementation of IDataErrorInfo where the errors are automatically updated when properties are changed.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    /// <summary>
    /// Provides an implementation of <see cref="IDataErrorInfo" /> where the errors are automatically updated when properties are changed.
    /// </summary>
    public abstract class ValidatableObject : Observable, IDataErrorInfo
    {
        /// <summary>
        /// The errors dictionary.
        /// </summary>
        private readonly Dictionary<string, List<ValidationResult>> errors = new Dictionary<string, List<ValidationResult>>();

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Gets a value indicating whether the entity has validation errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors
        {
            get
            {
                return this.errors.Count > 0;
            }
        }

        /// <summary>
        /// Gets the error messages.
        /// </summary>
        /// <value>The error messages.</value>
        public string ErrorMessages
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var error in this.errors)
                {
                    foreach (var result in error.Value)
                    {
                        sb.AppendLine(error.Key + ": " + result.ErrorMessage);
                    }
                }

                return sb.ToString().Trim();
            }
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; or null or <see cref="F:System.String.Empty" />, to retrieve entity-level errors.</param>
        /// <returns>The validation errors for the property or entity.</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            List<ValidationResult> propertyErrors;
            if (this.errors.TryGetValue(propertyName, out propertyErrors))
            {
                return propertyErrors;
            }

            return new ValidationResult[0];
        }

        /// <summary>
        /// Called when the specified property has been changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);
            this.ValidateProperty(propertyName, newValue);
        }

        /// <summary>
        /// Called when the errors have changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnErrorsChanged(string propertyName)
        {
            var handler = this.ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Validates all properties.
        /// </summary>
        protected void ValidateAllProperties()
        {
            foreach (var property in this.GetType().GetProperties())
            {
                this.ValidateProperty(property.Name, property.GetValue(this, null));
            }
        }

        /// <summary>
        /// Validates the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        protected void ValidateProperty(string propertyName, object value)
        {
            var hadErrors = this.HasErrors;
            List<ValidationResult> oldErrors;
            var propertyHadErrors = this.errors.TryGetValue(propertyName, out oldErrors) && oldErrors.Count > 0;

            var validationContext = new ValidationContext(this, null, null) { MemberName = propertyName };
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(value, validationContext, validationResults);

            var propertyHasErrors = validationResults.Count > 0;
            if (validationResults.Count > 0)
            {
                this.errors[propertyName] = validationResults;
            }
            else
            {
                this.errors.Remove(propertyName);
            }

            if (propertyHadErrors != propertyHasErrors)
            {
                this.OnErrorsChanged(propertyName);
            }

            // The actual validation errors may change every time the property is changed 
            // (e.g. if the property depends on another property)
            this.RaisePropertyChanged(() => this.ErrorMessages);

            if (hadErrors != this.HasErrors)
            {
                this.RaisePropertyChanged(() => this.HasErrors);
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>System.String.</returns>
        public string this[string columnName]
        {
            get
            {
                var sb = new StringBuilder();
                foreach (ValidationResult r in this.GetErrors(columnName))
                {
                    sb.AppendLine(r.ErrorMessage);
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <value>The error.</value>
        public string Error
        {
            get
            {
                return this.ErrorMessages;
            }
        }
    }
}