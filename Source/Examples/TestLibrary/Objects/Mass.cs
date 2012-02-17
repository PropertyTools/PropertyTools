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