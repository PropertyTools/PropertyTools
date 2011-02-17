using System.ComponentModel;

namespace PropertyTools.Wpf
{
    public class FilePathPropertyViewModel : PropertyViewModel
    {
        public string Filter { get; set; }
        public string DefaultExtension { get; set; }

        public FilePathPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }
    }

    public class DirectoryPathPropertyViewModel : PropertyViewModel
    {
        public DirectoryPathPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }
    }
}