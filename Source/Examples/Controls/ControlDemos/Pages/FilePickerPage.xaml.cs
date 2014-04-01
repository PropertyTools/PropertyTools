// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePickerPage.xaml.cs" company="PropertyTools">
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

        private string basePath;

        public FilePickerViewModel()
        {
            this.FilePath = @"C:\autoexec.bat";
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