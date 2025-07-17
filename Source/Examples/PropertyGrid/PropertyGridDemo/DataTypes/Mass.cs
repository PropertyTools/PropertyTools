// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mass.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [TypeConverter(typeof(MassConverter))]
    public class Mass : IFormattable, IComparable
    {
        public Mass(double value)
        {
            this.Value = value;
        }

        public double Value { get; }

        public static Mass operator +(Mass left, Mass right)
        {
            return new Mass(left.Value + right.Value);
        }

        public static Mass operator -(Mass left, Mass right)
        {
            return new Mass(left.Value - right.Value);
        }

        public static Mass operator *(Mass left, double right)
        {
            return new Mass(left.Value * right);
        }

        public static Mass operator *(double left, Mass right)
        {
            return new Mass(left * right.Value);
        }

        public override string ToString()
        {
            return this.ToString("", CultureInfo.CurrentCulture);
        }

        public int CompareTo(object obj)
        {
            if (obj is Mass)
            {
                return this.Value.CompareTo(((Mass)obj).Value);
            }

            return 1;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.Value.ToString(format, formatProvider) + " kg";
        }

        public static Mass Parse(string s, IFormatProvider formatProvider)
        {
            double value;
            string unit;
            if (!UnitUtilities.TrySplitValueAndUnit(s, formatProvider, out value, out unit))
            {
                throw new FormatException("Invalid format.");
            }

            // note, unit is not handled...
            return new Mass(value);
        }
    }
}