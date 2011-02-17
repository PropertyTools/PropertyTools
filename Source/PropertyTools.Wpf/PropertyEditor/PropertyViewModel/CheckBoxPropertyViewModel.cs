using System.ComponentModel;

namespace PropertyTools.Wpf
{
    public class CheckBoxPropertyViewModel : PropertyViewModel
    {
        public CheckBoxPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }
    }
}