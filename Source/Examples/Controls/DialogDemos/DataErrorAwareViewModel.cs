using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DialogDemos
{
    internal class DataErrorAwareViewModel : Observable, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ValidationResult> errors = [];
        private string name;

        [Category("Configuration|General")]
        [DisplayName("Name*")]
        [Required(AllowEmptyStrings = false)]
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.Validate(nameof(this.Name), !string.IsNullOrEmpty(this.name), "Name should be specified");
            }
        }

        public string Address { get; set; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        bool INotifyDataErrorInfo.HasErrors => errors.Count > 0;

        private void Validate(string propertyName, bool isValid, string message)
        {
            if (!isValid == errors.ContainsKey(propertyName))
            {
                return;
            }

            if (!isValid)
            {
                errors.Add(propertyName, new ValidationResult(message));
            }
            else
            {
                errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            if (propertyName != null && errors.ContainsKey(propertyName))
            {
                yield return errors[propertyName];
            }
        }

    }
}