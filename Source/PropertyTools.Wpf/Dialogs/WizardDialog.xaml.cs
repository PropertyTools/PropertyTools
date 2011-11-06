// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WizardDialog.xaml.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Represents a wizard dialog.
    /// </summary>
    public partial class WizardDialog : Window
    {
        #region Constants and Fields

        /// <summary>
        /// The current page property.
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
            "CurrentPage", typeof(int), typeof(WizardDialog), new UIPropertyMetadata(-1, CurrentPage_Changed));

        /// <summary>
        /// The pages property.
        /// </summary>
        public static readonly DependencyProperty PagesProperty = DependencyProperty.Register(
            "Pages", typeof(List<object>), typeof(WizardDialog), new UIPropertyMetadata(null));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardDialog"/> class.
        /// </summary>
        public WizardDialog()
        {
            this.InitializeComponent();
            this.Pages = new List<object>();

            this.Background = SystemColors.ControlBrush;

            this.NextButton.Click += this.NextButton_Click;
            this.BackButton.Click += this.BackButton_Click;
            this.FinishButton.Click += this.FinishButton_Click;
            this.CancelButton.Click += this.CancelButton_Click;
            this.Loaded += this.WizardDialog_Loaded;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets CurrentPage.
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return (int)this.GetValue(CurrentPageProperty);
            }

            set
            {
                this.SetValue(CurrentPageProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Pages.
        /// </summary>
        public List<object> Pages
        {
            get
            {
                return (List<object>)this.GetValue(PagesProperty);
            }

            set
            {
                this.SetValue(PagesProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The current page_ changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void CurrentPage_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var d = sender as WizardDialog;
            d.BindPage();
        }

        /// <summary>
        /// The back button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentPage--;
        }

        /// <summary>
        /// The bind page.
        /// </summary>
        private void BindPage()
        {
            if (this.CurrentPage < 0 || this.CurrentPage >= this.Pages.Count)
            {
                this.propertyControl1.DataContext = null;
            }
            else
            {
                this.propertyControl1.DataContext = this.Pages[this.CurrentPage];
            }

            this.BackButton.IsEnabled = this.CurrentPage > 0;
            this.NextButton.IsEnabled = this.CurrentPage + 1 < this.Pages.Count;
            this.FinishButton.IsEnabled = this.CurrentPage == this.Pages.Count - 1;
        }

        /// <summary>
        /// The cancel button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// The finish button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// The next button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentPage++;
        }

        /// <summary>
        /// The wizard dialog_ loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WizardDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.CurrentPage = 0;
        }

        #endregion
    }
}