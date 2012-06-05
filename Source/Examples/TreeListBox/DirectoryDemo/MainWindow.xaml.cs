
namespace DirectoryDemo
{
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Data;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public List<DirectoryViewModel> RootDirectories { get; private set; }

        public MainWindow()
        {
            this.InitializeComponent();

            tree1.ChildrenBinding = new Binding("SubDirectories");

            this.RootDirectories = new List<DirectoryViewModel>();
            foreach (var di in DriveInfo.GetDrives())
            {
                this.RootDirectories.Add(new DirectoryViewModel(di.RootDirectory.FullName));
            }

            this.DataContext = this;
        }
    }
}
