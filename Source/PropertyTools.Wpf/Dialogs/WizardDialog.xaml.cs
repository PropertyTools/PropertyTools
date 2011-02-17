using System.Collections.Generic;
using System.Windows;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Wizard dialog
    /// Todo: Win7 style
    /// </summary>
    public partial class WizardDialog : Window
    {
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof (int), typeof (WizardDialog),
                                        new UIPropertyMetadata(-1, CurrentPage_Changed));

        public static readonly DependencyProperty PagesProperty =
            DependencyProperty.Register("Pages", typeof (List<object>), typeof (WizardDialog),
                                        new UIPropertyMetadata(null));

        public WizardDialog()
        {
            InitializeComponent();
            Pages = new List<object>();

            Background = SystemColors.ControlBrush;

            NextButton.Click += NextButton_Click;
            BackButton.Click += BackButton_Click;
            FinishButton.Click += FinishButton_Click;
            CancelButton.Click += CancelButton_Click;
            Loaded += WizardDialog_Loaded;
        }

        public int CurrentPage
        {
            get { return (int) GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        public List<object> Pages
        {
            get { return (List<object>) GetValue(PagesProperty); }
            set { SetValue(PagesProperty, value); }
        }

        private static void CurrentPage_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var d = sender as WizardDialog;
            d.BindPage();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage++;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage--;
        }

        private void WizardDialog_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentPage = 0;
        }

        private void BindPage()
        {
            if (CurrentPage < 0 || CurrentPage >= Pages.Count)
                propertyControl1.DataContext = null;
            else
                propertyControl1.DataContext = Pages[CurrentPage];

            BackButton.IsEnabled = CurrentPage > 0;
            NextButton.IsEnabled = CurrentPage + 1 < Pages.Count;
            FinishButton.IsEnabled = CurrentPage == Pages.Count - 1;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}