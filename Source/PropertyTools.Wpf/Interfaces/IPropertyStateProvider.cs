using System.ComponentModel;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Return Enabled, Visible, Error and Warning states for a given component and property.
    /// </summary>
    public interface IPropertyStateProvider
    {
        bool IsEnabled(object component, PropertyDescriptor descriptor);
        bool IsVisible(object component, PropertyDescriptor descriptor);
        string GetError(object component, PropertyDescriptor descriptor);
        string GetWarning(object component, PropertyDescriptor descriptor);
    }
}