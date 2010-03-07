using System.Windows;

namespace SimpleDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(editor1.DataContext.ToString());
        }
    }
}
