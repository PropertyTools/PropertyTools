// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StyleApplicationExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Example demonstrating style application issue with IDataErrorInfo.
    /// Person1 (without IDataErrorInfo) should display with styled controls.
    /// Person2 (with IDataErrorInfo) should also display with styled controls.
    /// </summary>
    [PropertyGridExample]
    public class StyleApplicationExample : Example
    {
        private Person person1 = new Person { Name = "", Age = 0 };
        private Person2 person2 = new Person2 { Name = "", Age = 0 };

        [Category("Without IDataErrorInfo")]
        [Description("Person class without IDataErrorInfo - styles should apply")]
        public Person Person1
        {
            get => this.person1;
            set
            {
                this.person1 = value;
                this.RaisePropertyChanged(nameof(Person1));
            }
        }

        [Category("With IDataErrorInfo")]
        [Description("Person2 class with IDataErrorInfo - styles should also apply")]
        public Person2 Person2
        {
            get => this.person2;
            set
            {
                this.person2 = value;
                this.RaisePropertyChanged(nameof(Person2));
            }
        }
    }

    /// <summary>
    /// Simple Person class without IDataErrorInfo
    /// </summary>
    public class Person : INotifyPropertyChanged
    {
        private string name;
        private int age;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get => this.name;
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public int Age
        {
            get => this.age;
            set
            {
                if (this.age != value)
                {
                    this.age = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Age)));
                }
            }
        }
    }

    /// <summary>
    /// Person2 class with IDataErrorInfo implementation
    /// </summary>
    public class Person2 : Person, IDataErrorInfo
    {
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                var propertyInfo = this.GetType().GetProperty(columnName);
                if (propertyInfo == null) return null;
                var value = propertyInfo.GetValue(this, null);
                var context = new ValidationContext(this, null, null)
                {
                    MemberName = columnName
                };
                var validationResults = new List<ValidationResult>();

                if (Validator.TryValidateProperty(value, context, validationResults)) return null;
                var sb = new StringBuilder();
                foreach (var validationResult in validationResults)
                {
                    sb.AppendLine(validationResult.ErrorMessage);
                }

                return sb.ToString().Trim();
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(this, null, null);
                if (Validator.TryValidateObject(this, context, validationResults, true)) return null;
                var sb = new StringBuilder();
                foreach (var validationResult in validationResults)
                {
                    sb.AppendLine(validationResult.ErrorMessage);
                }

                return sb.ToString().Trim();
            }
        }
    }
}
