// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataAnnotationsExample.cs" company="PropertyTools">
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

    using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

    [PropertyGridExample]
    public class DataAnnotationsExample : Example, IDataErrorInfo
    {
        private string allowEmptyStrings;
        private string requiredString;
        private int angleInDegrees;
        private double angleInRadians;
        private string cityName;
        private string firstName;
        private string salesPerson;

        [Category("Required")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "A value is required.")]
        public string AllowEmptyStrings { get => this.allowEmptyStrings; set { this.allowEmptyStrings = value; this.RaisePropertyChanged(nameof(AllowEmptyStrings)); } }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A value is required.")]
        public string RequiredString { get => this.requiredString; set { this.requiredString = value; this.RaisePropertyChanged(nameof(RequiredString)); } }

        [Category("Range")]
        [Range(0, 360, ErrorMessage = "The angle must be in the interval [0,360].")]
        public int AngleInDegrees { get => this.angleInDegrees; set { this.angleInDegrees = value; this.RaisePropertyChanged(nameof(AngleInDegrees)); } }

        [Range(-Math.PI, Math.PI, ErrorMessage = "Angle must be in the interval [-pi,pi].")]
        public double AngleInRadians { get => this.angleInRadians; set { this.angleInRadians = value; this.RaisePropertyChanged(nameof(AngleInRadians)); } }

        [Category("StringLength")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Maximum length is 20. Minimum length is 1.")]
        public string CityName { get => this.cityName; set { this.cityName = value; this.RaisePropertyChanged(nameof(CityName)); } }

        [Category("RegularExpression")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{2,40}$", ErrorMessage = "Invalid name.")]
        public string FirstName { get => this.firstName; set { this.firstName = value; this.RaisePropertyChanged(nameof(FirstName)); } }

        [Category("CustomValidation")]
        [CustomValidation(typeof(AWValidation), "ValidateSalesPerson")]
        public string SalesPerson { get => this.salesPerson; set { this.salesPerson = value; this.RaisePropertyChanged(nameof(SalesPerson)); } }

        public DataAnnotationsExample()
        {
            this.AngleInDegrees = 360;
            this.AngleInRadians = Math.PI / 2;
            this.CityName = "Dublin";
            this.FirstName = "No numbers";
            this.AllowEmptyStrings = "";
            this.RequiredString = "Required value";
            this.SalesPerson = "Joe";
        }

        string IDataErrorInfo.Error => string.Empty;

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                var pi = this.GetType().GetProperty(columnName);
                var value = pi.GetValue(this, null);

                var context = new ValidationContext(this, null, null) { MemberName = columnName };
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateProperty(value, context, validationResults))
                {
                    var sb = new StringBuilder();
                    foreach (var vr in validationResults)
                    {
                        sb.AppendLine(vr.ErrorMessage);
                    }
                    return sb.ToString().Trim();
                }
                return null;
            }
        }
    }

    public class AWValidation
    {
        public static ValidationResult ValidateSalesPerson(string salesPerson)
        {
            var isValid = salesPerson.Contains("Joe");

            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(
                    "The selected sales person is not available for this customer.");
            }
        }

    }
}