// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationExampleWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ValidationExampleWindow.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for ValidationExampleWindow.
    /// </summary>
    public partial class ValidationExampleWindow
    {
        /// <summary>
        /// The static items source.
        /// </summary>
        /// <remarks>The static field makes it possible to use the same items source in multiple windows. This is great for testing change notifications!</remarks>
        private static ObservableCollection<ExampleObject> StaticItemsSource = new ObservableCollection<ExampleObject>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationExampleWindow" /> class.
        /// </summary>
        public ValidationExampleWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ExampleObject> ItemsSource => ValidationExampleWindow.StaticItemsSource;

        public class ExampleObject : ValidatableObject
        {
            /// <summary>
            /// The required string
            /// </summary>
            private string requiredString;

            /// <summary>
            /// The maximum length
            /// </summary>
            private string maxLength3;

            /// <summary>
            /// The percentage
            /// </summary>
            private int percentage;

            /// <summary>
            /// The minimum value
            /// </summary>
            private int minimum;

            /// <summary>
            /// The maximum value
            /// </summary>
            private int maximum;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExampleObject"/> class.
            /// </summary>
            public ExampleObject()
            {
                this.ValidateAllProperties();
            }

            /// <summary>
            /// Gets or sets the required string.
            /// </summary>
            /// <value>The required string.</value>
            [Required]
            public string RequiredString
            {
                get
                {
                    return this.requiredString;
                }

                set
                {
                    this.SetValue(ref this.requiredString, value, () => this.RequiredString);
                }
            }

            /// <summary>
            /// Gets or sets the maximum length.
            /// </summary>
            /// <value>The maximum length.</value>
            [MaxLength(3)]
            public string MaxLength3
            {
                get
                {
                    return this.maxLength3;
                }

                set
                {
                    this.SetValue(ref this.maxLength3, value, () => this.MaxLength3);
                }
            }

            /// <summary>
            /// Gets or sets the percentage.
            /// </summary>
            /// <value>The percentage.</value>
            [Range(0, 100)]
            public int Percentage
            {
                get
                {
                    return this.percentage;
                }

                set
                {
                    this.SetValue(ref this.percentage, value, () => this.Percentage);
                }
            }

            /// <summary>
            /// Gets or sets the minimum value.
            /// </summary>
            /// <value>The minimum.</value>
            public int Minimum
            {
                get
                {
                    return this.minimum;
                }

                set
                {
                    if (this.SetValue(ref this.minimum, value, () => this.Minimum))
                    {
                        // Also validate the dependent property
                        this.ValidateProperty("Maximum", this.Maximum);
                    }
                }
            }

            /// <summary>
            /// Gets or sets the maximum value.
            /// </summary>
            /// <value>The maximum.</value>
            [GreaterThan("Minimum")]
            public int Maximum
            {
                get
                {
                    return this.maximum;
                }

                set
                {
                    this.SetValue(ref this.maximum, value, () => this.Maximum);
                }
            }
        }

        /// <summary>
        /// Specifies that the value should be greater than the value of another property.
        /// </summary>
        /// <remarks>The value should be convertible to <see cref="double" />.</remarks>
        public class GreaterThanAttribute : ValidationAttribute
        {
            /// <summary>
            /// The property name
            /// </summary>
            private readonly string propertyName;

            /// <summary>
            /// Initializes a new instance of the <see cref="GreaterThanAttribute"/> class.
            /// </summary>
            /// <param name="propertyName">Name of the property.</param>
            public GreaterThanAttribute(string propertyName)
            {
                this.propertyName = propertyName;
            }

            /// <summary>
            /// Validates the specified value with respect to the current validation attribute.
            /// </summary>
            /// <param name="value">The value to validate.</param>
            /// <param name="validationContext">The context information about the validation operation.</param>
            /// <returns>An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.</returns>
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var otherValue = validationContext.ObjectType.GetProperty(this.propertyName).GetValue(validationContext.ObjectInstance, null);
                if (this.IsGreaterThan(value, otherValue))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult("The value should be greater than " + otherValue);
            }

            /// <summary>
            /// Determines whether <paramref name="value" /> is greater than <paramref name="otherValue" />.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="otherValue">The value to compare to.</param>
            /// <returns><c>true</c> if <paramref name="value" /> is greater than <paramref name="otherValue" />; otherwise, <c>false</c>.</returns>
            private bool IsGreaterThan(object value, object otherValue)
            {
                var convertedValue = Convert.ToDouble(value);
                var convertedOtherValue = Convert.ToDouble(otherValue);
                return convertedValue > convertedOtherValue;
            }
        }
    }
}