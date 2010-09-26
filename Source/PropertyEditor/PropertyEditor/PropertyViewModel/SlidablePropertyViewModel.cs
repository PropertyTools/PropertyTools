using System;
using System.ComponentModel;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Properties marked [Slidable] are using a slider
    /// </summary>
    public class SlidablePropertyViewModel : PropertyViewModel
    {
        public double SliderMaximum { get; set; }
        public double SliderMinimum { get; set; }
        public double SliderSmallChange { get; set; }
        public double SliderLargeChange { get; set; }

        public double DoubleValue
        {
            get
            {
                if (Value == null)
                    return 0;
                var t = Value.GetType();
                if (t==typeof(int))
                {
                    int i = (int)Value;
                    return i;
                }
                if (t==typeof(double))
                    return (double) Value;
                return 0;
            }
            set
            {                
                Value = value;
            }
        }

        public SlidablePropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }
    }
}