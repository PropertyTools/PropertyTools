using System.Windows;

namespace PropertyControlDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void FileExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
