using System;
using System.Windows;
using System.Windows.Controls;

namespace ControlsDemo.Pages
{
    /// <summary>
    /// Interaction logic for FormattingTextBoxPage.xaml
    /// </summary>
    public partial class FormattingTextBoxPage : Page
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof (double), typeof (MainWindow), new UIPropertyMetadata(Math.PI));

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof (DateTime), typeof (MainWindow),
                                        new UIPropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty StringProperty =
            DependencyProperty.Register("String", typeof (string), typeof (MainWindow), new UIPropertyMetadata("John"));

        public FormattingTextBoxPage()
        {
            InitializeComponent();
            DataContext = this;
        }


        public double Value
        {
            get { return (double) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public DateTime Time
        {
            get { return (DateTime) GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public string String
        {
            get { return (string) GetValue(StringProperty); }
            set { SetValue(StringProperty, value); }
        }
    }
}