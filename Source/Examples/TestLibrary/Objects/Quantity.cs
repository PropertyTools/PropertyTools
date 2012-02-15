namespace TestLibrary
{
    using System;

    public abstract class Quantity : IComparable<Quantity>, IComparable
    {
        public double Amount { get; set; }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as Quantity);
        }

        public int CompareTo(Quantity other)
        {
            return other != null ? Amount.CompareTo(other.Amount) : 0;
        }

//        public abstract double GetUnitMultiplier(string unit);
    }
}