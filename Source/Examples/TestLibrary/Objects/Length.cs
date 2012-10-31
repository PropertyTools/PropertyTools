// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Length.cs" company="PropertyTools">
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

    [TypeConverter(typeof(LengthConverter))]
    public class Length : Quantity<Length>
    {
        static Length()
        {
            RegisterUnit("m", new Length(1));
            RegisterUnit("mm", new Length(1e-3));
            RegisterUnit("cm", new Length(1e-2));
            RegisterUnit("dm", new Length(1e-1));
            RegisterUnit("km", new Length(1e3));

            RegisterUnit("mile", new Length(1609.344)); // international
            RegisterUnit("usmile", new Length(1609.347)); // US survey
            RegisterUnit("nauticalmile", new Length(1852)); // nautical

            RegisterUnit("ft", new Length(0.3048));
            RegisterUnit("'", new Length(0.3048));
            RegisterUnit("yd", new Length(0.9144));
            RegisterUnit("in", new Length(0.0254));
            RegisterUnit("\"", new Length(0.0254));
        }

        public Length()
        {
        }

        public Length(double amount)
        {
            this.Amount = amount;
        }

        protected override string GetStandardUnit()
        {
            return "m";
        }

        public static Length operator +(Length mass1, Length mass2)
        {
            return new Length { Amount = mass1.Amount + mass2.Amount };
        }

        public static Length operator -(Length mass1, Length mass2)
        {
            return new Length { Amount = mass1.Amount - mass2.Amount };
        }

        public static Length Parse(string s)
        {
            return Parse<Length>(s);
        }
    }
}