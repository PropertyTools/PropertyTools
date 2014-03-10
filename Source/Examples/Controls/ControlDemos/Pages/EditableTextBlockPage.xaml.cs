// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableTextBlockPage.xaml.cs" company="PropertyTools">
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
//   Interaction logic for FilePickerPage.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlDemos
{
    using System;
    using System.Windows.Controls;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for EditableTextBlockPage.xaml
    /// </summary>
    public partial class EditableTextBlockPage : Page
    {
        public EditableTextBlockPage()
        {
            this.InitializeComponent();
            this.DataContext = new EditableTextBlockDemoViewModel();
        }
    }

    public class EditableTextBlockDemoViewModel : Observable
    {
        private bool isEditing1;

        private bool isEditing1c;

        private bool isEditing2;

        private string text1;

        private string text2;

        public EditableTextBlockDemoViewModel()
        {
            this.IsEditing1 = false;
            this.IsEditing2 = false;
            this.Text1 = "Some text";
            this.Text2 = "TextWithoutSpaces";
        }

        public bool IsEditing1
        {
            get
            {
                return this.isEditing1;
            }

            set
            {
                this.SetValue(ref this.isEditing1, value, () => this.IsEditing1);
            }
        }

        public bool IsEditing1c
        {
            get
            {
                return this.isEditing1c;
            }

            set
            {
                this.SetValue(ref this.isEditing1c, value, () => this.IsEditing1c);
            }
        }

        public bool IsEditing2
        {
            get
            {
                return this.isEditing2;
            }

            set
            {
                this.SetValue(ref this.isEditing2, value, () => this.IsEditing2);
            }
        }

        public string Text1
        {
            get
            {
                return this.text1;
            }

            set
            {
                this.SetValue(ref this.text1, value, () => this.Text1);
            }
        }

        public string Text2
        {
            get
            {
                return this.text2;
            }

            set
            {
                if (value.Contains(" "))
                {
                    throw new Exception("Spaces not allowed.");
                }

                this.SetValue(ref this.text2, value, () => this.Text2);
            }
        }
    }
}