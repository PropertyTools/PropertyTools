// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePickerPage.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for FilePickerPage.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlDemos
{
    using System;
    using System.Windows.Controls;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for FilePickerPage.xaml
    /// </summary>
    public partial class FilePickerPage : Page
    {
        public FilePickerPage()
        {
            this.InitializeComponent();
            this.DataContext = new FilePickerViewModel();
        }
    }

    public class FilePickerViewModel : Observable
    {
        private string filePath;

        private string[] filePaths;

        private string basePath;

        public FilePickerViewModel()
        {
            this.FilePath = @"C:\autoexec.bat";
            this.filePaths = new string[]
            {
                @"C:\autoexec.bat",
                @"C:\autorun.bat"
            };
            this.BasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public string FilePath
        {
            get
            {
                return this.filePath;
            }

            set
            {
                this.SetValue(ref this.filePath, value, () => this.FilePath);
            }
        }

        public string[] FilePaths
        {
            get
            {
                return this.filePaths;
            }

            set
            {
                this.SetValue(ref this.filePaths, value, () => this.FilePaths);
            }
        }

        public string BasePath
        {
            get
            {
                return this.basePath;
            }

            set
            {
                this.SetValue(ref this.basePath, value, () => this.BasePath);
            }
        }
    }
}