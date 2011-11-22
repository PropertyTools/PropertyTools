namespace TestLibrary
{
    using System;
    using System.Collections.Generic;

    public class Quantity<T> : Quantity where T : Quantity, new()
    {
        public static T operator +(Quantity<T> mass1, T mass2)
        {
            return new T { Amount = mass1.Amount + mass2.Amount };
        }

        public static T operator -(Quantity<T> mass1, T mass2)
        {
            return new T { Amount = mass1.Amount - mass2.Amount };
        }

        public override string ToString()
        {
            return this.Amount + " " + this.GetStandardUnit();
        }

        protected virtual string GetStandardUnit()
        {
            return null;
        }

        static private Dictionary<string, Quantity> Units = new Dictionary<string, Quantity>();

        protected static void RegisterUnit(string unit, Quantity q)
        {
            Units.Add(unit, q);
        }

        protected static double GetUnitMultiplier(string unit, Type type)
        {
            Quantity q;
            if (unit != null && Units.TryGetValue(unit, out q))
            {
                if (q.GetType() == type)
                    return q.Amount;
            }
            throw new FormatException("Unknown unit.");
        }

        public static T Parse<T>(string s) where T : Quantity, new()
        {
            double value;
            string unit;
            if (!UnitHelper.TrySplitValueAndUnit(s, out value, out unit)) throw new FormatException("Invalid format.");
            if (!string.IsNullOrWhiteSpace(unit))
            {
                value *= GetUnitMultiplier(unit, typeof(T));
            }

            return new T { Amount = value };
        }
    }
}