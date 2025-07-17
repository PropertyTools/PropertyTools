// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Length.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    public struct Length : IFormattable, IComparable
    {
        public Length(double value)
        {
            this.Value = value;
        }

        public double Value { get; }

        public static Length operator +(Length left, Length right)
        {
            return new Length(left.Value + right.Value);
        }

        public static Length operator -(Length left, Length right)
        {
            return new Length(left.Value - right.Value);
        }

        public static Length operator *(Length left, double right)
        {
            return new Length(left.Value * right);
        }

        public static Length operator *(double left, Length right)
        {
            return new Length(left * right.Value);
        }

        public override string ToString()
        {
            return this.Value + " m";
        }

        public int CompareTo(object obj)
        {
            if (obj is Length)
            {
                return this.Value.CompareTo(((Length)obj).Value);
            }

            return 1;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.Value.ToString(format, formatProvider) + " m";
        }

        public static Length Parse(string s, IFormatProvider formatProvider)
        {
            double value;
            string unit;
            if (!UnitUtilities.TrySplitValueAndUnit(s, formatProvider, out value, out unit))
            {
                throw new FormatException("Invalid format.");
            }

            // note, unit is not handled...
            return new Length(value);
        }
    }
}