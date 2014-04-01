// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestFormatStringAttribute.cs" company="PropertyTools">
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
    using System;
    using System.ComponentModel;

    using PropertyTools.DataAnnotations;

    public class TestFormatStringAttribute : TestBase
    {
        [Category("Double")]
        [FormatString("0.00")]
        public double Double { get; set; }

        [Category("Int")]
        [FormatString("000")]
        public int Integer { get; set; }

        [Category("TimeSpan")]
        [FormatString("hh:mm")]
        [Description("hh:mm")]
        public TimeSpan TimeSpan1 { get; set; }

        [FormatString("mm:ss")]
        [Description("mm:ss")]
        public TimeSpan TimeSpan2 { get; set; }

        [Category("DateTime")]
        [FormatString("yyyy-MM-dd hh:mm")]
        [Description("yyyy-MM-dd hh:mm")]
        public DateTime DateTime1 { get; set; }

        [FormatString("MM/dd/yyyy hh.mm.ss")]
        [Description("MM/dd/yyyy hh.mm.ss")]
        public DateTime DateTime2 { get; set; }

        [FormatString("yyyy-MM-dd")]
        public DateTime Date { get; set; }

        [FormatString("hh:MM")]
        public DateTime Time { get; set; }

        public TestFormatStringAttribute()
        {
            Double = Math.PI;
            Integer = 1;

            TimeSpan1 = new TimeSpan(0, 12, 39, 0);
            TimeSpan2 = new TimeSpan(0, 0, 12, 39);
            Date = Time = DateTime1 = DateTime2 = new DateTime(2012, 3, 5, 21, 34, 14);
        }

        public override string ToString()
        {
            return "FormatString attribute";
        }
    }
}