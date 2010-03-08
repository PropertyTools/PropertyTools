using System.ComponentModel;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// The PropertyAttributeProvider provides the attributes 
    /// Optional, FormatString, Height, SortOrder, Wide, Slidable
    /// 
    /// Defined in PropertyEditor.PropertyAttributeProvider
    /// </summary>
    public interface IPropertyAttributeProvider
    {
        PropertyAttributes GetAttributes(PropertyDescriptor descriptor);
    }

    public class PropertyAttributes
    {
        public string OptionalProperty { get; set; }
        public string FormatString { get; set; }
        public double Height { get; set; }
        public int SortOrder { get; set; }
        public bool IsWide { get; set; }
        public bool NoHeader { get; set; }
        public bool IsSlidable { get; set; }
        public double SliderMinimum { get; set; }
        public double SliderMaximum { get; set; }
        public double SliderLargeChange { get; set; }
        public double SliderSmallChange { get; set; }

        public PropertyAttributes()
        {
            Height = double.NaN;
        }
    }
}