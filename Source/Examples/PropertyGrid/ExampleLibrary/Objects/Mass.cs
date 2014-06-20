// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mass.cs" company="PropertyTools">
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
    using System.ComponentModel;
    using System.Globalization;

    [TypeConverter(typeof(MassConverter))]
    public class Mass : IFormattable, IComparable
    {
        private double value;

        public double Value
        {
            get
            {
                return this.value;
            }
        }

        public Mass(double value)
        {
            this.value = value;
        }

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
                return this.value.CompareTo(((Mass)obj).value);
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

            // TODO: handle unit
            return new Mass(value);
        }
    }
}