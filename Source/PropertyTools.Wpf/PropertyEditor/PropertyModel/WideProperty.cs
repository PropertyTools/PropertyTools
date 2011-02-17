using System.ComponentModel;
using System.Windows;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Properties marked [WideProperty] are using the full width of the control
    /// </summary>
    public class WideProperty : Property
    {
        public Visibility HeaderVisibility { get; private set; }

        public WideProperty(object instance, PropertyDescriptor descriptor, bool noHeader, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
            HeaderVisibility = noHeader ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}