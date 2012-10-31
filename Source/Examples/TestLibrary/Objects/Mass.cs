// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mass.cs" company="PropertyTools">
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
    using System.ComponentModel;

    [TypeConverter(typeof(MassConverter))]
    public class Mass : Quantity<Mass>
    {
        static Mass()
        {
            RegisterUnit("kg", new Mass(1));
            RegisterUnit("g", new Mass(1e-3));
            RegisterUnit("mg", new Mass(1e-6));
            RegisterUnit("tonne", new Mass(1000)); // Metric
            RegisterUnit("longton", new Mass(1016.047)); // UK, new Mass( Imperial system
            RegisterUnit("shortton", new Mass(907.1847)); // US
            RegisterUnit("lb", new Mass(0.45359237));
        }

        public Mass()
        {

        }

        public Mass(double amount)
        {
            this.Amount = amount;
        }

        protected override string GetStandardUnit()
        {
            return "kg";
        }

        public static Mass Parse(string s)
        {
            return Parse<Mass>(s);
        }
    }
}