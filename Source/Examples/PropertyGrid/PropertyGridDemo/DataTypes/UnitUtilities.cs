// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitUtilities.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Text.RegularExpressions;

    public static class UnitUtilities
    {
        private static Regex unitAndValueExpression = new Regex(@"^\s*(?<value>[\d\.\,]+)*\s*(?<unit>.*)\s*$");

        public static bool TrySplitValueAndUnit(string s, IFormatProvider provider, out double value, out string unit)
        {
            s = s.Trim();
            var m = unitAndValueExpression.Match(s);
            if (!m.Success)
            {
                value = 0;
                unit = null;
                return false;
            }

            value = double.Parse(m.Groups["value"].Value, provider);
            unit = m.Groups["unit"].Value;
            return true;
        }
    }
}