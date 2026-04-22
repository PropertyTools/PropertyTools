// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window1.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window1.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Media.Imaging;
using PropertyTools.Wpf;

namespace DialogDemos
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            DataContext = new Person { FirstName = "John", LastName = "Doe" };
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditObject_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new PropertyDialog { DataContext = DataContext };
            dlg.ShowDialog();
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            var options = new OptionsViewModel();
            var dlg = new PropertyDialog
            {
                Owner = this,
                DataContext = options,
                Title = "Options"
            };
            if (dlg.ShowDialog().Value)
                options.Save();
        }

        private void Wizard_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new WizardDialog();
            dlg.ShowDialog();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AboutDialog(this)
            {
                Title = "About the application",
                UpdateStatus = "The application is updated.",
                Image = new BitmapImage(new Uri(@"pack://application:,,,/DialogDemos;component/pt.png"))
            };
            dlg.ShowDialog();
        }

        private void DataErrorAware_Click(object sender, RoutedEventArgs e)
        {
            var options = new DataErrorAwareViewModel();
            var dlg = new PropertyDialog
            {
                Owner = this,
                OkButtonDataErrorAware = true,
                DataContext = options,
                Title = "Options"
            };

            dlg.ShowDialog();
        }

		private void ClosingEventExample1_Click(object sender, RoutedEventArgs e)
		{
			var model = new ClosingEventExampleViewModels();
			var dlg = new PropertyDialog
			{
				Owner = this,
				DataContext = model,
				Title = "OnClosing Event Example 1"
			};

			int closingAttempt = 0;

			dlg.Closing += (s, e) =>
			{
				closingAttempt++;
				e.Cancel = closingAttempt == 2;
			};

			dlg.Closed += (s, e) =>
			{
				MessageBox.Show("Dialog was closed. Total attempts= " + closingAttempt);
			};

			dlg.ShowDialog();
		}

		private void ClosingEventExample2_Click(object sender, RoutedEventArgs e)
		{
			var model = new ClosingEventExample2ViewModel();
			var dlg = new PropertyDialog
			{
				Owner = this,
				OkButtonDataErrorAware = true,
				DataContext = model,
				Title = "OnClosing Event Example 2"
			};

			int closingAttempt = 0;

			dlg.Closing += (s, e) =>
			{
				closingAttempt++;
				if (e is PropertyDialogCancelEventArgs customCancelEventArgs)
				{
					var editingContext = (ClosingEventExample2ViewModel)customCancelEventArgs.EditingContext;
					e.Cancel = editingContext.CancelOnClosingFlag;
				}
			};

			dlg.Closed += (s, e) =>
			{
				MessageBox.Show("Dialog was closed. Total attempts= " + closingAttempt);
			};

			dlg.ShowDialog();
		}

		private void ClosingEventExample3_Click(object sender, RoutedEventArgs e)
		{
			var existingUsername = "pt_user";
			var model = new ClosingEventExample3ViewModel()
			{
				Username = existingUsername
			};
			var dlg = new PropertyDialog
			{
				Owner = this,
				DataContext = model,
				Title = "Add New User"
			};

			dlg.Closing += (s, e) =>
			{
				// perform validation when OK button has been clicked.
				if (e is PropertyDialogCancelEventArgs customCancelEventArgs 
					&& customCancelEventArgs.DialogResult == true
					)
				{
					var editingContext = (ClosingEventExample3ViewModel)customCancelEventArgs.EditingContext;

					// username must be different that existing					
					if (editingContext.Username.Equals(existingUsername, StringComparison.OrdinalIgnoreCase))
					{
						MessageBox.Show("User with same name already exists. Please choose a different one", "Invalid Username", 
							MessageBoxButton.OK, MessageBoxImage.Information);
						e.Cancel = true;
					}
				}
			};

			dlg.ShowDialog();
		}
	}

	public enum StartupAction
    {
        NewProject,
        OpenProject,
        OpenLatest,
        Nothing
    }
}