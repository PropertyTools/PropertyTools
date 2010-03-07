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
        string GetOptionalProperty(PropertyDescriptor descriptor);
        string GetFormatString(PropertyDescriptor descriptor);
        double GetHeight(PropertyDescriptor descriptor);
        int GetSortOrder(PropertyDescriptor descriptor);
        bool IsWide(PropertyDescriptor descriptor, out bool noHeader);
        bool IsSlidable(PropertyDescriptor descriptor,
                        out double minimum,
                        out double maximum,
                        out double largeChange,
                        out double smallChange);
    }
}