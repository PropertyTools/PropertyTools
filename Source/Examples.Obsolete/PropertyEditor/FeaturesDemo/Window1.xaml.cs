// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window1.xaml.cs" company="PropertyTools">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
//   Interaction logic for Window1.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows;
using Ookii.Dialogs.Wpf;
using PropertyTools.Wpf;

namespace FeaturesDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public IList<Object> Objects { get; set; }

        public Window1()
        {
            InitializeComponent();
            DataContext = this;

            Objects = new List<Object>();
            Objects.Add(new Object() { Name = "Empty" });
            Objects.Add(new Object() { Name = "SimpleObject", Value = new SimpleObject() });
            Objects.Add(new Object() { Name = "ExampleObject", Value = new ExampleObject() });
            Objects.Add(new Object() { Name = "SuperExampleObject", Value = new SuperExampleObject() });

            // Use the Ookii folder browser dialog
            editor1.FolderBrowserDialogService = new OokiiDialogService();
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(editor1.SelectedObject.ToString());
        }
    }

    public class Object
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class OokiiDialogService : IFolderBrowserDialogService
    {
        public bool ShowFolderBrowserDialog(ref string directory, bool showNewFolderButton = true, string description = null, bool useDescriptionForTitle = true)
        {
            var dlg = new VistaFolderBrowserDialog();
            dlg.Description = description;
            dlg.UseDescriptionForTitle = useDescriptionForTitle;
            dlg.ShowNewFolderButton = showNewFolderButton;

            dlg.SelectedPath = directory;
            if (dlg.ShowDialog() == true)
            {
                directory = dlg.SelectedPath;
                return true;
            }
            return false;
        }
    }
}