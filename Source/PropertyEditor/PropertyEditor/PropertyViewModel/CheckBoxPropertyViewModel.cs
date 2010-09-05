using System.ComponentModel;

namespace PropertyEditorLibrary
{
    public class CheckBoxPropertyViewModel : PropertyViewModel
    {
        public CheckBoxPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }
    }
}