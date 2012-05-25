using System.ComponentModel;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Properties marked [Slidable] are using a slider
    /// </summary>
    public class SlidableProperty : Property
    {
        public double SliderMaximum { get; set; }
        public double SliderMinimum { get; set; }
        public double SliderSmallChange { get; set; }
        public double SliderLargeChange { get; set; }

        public SlidableProperty(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }
    }
}