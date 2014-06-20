namespace ExampleLibrary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class TestNotifyDataErrorInfo : TestBase, INotifyDataErrorInfo
    {
        private Dictionary<string, ValidationResult> errors = new Dictionary<string, ValidationResult>();

        private string name;

        public TestNotifyDataErrorInfo()
        {
            this.Name = "Anna";
        }

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

        public override string ToString()
        {
            return "Validation with INotifyDataErrorInfo";
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

            this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            if (this.errors.ContainsKey(propertyName))
            {
                yield return this.errors[propertyName];
            }
        }

        bool INotifyDataErrorInfo.HasErrors
        {
            get
            {
                return this.errors.Count > 0;
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        //string IDataErrorInfo.this[string columnName]
        //{
        //    get
        //    {
        //        ValidationResult error;
        //        return this.errors.TryGetValue(columnName, out error) ? error.ErrorMessage : null;
        //    }
        //}

        //string IDataErrorInfo.Error
        //{
        //    get
        //    {
        //        if (this.errors.Count > 0)
        //        {
        //            return this.errors.Count + " errors";
        //        }

        //        return null;
        //    }
        //}
    }
}