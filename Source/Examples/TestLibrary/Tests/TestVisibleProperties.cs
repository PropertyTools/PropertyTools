// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestVisibleProperties.cs" company="PropertyTools">
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

namespace TestLibrary
{
    using System.ComponentModel;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class TestVisibleProperties : TestBase
    {
        [Category("Visibility by convention")]
        public double Weight { get; set; }

        public bool IsWeightVisible { get; set; }

        [Category("Visibility by attribute")]
        [Description("Select green or blue to make the string visible")]
        public TestEnumeration Color2 { get; set; }

        [Browsable(false)]
        public bool IsColor2Ok { get { return this.Color2 == TestEnumeration.Green || this.Color2 == TestEnumeration.Blue; } }

        [VisibleBy("IsColor2Ok")]
        public string String2 { get; set; }

        [Category("Collapsing group")]
        [Description("This group is collapsing when String3 is not visible.")]
        [VisibleBy("IsColor2Ok")]
        public string String3 { get; set; }

        public TestVisibleProperties()
        {
            this.IsWeightVisible = true;
        }

        public override string ToString()
        {
            return "Visibility";
        }
    }
}