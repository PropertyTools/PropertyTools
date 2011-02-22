using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SimpleGridDemo
{
    [TypeConverter(typeof(MassConverter))]
    public class Mass
    {
        public double Value { get; set; }

        public static Mass Parse(string s)
        {
            s = s.Replace(',', '.').Trim();
            var r = new Regex(@"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?");
            Match m = r.Match(s);
            if (!m.Success)
                return null;
            double value = double.Parse(m.Groups[0].Value, CultureInfo.InvariantCulture);
            // string unit = m.Groups[1].Value;
            return new Mass { Value = value };
        }

        public override string ToString()
        {
            return String.Format("{0:N0} kg", Value, CultureInfo.InvariantCulture);
        }
    }
}