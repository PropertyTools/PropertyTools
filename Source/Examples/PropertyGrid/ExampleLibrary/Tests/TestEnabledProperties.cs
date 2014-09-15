// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestEnabledProperties.cs" company="PropertyTools">
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
    using System.ComponentModel;

    [PropertyGridExample]
    public class TestEnabledProperties : TestBase
    {
        [Category("Enable by convention")]
        [Description("The IsAgeEnabled property controls the enable state.")]
        public int Age { get; set; }

        public bool IsAgeEnabled { get; set; }

        [Description("The IsNameEnabled property controls the enable state. This property is also optional.")]
        [PropertyTools.DataAnnotations.Optional]
        public string Name { get; set; }

        public bool IsNameEnabled { get; set; }

        [Category("Enable by attribute")]
        [Description("Select green or blue to enable the string")]
        public TestEnumeration Color { get; set; }

        [PropertyTools.DataAnnotations.EnableBy("IsColorOk")]
        [Description("The IsColorOk property controls the enable state. The property should be enabled when the color is green or blue.")]
        public string EnableByIsColorOkProperty { get; set; }

        [PropertyTools.DataAnnotations.EnableBy("Color", TestEnumeration.Blue)]
        [Description("The control is enabled when Color = Blue.")]
        public string EnableByBlueColor { get; set; }

        [Browsable(false)]
        public bool IsColorOk
        {
            get
            {
                return this.Color == TestEnumeration.Green || this.Color == TestEnumeration.Blue;
            }
        }

        public override string ToString()
        {
            return "Enabled properties";
        }
    }
}