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
        private bool boolean;
        private sbyte signedByte;
        private byte @byte;
        private short @short;
        private int integer;
        private long @long;
        private ushort unsignedShort;
        private uint unsignedInteger;
        private ulong unsignedLong;
        private double @double;
        private float @float;
        private decimal @decimal;
        private TestEnumeration @enum;
        private string @string;
        private char @char;
        private DateTime dateTime;
        private TimeSpan timeSpan;

        public bool Boolean { get => this.boolean; set { this.boolean = value; this.RaisePropertyChanged(nameof(Boolean)); } }
        public sbyte SignedByte { get => this.signedByte; set { this.signedByte = value; this.RaisePropertyChanged(nameof(SignedByte)); } }
        public byte Byte { get => this.@byte; set { this.@byte = value; this.RaisePropertyChanged(nameof(Byte)); } }
        public short Short { get => this.@short; set { this.@short = value; this.RaisePropertyChanged(nameof(Short)); } }
        public int Integer { get => this.integer; set { this.integer = value; this.RaisePropertyChanged(nameof(Integer)); } }
        public long Long { get => this.@long; set { this.@long = value; this.RaisePropertyChanged(nameof(Long)); } }
        public ushort UnsignedShort { get => this.unsignedShort; set { this.unsignedShort = value; this.RaisePropertyChanged(nameof(UnsignedShort)); } }
        public uint UnsignedInteger { get => this.unsignedInteger; set { this.unsignedInteger = value; this.RaisePropertyChanged(nameof(UnsignedInteger)); } }
        public ulong UnsignedLong { get => this.unsignedLong; set { this.unsignedLong = value; this.RaisePropertyChanged(nameof(UnsignedLong)); } }
        public double Double { get => this.@double; set { this.@double = value; this.RaisePropertyChanged(nameof(Double)); } }
        public float Float { get => this.@float; set { this.@float = value; this.RaisePropertyChanged(nameof(Float)); } }
        public decimal Decimal { get => this.@decimal; set { this.@decimal = value; this.RaisePropertyChanged(nameof(Decimal)); } }
        public TestEnumeration Enum { get => this.@enum; set { this.@enum = value; this.RaisePropertyChanged(nameof(Enum)); } }
        public string String { get => this.@string; set { this.@string = value; this.RaisePropertyChanged(nameof(String)); } }
        public char Char { get => this.@char; set { this.@char = value; this.RaisePropertyChanged(nameof(Char)); } }
        public DateTime DateTime { get => this.dateTime; set { this.dateTime = value; this.RaisePropertyChanged(nameof(DateTime)); } }
        public TimeSpan TimeSpan { get => this.timeSpan; set { this.timeSpan = value; this.RaisePropertyChanged(nameof(TimeSpan)); } }        
    }
}