// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestReadOnlyProperties.cs" company="PropertyTools">
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
    [PropertyGridExample]
    public class TestReadOnlyAttribute : TestBase
    {
        [System.ComponentModel.Category("Private setter")]
        public string Property1 { get; private set; }

        public bool Check1 { get; private set; }

        [System.ComponentModel.Category("System.ComponentModel")]
        [System.ComponentModel.ReadOnly(true)]
        public string Property2 { get; set; }

        [System.ComponentModel.ReadOnly(true)]
        public bool Check2 { get; set; }

        [System.ComponentModel.Category("PropertyTools.DataAnnotations")]
        [PropertyTools.DataAnnotations.ReadOnly(true)]
        public string Property3 { get; set; }

        [PropertyTools.DataAnnotations.ReadOnly(true)]
        public bool Check3 { get; set; }

        public TestReadOnlyAttribute()
        {
            this.Check1 = this.Check2 = this.Check3 = true;
            this.Property1 = this.Property2 = this.Property3 = "Read only";
        }

        public override string ToString()
        {
            return "ReadOnlyAttribute";
        }
    }
}