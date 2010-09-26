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

namespace ControlsDemo
{
    /// <summary>
    /// Interaction logic for FilePickerPage.xaml
    /// </summary>
    public partial class FilePickerPage : Page
    {
        public FilePickerPage()
        {
            InitializeComponent();
            DataContext = this;
            FilePath = @"C:\autoexec.bat";
            Directory = @"C:\Windows";
        }

        public string FilePath { get; set; }
        public string Directory { get; set; }
    }
}
