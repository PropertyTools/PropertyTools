namespace TestLibrary
{
    using System.Globalization;
    using System.Text.RegularExpressions;

    public static class UnitHelper
    {
        private static Regex unitAndValueExpression = new Regex(@"^\s*(?<value>[\d\.\,]+)*\s*(?<unit>.*)\s*$");

        public static bool TrySplitValueAndUnit(string s, out double value, out string unit)
        {
            s = s.Replace(',', '.').Trim();
            Match m = unitAndValueExpression.Match(s);
            if (!m.Success)
            {
                value = 0;
                unit = null;
                return false;
            }
            value = double.Parse(m.Groups["value"].Value, CultureInfo.InvariantCulture);
            unit = m.Groups["unit"].Value;
            return true;
        }


    }
}