// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyDataErrorInfoSeverityExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using PropertyTools.DataAnnotations;
    using PropertyTools.Wpf;

    [PropertyGridExample]
    public class NotifyDataErrorInfoSeverityExample : Example, System.ComponentModel.INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<ValidationResultEx>> errors = new Dictionary<string, List<ValidationResultEx>>();

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
                this.Validate("FirstName", !string.IsNullOrEmpty(this.firstName), "First Name should be specified.",Severity.Error);
                this.Validate("FirstName", !string.IsNullOrEmpty(this.firstName), "First Name should not be numeric.", Severity.Warning);
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
                this.Validate("LastName", !string.IsNullOrEmpty(this.lastName), "Last Name should be specified.",Severity.Warning);
            }
        }

        private void Validate(string propertyName, bool isValid, string message, Severity severity = Severity.Error)
        {
            
            if (!isValid == this.errors.Any(k => k.Key == propertyName && k.Value.Any(v => v.ErrorMessage.ToLower() == message.ToLower())))
            {
                return;
            }

            if (!isValid)
            {                
                if(this.errors.ContainsKey(propertyName))
                {                                 
                    this.errors[propertyName].Add(new ValidationResultEx(message));
                }
                else
                {
                    this.errors.Add(propertyName, new List<ValidationResultEx> { new ValidationResultEx(message, severity) });                    
                }
            }
            else
            {
                this.errors.Remove(propertyName);
            }

            this.ErrorsChanged?.Invoke(this, new System.ComponentModel.DataErrorsChangedEventArgs(propertyName));
        }               

        IEnumerable System.ComponentModel.INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            var results = new List<ValidationResultEx>();
            if (this.errors.ContainsKey(propertyName))
            {                
                foreach (var r in this.errors[propertyName])
                {
                    results.Add(r);
                }
            }
            return results;
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