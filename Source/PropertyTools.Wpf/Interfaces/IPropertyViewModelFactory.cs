using System.ComponentModel;

namespace PropertyTools.Wpf
{
    public interface IPropertyViewModelFactory
    {
        /// <summary>
        /// Create a PropertyViewModel for the given property.
        /// The ViewModel could be populated with data from local attributes.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        PropertyViewModel CreateViewModel(object instance, PropertyDescriptor descriptor);
    }
}