// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridFactoryExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for DataGridFactoryExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using PropertyTools;
    using PropertyTools.Wpf;

    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows;
    using PropertyTools.DataAnnotations;
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for DataGridFactoryExample.
    /// </summary>
    public partial class DataGridFactoryExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridExample" /> class.
        /// </summary>
        public DataGridFactoryExample()
        {
            this.InitializeComponent();
        }
    }

    public class DataGridFactoryExampleViewModel : Observable
    {
        static object StaticInstance = new DataGridFactoryExampleModel();

        public DataGridFactoryExampleViewModel()
        {
            this.SelectedObject = StaticInstance;
        }

        private object selectedObject;

        public object SelectedObject
        {
            get => this.selectedObject;
            internal set => this.SetValue(ref this.selectedObject, value);
        }

        public CustomControlFactory ControlFactory { get; } = new CustomControlFactory();
        public CustomOperator Operator { get; } = new CustomOperator();
    }

    public class CustomControlFactory : PropertyGridControlFactory
    {
        public override FrameworkElement CreateControl(PropertyItem pi, PropertyControlFactoryOptions options)
        {
            if (pi is BigCollectionPropertyItem)
            {
                return this.CreateBigCollectionControl(pi, options);
            }

            return base.CreateControl(pi, options);
        }

        protected virtual FrameworkElement CreateBigCollectionControl(PropertyItem property, PropertyControlFactoryOptions options)
        {
            var c = new System.Windows.Controls.DataGrid();
            foreach (var cd in property.Columns)
            {
                c.Columns.Add(new DataGridTextColumn() { Binding = new Binding(cd.PropertyName) });
            }

            c.SetBinding(System.Windows.Controls.DataGrid.ItemsSourceProperty, property.CreateBinding());
            return c;
        }
    }

    public class CustomOperator : PropertyGridOperator
    {
        protected override PropertyItem CreateCore(PropertyDescriptor pd, PropertyDescriptorCollection properties)
        {
            var ia = pd.GetFirstAttributeOrDefault<BigCollectionAttribute>();
            if (ia != null)
            {
                return new BigCollectionPropertyItem(pd, properties);
            }

            return base.CreateCore(pd, properties);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class BigCollectionAttribute : Attribute
    {
    }

    public class BigCollectionPropertyItem : PropertyItem
    {
        public BigCollectionPropertyItem(PropertyDescriptor pd, PropertyDescriptorCollection properties)
            : base(pd, properties)
        {
        }
    }

    public class DataGridFactoryExampleModel : Observable
    {
        [System.ComponentModel.Category("Data")]
        [System.ComponentModel.DisplayName("Items (small collection)")]
        [System.ComponentModel.Description("This is using the default implementation using the PropertyTools.DataGrid")]
        [Height(200)]
        public ObservableCollection<Data> Items1 { get; } = new ObservableCollection<Data>();
       
        [BigCollection] // custom attribute to direct the custom control factory to use the WPF DataGrid
        [Height(200)] // limit the height of the UI control
        [System.ComponentModel.DisplayName("Items (big collection)")]
        [System.ComponentModel.Description("This is implemented using the WPF DataGrid through the custom `ControlFactory`")]
        public ObservableCollection<Data> Items2 { get; } = new ObservableCollection<Data>();

        public DataGridFactoryExampleModel()
        {
            for (int i = 1; i <= 10; i++) this.Items1.Add(new Data() { Number = i, Text = "Row" + i.ToString() });
            for (int i = 1; i <= 1000000; i++) this.Items2.Add(new Data() { Number = i, Text = "Row" + i.ToString() });
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