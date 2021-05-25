// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyDataErrorInfoSeverityExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;
    using PropertyGridDemo;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    [PropertyGridExample]
    public class NotifyDataErrorInfoSeverityExample : Example, System.ComponentModel.INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ValidationResultEx> errors = new Dictionary<string, ValidationResultEx>();

        private string firstName;

        private string lastName;

        public NotifyDataErrorInfoSeverityExample()
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
        }

        [AutoUpdateText]
        [Description("Should not be empty.")]
        public string FirstName
        {
            get
            {
                return this.firstName;
            }

            set
            {
                this.firstName = value;
                this.Validate("FirstName", !string.IsNullOrEmpty(this.firstName), "First Name should be specified.", Severity.Error);
            }
        }

        [AutoUpdateText]
        [Description("Should not be empty.")]
        public string LastName
        {
            get
            {
                return this.lastName;
            }

            set
            {
                this.lastName = value;
                this.Validate("LastName", !string.IsNullOrEmpty(this.lastName), "Last Name should be specified.", Severity.Warning);
            }
        }

        private void Validate(string propertyName, bool isValid, string message, Severity severity = Severity.Error)
        {
            if (!isValid == this.errors.ContainsKey(propertyName))
            {
                return;
            }

            if (!isValid)
            {
                this.errors.Add(propertyName, new ValidationResultEx(message, severity));
            }
            else if (!string.IsNullOrEmpty(this.FirstName) && propertyName == nameof(this.FirstName) && !System.Text.RegularExpressions.Regex.IsMatch(this.FirstName, "^[a-zA-Z]+$"))
            {
                this.errors.Remove(propertyName);
                this.errors.Add(propertyName, new ValidationResultEx("It should be string.", Severity.Warning));
            }
            else
            {
                this.errors.Remove(propertyName);
            }

            this.ErrorsChanged?.Invoke(this, new System.ComponentModel.DataErrorsChangedEventArgs(propertyName));
        }               

        IEnumerable System.ComponentModel.INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            if (this.errors.ContainsKey(propertyName))
            {
                yield return this.errors[propertyName];
            }
            yield return null;
        }

        bool System.ComponentModel.INotifyDataErrorInfo.HasErrors
        {
            get
            {
                return this.errors.Count > 0;
            }
        }

        public event EventHandler<System.ComponentModel.DataErrorsChangedEventArgs> ErrorsChanged;
    }
}