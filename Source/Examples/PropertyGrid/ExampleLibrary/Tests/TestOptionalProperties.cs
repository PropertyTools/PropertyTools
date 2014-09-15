// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestOptionalProperties.cs" company="PropertyTools">
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
    using System;
    using System.Windows.Media;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class TestOptionalProperties : TestBase
    {
        [Category("By convention")]
        [Description("The UseName property specifies if Name is active or not.")]
        public string Name { get; set; }

        [Browsable(false)]
        public bool UseName { get; set; }

        [Description("The UseAge property specifies if Age is active or not.")]
        public int Age { get; set; }

        [Browsable(false)]
        public bool UseAge { get; set; }

        [Category("By OptionalAttribute")]
        [Optional("SpecifyWeight")]
        [Description("The SpecifyWeight property specifies if the Weight is active or not.")]
        public double Weight { get; set; }

        [Browsable(false)]
        public bool SpecifyWeight { get; set; }

        [Optional]
        [Description("The Nullable property is inactive if set to null.")]
        public int? Number { get; set; }

        [Optional]
        [Description("The property is inactive if set to null.")]
        public string String { get; set; }

        [Optional]
        [Description("The property is inactive if set to NaN.")]
        public double Double { get; set; }

        [Optional]
        [Description("The property is inactive if set to null.")]
        public Color? Color { get; set; }

        [Optional]
        [Description("The property is inactive if set to null.")]
        public DateTime? DateTime { get; set; }

        public override string ToString()
        {
            return "Optional properties";
        }
    }
}