// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyDataErrorInfoExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class NotifyDataErrorInfoExample : Example, System.ComponentModel.INotifyDataErrorInfo
    {
        private Dictionary<string, ValidationResult> errors = new Dictionary<string, ValidationResult>();

        private string name;

        [AutoUpdateText]
        [Description("Should not be empty.")]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.Validate("Name", !string.IsNullOrEmpty(this.name), "Name should be specified");
            }
        }

        private void Validate(string propertyName, bool isValid, string message)
        {
            if (!isValid == this.errors.ContainsKey(propertyName))
            {
                return;
            }

            if (!isValid)
            {
                this.errors.Add(propertyName, new ValidationResult(message));
            }
            else
            {
                this.errors.Remove(propertyName);
            }

            this.ErrorsChanged(this, new System.ComponentModel.DataErrorsChangedEventArgs(propertyName));
        }

        IEnumerable System.ComponentModel.INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            if (this.errors.ContainsKey(propertyName))
            {
                yield return this.errors[propertyName];
            }
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