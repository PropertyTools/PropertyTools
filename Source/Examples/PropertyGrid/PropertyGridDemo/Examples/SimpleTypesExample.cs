// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleTypesExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    [PropertyGridExample]
    public class SimpleTypesExample : Example
    {
        public bool Boolean { get; set; }
        public sbyte SignedByte { get; set; }
        public byte Byte { get; set; }
        public short Short { get; set; }
        public int Integer { get; set; }
        public long Long { get; set; }
        public ushort UnsignedShort { get; set; }
        public uint UnsignedInteger { get; set; }
        public ulong UnsignedLong { get; set; }
        public double Double { get; set; }
        public float Float { get; set; }
        public decimal Decimal { get; set; }
        public TestEnumeration Enum { get; set; }
        public string String { get; set; }
        public char Char { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }        
    }
}