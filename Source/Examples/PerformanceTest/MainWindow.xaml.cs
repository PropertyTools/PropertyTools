using System.Windows;

namespace PerformanceTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Test1_Click(object sender, RoutedEventArgs e)
        {
            propertyEditor1.SelectedObject = new TestClass1();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            propertyEditor1.SelectedObject = null;
        }
    }
}