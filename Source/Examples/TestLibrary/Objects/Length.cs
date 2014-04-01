// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Length.cs" company="PropertyTools">
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