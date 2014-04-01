// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataAnnotations.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TestLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

    public class TestDataAnnotations : TestBase, IDataErrorInfo
    {
        //// http://msdn.microsoft.com/en-us/library/dd901590(v=vs.95).aspx

        [Category("Required")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "A value is required.")]
        public string AllowEmptyStrings { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A value is required.")]
        public string RequiredString { get; set; }

        [Category("Range")]
        [Range(0, 360, ErrorMessage = "The angle must be in the interval [0,360].")]
        public int AngleInDegrees { get; set; }

        [Range(-Math.PI, Math.PI, ErrorMessage = "Angle must be in the interval [-pi,pi].")]
        public double AngleInRadians { get; set; }

        [Category("StringLength")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Maximum length is 20. Minimum length is 1.")]
        public string CityName { get; set; }

        [Category("RegularExpression")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{2,40}$", ErrorMessage = "Invalid name.")]
        public string FirstName { get; set; }

        [Category("CustomValidation")]
        [CustomValidation(typeof(AWValidation), "ValidateSalesPerson")]
        public string SalesPerson { get; set; }

        public TestDataAnnotations()
        {
            AngleInDegrees = 360;
            AngleInRadians = Math.PI / 2;
            CityName = "Dublin";
            FirstName = "No numbers";
            AllowEmptyStrings = "";
            RequiredString = "Required value";
            SalesPerson = "Joe";
        }

        public override string ToString()
        {
            return "Data annotations";
        }

        string IDataErrorInfo.Error { get { return string.Empty; } }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                var pi = GetType().GetProperty(columnName);
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
        // http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.customvalidationattribute(v=vs.95).aspx
        public static ValidationResult ValidateSalesPerson(string salesPerson)
        {
            bool isValid;

            isValid = salesPerson.Contains("Joe");

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