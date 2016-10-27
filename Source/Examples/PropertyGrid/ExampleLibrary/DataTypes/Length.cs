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
        private double value;

        public double Value
        {
            get
            {
                return this.value;
            }
        }

        public Length(double value)
        {
            this.value = value;
        }

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
                return this.value.CompareTo(((Length)obj).value);
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

            // TODO: handle unit
            return new Length(value);
        }
    }
}