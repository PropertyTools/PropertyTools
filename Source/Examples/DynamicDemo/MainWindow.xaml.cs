using System.Dynamic;
using System.Windows;

namespace DynamicDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Create a dynamic ExpandoObject
            dynamic person = new ExpandoObject();
            person.Name = "Peter";
            person.Age = 26;
            person.Weight = 62.5d;
            person.IsMarried = false;
          
            // todo: do we need to create a custom TypeDescriptorProvider?

            editor1.SelectedObject = person;
        }
    }
}
