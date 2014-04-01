// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WizardDialog.xaml.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents a wizard dialog.
// </summary>
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
        /// <summary>
        /// Identifies the <see cref="CurrentPage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
            "CurrentPage", typeof(int), typeof(WizardDialog), new UIPropertyMetadata(-1, CurrentPage_Changed));

        /// <summary>
        /// Identifies the <see cref="Pages"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PagesProperty = DependencyProperty.Register(
            "Pages", typeof(List<object>), typeof(WizardDialog), new UIPropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardDialog" /> class.
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

        /// <summary>
        /// The current page_ changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void CurrentPage_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var d = sender as WizardDialog;
            d.BindPage();
        }

        /// <summary>
        /// The back button_ click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
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
                this.PropertyGrid1.DataContext = null;
            }
            else
            {
                this.PropertyGrid1.DataContext = this.Pages[this.CurrentPage];
            }

            this.BackButton.IsEnabled = this.CurrentPage > 0;
            this.NextButton.IsEnabled = this.CurrentPage + 1 < this.Pages.Count;
            this.FinishButton.IsEnabled = this.CurrentPage == this.Pages.Count - 1;
        }

        /// <summary>
        /// The cancel button_ click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// The finish button_ click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// The next button_ click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentPage++;
        }

        /// <summary>
        /// The wizard dialog_ loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void WizardDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.CurrentPage = 0;
        }
    }
}