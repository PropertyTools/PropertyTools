// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableTextBlockAndPropertyGridPage.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for EditableTextBlockAndPropertyGridPage.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlDemos
{
    using System.Windows.Input;

    using PropertyTools;
    using PropertyTools.Wpf;

    /// <summary>
    /// Interaction logic for EditableTextBlockAndPropertyGridPage.xaml
    /// </summary>
    public partial class EditableTextBlockAndPropertyGridPage
    {
        public EditableTextBlockAndPropertyGridPage()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel();
        }

        public class ViewModel : Observable
        {
            private string name;

            public ViewModel()
            {
                this.Name = "Model1";
            }

            public string Name
            {
                get
                {
                    return this.name;
                }

                set
                {
                    this.SetValue(ref this.name, value, nameof(this.Name));
                }
            }
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var etb = (EditableTextBlock)sender;
            etb.IsEditing = !etb.IsEditing;
        }
    }
}