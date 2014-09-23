// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mass.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
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