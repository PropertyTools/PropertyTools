using System.ComponentModel;
using PropertyEditorLibrary;

namespace ViewModelDemo
{
    /// <summary>
    /// Providing default property states, using IDataErrorInfo for property error message
    /// </summary>
    public class PropertyStateProvider : IPropertyStateProvider
    {
        public bool IsEnabled(object component, PropertyDescriptor descriptor)
        {
            return true;
        }

        public bool IsVisible(object component, PropertyDescriptor descriptor)
        {
            return true;
        }

        public string GetError(object component, PropertyDescriptor descriptor)
        {
            var dei = component as IDataErrorInfo;
            if (dei != null)
                return dei[descriptor.Name];
            return null;
        }

        public string GetWarning(object component, PropertyDescriptor descriptor)
        {
            return null; //
        }
    }
}