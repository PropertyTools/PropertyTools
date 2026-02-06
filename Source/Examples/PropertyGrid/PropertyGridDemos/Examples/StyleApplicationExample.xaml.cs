// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StyleApplicationExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Demonstrates that implicit styles from Style.Resources work correctly with IDataErrorInfo (issue #455).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using PropertyTools;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    /// <summary>
    /// Interaction logic for StyleApplicationExample.
    /// </summary>
    public partial class StyleApplicationExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StyleApplicationExample" /> class.
        /// </summary>
        public StyleApplicationExample()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    /// ViewModel for StyleApplicationExample demonstrating issue #455.
    /// </summary>
    public class StyleApplicationExampleViewModel : Observable
    {
        private Person person1;
        private Person2 person2;

        public StyleApplicationExampleViewModel()
        {
            this.person1 = new Person { Name = "John", Age = 30 };
            this.person2 = new Person2 { Name = "Jane", Age = 25 };
        }

        public Person Person1
        {
            get => this.person1;
            set => this.SetValue(ref this.person1, value);
        }

        public Person2 Person2
        {
            get => this.person2;
            set => this.SetValue(ref this.person2, value);
        }
    }

    /// <summary>
    /// Simple Person class based on Observable (like BindableBase).
    /// </summary>
    public class Person : Observable
    {
        private string name;
        private int age;

        public string Name
        {
            get => this.name;
            set => this.SetValue(ref this.name, value);
        }

        public int Age
        {
            get => this.age;
            set => this.SetValue(ref this.age, value);
        }
    }

    /// <summary>
    /// Person2 class implementing IDataErrorInfo.
    /// This demonstrates the fix for issue #455 where implicit styles were not applied
    /// to controls when PropertyGrid was bound to objects implementing IDataErrorInfo.
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
