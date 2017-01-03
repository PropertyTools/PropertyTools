// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpinnableAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class SpinnableAttributeExample : Example
    {
        [Spinnable(10, 90, 0, 360)]
        [Width(100)]
        public double Angle { get; set; }

        [Spinnable(10, 90, 0, 360)]
        [HorizontalAlignment(HorizontalAlignment.Right)]
        [FormatString("{0:0}°")]
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

        [Spinnable("1m", "2.5m", "0m", "100m")]
        [Width(100)]
        public Length? Length2 { get; set; }

        [Spinnable("1", "2.5", "0", "100")]
        [Width(100)]
        public double? NullableDouble { get; set; }

        [Spinnable(1, 10), Width(100)]
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

        public SpinnableAttributeExample()
        {
            this.Date = DateTime.Now;
            this.Length = new Length(25);
            this.Length2 = new Length(25);
            this.NullableDouble = 25;

            this.Info = "The spin control also supports mouse wheel, up/down/pgup/pgdn keys.";
        }
    }
}