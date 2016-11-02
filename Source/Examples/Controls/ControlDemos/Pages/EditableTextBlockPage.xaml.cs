// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableTextBlockPage.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for EditableTextBlockPage.xaml
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
        private string text1;

        private string text2;

        public EditableTextBlockDemoViewModel()
        {
            this.Text1 = "Some text";
            this.Text2 = "TextWithoutSpaces";
        }

        public string Text1
        {
            get
            {
                return this.text1;
            }

            set
            {
                this.SetValue(ref this.text1, value);
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

                this.SetValue(ref this.text2, value);
            }
        }
    }
}