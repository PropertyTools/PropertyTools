// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataErrorInfo.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
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

namespace ExampleLibrary
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Markup;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class TestDataErrorInfo : TestBase, IDataErrorInfo
    {
        [AutoUpdateText]
        [Description("Should not be empty.")]
        public string Name { get; set; }

        [AutoUpdateText]
        [Description("Should be larger or equal to zero.")]
        public int Age { get; set; }

        [DependsOn("Honey")]
        [Description("You cannot select both.")]
        public bool CondensedMilk { get; set; }

        [DependsOn("CondensedMilk")]
        [Description("You cannot select both.")]
        public bool Honey { get; set; }

        [ItemsSourceProperty("Countries")]
        [Description("Required field.")]
        public string Country { get; set; }

        [Browsable(false)]
        public IEnumerable<string> Countries
        {
            get
            {
                return new[] { "Norway", "Sweden", "Denmark", "Finland", string.Empty };
            }
        }

        [Browsable(false)]
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name": return string.IsNullOrEmpty(Name) ? "The name should be specified." : null;
                    case "Age":
                        {
                            if (Age > 130) return "The age is probably too large";
                            if (Age < 0) return "The age should not be less than 0.";
                            return null;
                        }
                    case "CondensedMilk":
                    case "Honey":
                        return this.CondensedMilk && this.Honey ? "You cannot have both condensed milk and honey!" : null;
                    case "Country": return string.IsNullOrEmpty(Country) ? "The country should be specified." : null;
                }

                return null;
            }
        }

        [Browsable(false)]
        string IDataErrorInfo.Error
        {
            get
            {
                return this["Name"] ?? this["Age"] ?? this["Honey"] ?? this["Country"];
            }
        }

        public TestDataErrorInfo()
        {
            this.Name = "Mike";
            this.Age = 3;
            this.Country = "Norway";
        }

        public override string ToString()
        {
            return "Validation with IDataErrorInfo";
        }
    }
}