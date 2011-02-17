using System.Windows;

namespace CustomEditorDemo
{
    public partial class Window1 : Window
    {
        public Person CurrentPerson { get; set; }

        public Window1()
        {
            InitializeComponent();
            CurrentPerson = new Person();
            DataContext = this;
        }
    }

}
