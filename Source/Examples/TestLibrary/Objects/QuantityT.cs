// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuantityT.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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

        public static T2 Parse<T2>(string s) where T2 : Quantity, new()
        {
            double value;
            string unit;
            if (!UnitHelper.TrySplitValueAndUnit(s, out value, out unit)) throw new FormatException("Invalid format.");
            if (!string.IsNullOrWhiteSpace(unit))
            {
                value *= GetUnitMultiplier(unit, typeof(T));
            }

            return new T2 { Amount = value };
        }
    }
}