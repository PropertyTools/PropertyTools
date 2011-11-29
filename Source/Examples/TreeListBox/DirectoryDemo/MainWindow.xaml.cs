using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DirectoryDemo
{
    using System.IO;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<DirectoryViewModel> RootDirectories { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            tree1.ChildrenBinding = new Binding("SubDirectories");

            RootDirectories = new List<DirectoryViewModel>();
            foreach (var di in DriveInfo.GetDrives())
                RootDirectories.Add(new DirectoryViewModel(di.RootDirectory.FullName));
            DataContext = this;
        }
    }
}
