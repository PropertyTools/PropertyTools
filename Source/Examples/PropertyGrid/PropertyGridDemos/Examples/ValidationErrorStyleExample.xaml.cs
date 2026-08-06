// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationErrorStyleExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Demonstrates that ValidationErrorStyle can still be used with a custom ControlFactory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using PropertyTools;
    using PropertyTools.Wpf;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ValidationErrorStyleExample.
    /// </summary>
    public partial class ValidationErrorStyleExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationErrorStyleExample" /> class.
        /// </summary>
        public ValidationErrorStyleExample()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    /// ViewModel for ValidationErrorStyleExample.
    /// </summary>
    public class ValidationErrorStyleExampleViewModel : Observable
    {
        private PersonWithValidation person;

        public ValidationErrorStyleExampleViewModel()
        {
            this.person = new PersonWithValidation { Name = "John Doe", Age = 30 };
        }

        public PersonWithValidation Person
        {
            get => this.person;
            set => this.SetValue(ref this.person, value);
        }
    }

    /// <summary>
    /// Custom ControlFactory that applies ValidationErrorStyle.
    /// This demonstrates that ValidationErrorStyle functionality is still available
    /// for users who want to explicitly apply it via a custom factory.
    /// </summary>
    public class ValidationErrorStyleControlFactory : PropertyGridControlFactory
    {
        public Style ValidationErrorStyle { get; set; }

        public override System.Windows.FrameworkElement CreateControl(PropertyItem property, PropertyControlFactoryOptions options, object instance = null)
        {
            var control = base.CreateControl(property, options, instance);
            
            // Apply ValidationErrorStyle if specified and the control doesn't have a style
            if (control != null && this.ValidationErrorStyle != null && control.Style == null)
            {
                control.Style = this.ValidationErrorStyle;
            }
            
            return control;
        }
    }

    /// <summary>
    /// Person class with IDataErrorInfo validation.
    /// This demonstrates that ValidationErrorStyle still works - validation errors will
    /// appear in tooltips with custom styling (red border).
    /// </summary>
    public class PersonWithValidation : Observable, IDataErrorInfo
    {
        private string name;
        private int age;

        [Description("Enter a name (required)")]
        public string Name
        {
            get => this.name;
            set => this.SetValue(ref this.name, value);
        }

        [Description("Enter an age between 0 and 130")]
        public int Age
        {
            get => this.age;
            set => this.SetValue(ref this.age, value);
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrEmpty(this.Name))
                            return "Name is required";
                        if (this.Name.Length < 2)
                            return "Name must be at least 2 characters";
                        break;

                    case nameof(Age):
                        if (this.Age < 0)
                            return "Age cannot be negative";
                        if (this.Age > 130)
                            return "Age seems unrealistic (must be <= 130)";
                        break;
                }

                return null;
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                var dei = (IDataErrorInfo)this;
                var nameError = dei[nameof(Name)];
                var ageError = dei[nameof(Age)];
                
                if (!string.IsNullOrEmpty(nameError))
                    return nameError;
                if (!string.IsNullOrEmpty(ageError))
                    return ageError;
                    
                return null;
            }
        }
    }
}
