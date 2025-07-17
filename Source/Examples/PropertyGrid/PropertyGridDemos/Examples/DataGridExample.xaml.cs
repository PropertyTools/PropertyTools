// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for DataGridExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using PropertyTools;

    using System.Collections.ObjectModel;
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for DataGridExample.
    /// </summary>
    public partial class DataGridExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridExample" /> class.
        /// </summary>
        public DataGridExample()
        {
            this.InitializeComponent();
        }
    }

    public class DataGridExampleViewModel : Observable
    {
        static object StaticInstance = new DataGridExampleModel();

        public DataGridExampleViewModel()
        {
            this.SelectedObject = StaticInstance;
        }

        private object selectedObject;

        public object SelectedObject
        {
            get => this.selectedObject;
            internal set => this.SetValue(ref this.selectedObject, value);
        }
    }

    public class DataGridExampleModel : Observable
    {
        [PropertyTab("General")]
        [Category("Data"), Description("Collection of data.")]
        [DisplayName("Items")]
        public ObservableCollection<Data> Items { get; } = new ObservableCollection<Data>();

        public DataGridExampleModel()
        {
            for (int i = 1; i <= 40; i++) this.Items.Add(new Data() { Number = i, Text = "Row" + i.ToString() });
        }

        public class Data : Observable
        {
            private int number;

            public int Number
            {
                get => this.number;
                set => this.SetValue(ref this.number, value);
            }
 
            private string text;
            public string Text
            {
                get => this.text;
                set => this.SetValue(ref this.text, value);
            }
        }
    }
}