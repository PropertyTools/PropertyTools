using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DialogDemos
{
    internal class DataErrorAwareViewModel : INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ValidationResult> m_errors = new Dictionary<string, ValidationResult>();
        private          string                               m_name;

        [Category("Configuration|General")]
        [Required(AllowEmptyStrings = false)]
        public string Name
        {
            get => m_name;
            set
            {
                m_name = value;
                validate("Name", !string.IsNullOrEmpty(m_name), "Name should be specified");
            }
        }

        private void validate(string propertyName, bool isValid, string message)
        {
            if (!isValid == m_errors.ContainsKey(propertyName))
            {
                return;
            }

            if (!isValid)
            {
                m_errors.Add(propertyName, new ValidationResult(message));
            }
            else
            {
                m_errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            if (m_errors.ContainsKey(propertyName))
            {
                yield return m_errors[propertyName];
            }
        }

        bool INotifyDataErrorInfo.HasErrors => m_errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}