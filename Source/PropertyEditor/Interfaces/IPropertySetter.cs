using System.ComponentModel;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Set property interface.
    /// Used in PropertyEditor.PropertySetter
    /// </summary>
    public interface IPropertySetter
    {
        void SetProperty(object instance, PropertyDescriptor descriptor, object value);
    }
}