// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            var data = new TestDictionary();

            this.pgrid2.SelectedObject = new CollectionContainer { Items = data.Values1 };
        }

        #region nested classes
        // /////////////////////////////////////////////////////
        public class CollectionContainer
        {
//            [HeaderPlacement(HeaderPlacement.Collapsed)]    // don't show propertyName
            public ICollection Items { get; set; }
        }

        // /////////////////////////////////////////////////////

        public class TestDictionary
        {
            private Dictionary<int, Item> Dictionary { get; set; }
            public List<Item> Values1
            {
                get
                {
                    var result = new List<Item>();
                    foreach (var item in Dictionary.Values)
                    {
                        result.Add(item);
                    }

                    return result;
                }
            }

            public TestDictionary()
            {
                Dictionary = new Dictionary<int, Item>();

                Dictionary.Add(1, new Item() { Name = "Otto.1", Fraction = 5.1, Number = 21 });
                Dictionary.Add(2, new Item() { Name = "Otto.2", Fraction = 5.2, Number = 22, Test = Item.ETest.Otto });
            }

            public override string ToString()
            {
                return "Dictionary";
            }
        }

        // ////////////////////////////////////////////////////
        public class Item
        {
            public enum ETest
            {
                Hugo,
                Otto,
            }

            private string name;

            private int? number;

            private double? fraction;

            public Item()
            {
            }

            public string Name
            {
                get
                {
                    return this.name;
                }
                set
                {
#if Umbau
                this.SetValue(ref name, value, () => Name);
#else
                    this.name = value;
#endif
                }
            }

            public int? Number
            {
                get
                {
                    return this.number;
                }
                set
                {
#if Umbau
                this.SetValue(ref number, value, () => Number);
#else
                    this.number = value;
#endif
                }
            }

            public double? Fraction
            {
                get
                {
                    return this.fraction;
                }
                set
                {
#if Umbau
                this.SetValue(ref fraction, value, () => Fraction);
#else
                    this.fraction = value;
#endif
                }
            }

            public ETest? Test
            {
                get;
                set;
            }
        }


        #endregion

    }
}