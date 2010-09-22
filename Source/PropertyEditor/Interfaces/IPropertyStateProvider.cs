using System.ComponentModel;

namespace PropertyEditorLibrary
{
    public interface IPropertyStateProvider
    {
        bool IsEnabled(object component, PropertyDescriptor descriptor);
        bool IsVisible(object component, PropertyDescriptor descriptor);
        string GetError(object component, PropertyDescriptor descriptor);
        string GetWarning(object component, PropertyDescriptor descriptor);
    }
}