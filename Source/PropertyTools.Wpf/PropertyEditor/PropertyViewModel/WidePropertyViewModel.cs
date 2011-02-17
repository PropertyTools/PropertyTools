using System.ComponentModel;
using System.Windows;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Properties marked [WideProperty] are using the full width of the control
    /// </summary>
    public class WidePropertyViewModel : PropertyViewModel
    {
        public Visibility HeaderVisibility { get; private set; }

        public WidePropertyViewModel(object instance, PropertyDescriptor descriptor, bool showHeader, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
            HeaderVisibility = showHeader ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}