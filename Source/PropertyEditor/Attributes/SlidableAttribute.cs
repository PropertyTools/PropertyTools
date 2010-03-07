using System;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// The [Slidable] attribute is used for slidable properties.
    /// Properties marked with [Slidable] will have a slider next to its editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class SlidableAttribute : Attribute
    {
        public static readonly SlidableAttribute Default = new SlidableAttribute();

        public SlidableAttribute()
        {
            Minimum = 0;
            Maximum = 100;
            SmallChange = 1;
            LargeChange = 10;
        }

        public SlidableAttribute(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
            SmallChange = 1;
            LargeChange = 10;
        }
        public SlidableAttribute(double minimum, double maximum, double smallChange, double largeChange)
        {
            Minimum = minimum;
            Maximum = maximum;
            SmallChange = smallChange;
            LargeChange = largeChange;
        }

        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public double LargeChange { get; set; }
        public double SmallChange { get; set; }

        public override bool Equals(object obj)
        {
            var o = obj as SlidableAttribute;
            return Minimum.Equals(o.Minimum) && Maximum.Equals(o.Maximum);
        }

        public override int GetHashCode()
        {
            return Minimum.GetHashCode() ^ Maximum.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return Equals(Default);
        }
    }
}
