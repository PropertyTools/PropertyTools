using System.ComponentModel;
using System.Windows.Controls;

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
            DataContext = new FilePickerViewModel();
        }
    }

    public class FilePickerViewModel : INotifyPropertyChanged
    {
        private string _directory;
        private string _filePath;

        public FilePickerViewModel()
        {
            FilePath = @"C:\autoexec.bat";
            Directory = @"C:\Windows";
        }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                RaisePropertyChanged("FilePath");
            }
        }

        public string Directory
        {
            get { return _directory; }
            set
            {
                _directory = value;
                RaisePropertyChanged("Directory");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected void RaisePropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}