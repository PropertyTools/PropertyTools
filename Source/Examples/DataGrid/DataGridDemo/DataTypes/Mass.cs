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

namespace DataGridDemo
{
    using System;
    using System.Text.RegularExpressions;

    public struct Mass
    {
        public static Mass Kilogram = new Mass(1);
        public static Mass Gram = new Mass(1e-3);
        public static Mass Tonne = new Mass(1000);
        public static Mass Pound = new Mass(0.45359237);

        private double value;

        public Mass(double value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value + " kg";
        }

        public static Mass operator +(Mass x, Mass y)
        {
            return new Mass(x.value + y.value);
        }

        public static Mass operator -(Mass x, Mass y)
        {
            return new Mass(x.value - y.value);
        }

        public static Mass operator *(double x, Mass y)
        {
            return new Mass(x * y.value);
        }

        public static Mass operator *(Mass x, double y)
        {
            return new Mass(x.value * y);
        }

        public static Mass operator /(Mass x, double y)
        {
            return new Mass(x.value / y);
        }

        private static Regex parseExpression = new Regex(@"^\s*(?<value>[\d\.\,]+)*\s*(?<unit>.*)\s*$");

        public static Mass Parse(string s, IFormatProvider formatProvider)
        {
            Match m = parseExpression.Match(s);
            if (!m.Success)
            {
                throw new FormatException();
            }

            double value = double.Parse(m.Groups["value"].Value, formatProvider);
            string unit = m.Groups["unit"].Value;
            switch (unit)
            {
                case "tonne":
                case "t":
                    return value * Tonne;
                case "lb":
                    return value * Pound;
                case "g":
                    return value * Gram;
                case "kg":
                case "":
                    return value * Kilogram;
                default:
                    throw new FormatException();
            }
        }
    }
}