using System.ComponentModel;
using System.Windows.Input;

namespace PropertyTools.Wpf
{
    public class FilePathPropertyViewModel : PropertyViewModel
    {
        public string Filter { get; set; }
        public string DefaultExtension { get; set; }
        public bool UseOpenDialog { get; set; }
        public ICommand BrowseCommand { get; set; }
        public IFileDialogService FileDialogService { get { return Owner.FileDialogService; } }

        public FilePathPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }
    }

    public class DirectoryPathPropertyViewModel : PropertyViewModel
    {
        public ICommand BrowseCommand { get; set; }
        public IFolderBrowserDialogService FolderBrowserDialogService { get { return Owner.FolderBrowserDialogService; } }
        public DirectoryPathPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }
    }

    public class PasswordPropertyViewModel : PropertyViewModel
    {
        // public char PasswordChar { get; set; }

        public PasswordPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            :base(instance,descriptor,owner)
        {            
        }
    }
}