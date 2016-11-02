// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryPickerPage.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for DirectoryPickerPage.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlDemos
{
    using System;
    using System.Windows.Controls;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for DirectoryPickerPage.xaml
    /// </summary>
    public partial class DirectoryPickerPage : Page
    {
        public DirectoryPickerPage()
        {
            this.InitializeComponent();
            this.DataContext = new DirectoryPickerViewModel();
        }
    }

    public class DirectoryPickerViewModel : Observable
    {
        private string directory;

        private string basePath;

        public DirectoryPickerViewModel()
        {
            this.BasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            this.Directory = @"C:\Windows";
        }

        public string BasePath
        {
            get
            {
                return this.basePath;
            }

            set
            {
                this.SetValue(ref this.basePath, value);
            }
        }

        public string Directory
        {
            get
            {
                return this.directory;
            }

            set
            {
                this.SetValue(ref this.directory, value);
            }
        }
    }
}