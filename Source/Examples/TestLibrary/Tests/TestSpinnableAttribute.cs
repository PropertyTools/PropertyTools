// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSpinnableAttribute.cs" company="PropertyTools">
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

    using PropertyTools.DataAnnotations;

    public class TestSpinnableAttribute : TestBase
    {
        [Spinnable(10, 90, 0, 360)]
        [Width(100)]
        public double Angle { get; set; }

        [Spinnable(10, 90, 0, 360)]
        [FormatString("{0:0}�")]
        [Width(100)]
        public double AngleWithFormatString { get; set; }

        [Spinnable]
        [FormatString("{0:yyyy-MM-dd}")]
        [Width(100)]
        public DateTime Date { get; set; }

        [Spinnable(SmallChange = 1.0 / 24 / 2, LargeChange = 1.0 / 24)]
        [FormatString("{0:HH:mm:ss}")]
        [Width(100)]
        public DateTime Time { get { return Date; } set { Date = value; } }

        [Spinnable(1, 10, 0, int.MaxValue)]
        [Width(100)]
        public int Days { get; set; }

        [Spinnable("1m", "2.5m", "0m", "100m")]
        [Width(100)]
        public Length Length { get; set; }

        [Spinnable(1,10), Width(100)]
        public byte Byte { get; set; }
        [Spinnable(1, 10), Width(100)]
        public sbyte SByte { get; set; }
        [Spinnable(1, 10), Width(100)]
        public short Short { get; set; }
        [Spinnable(1, 10), Width(100)]
        public short Int { get; set; }
        [Spinnable(1, 10), Width(100)]
        public short Long { get; set; }
        [Spinnable(1, 10), Width(100)]
        public ushort UShort { get; set; }
        [Spinnable(1, 10), Width(100)]
        public ushort ULong { get; set; }
        [Spinnable(1, 10), Width(100)]
        public uint UInt { get; set; }
        [Spinnable(1, 10), Width(100)]
        public float Float { get; set; }
        [Spinnable(1, 10), Width(100)]
        public double Double { get; set; }
        [Spinnable(1, 10), Width(100)]
        public decimal Decimal { get; set; }

        [Comment, Height(40)]
        public string Info { get; set; }

        public TestSpinnableAttribute()
        {
            Date = DateTime.Now;
            Length = new Length() { Amount = 25 };
            Info = "The spin control also supports mouse wheel, up/down/pgup/pgdn keys.";
        }

        public override string ToString()
        {
            return "Spinnable attribute";
        }
    }
}