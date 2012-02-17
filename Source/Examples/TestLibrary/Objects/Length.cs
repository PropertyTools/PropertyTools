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