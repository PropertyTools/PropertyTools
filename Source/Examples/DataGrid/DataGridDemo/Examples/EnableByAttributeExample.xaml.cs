// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnableByAttributeExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for EnableByAttributeExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    using PropertyTools;
    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Interaction logic for EnableByAttributeExample.xaml
    /// </summary>
    public partial class EnableByAttributeExample
    {
        /// <summary>
        /// The static items source.
        /// </summary>
        /// <remarks>The static field makes it possible to use the same items source in multiple windows. 
        /// This is great for testing change notifications!</remarks>
        private static readonly ObservableCollection<ExampleObject> StaticItemsSource =
            new ObservableCollection<ExampleObject>
                {
                    new ExampleObject { City = "Oslo", IsCapital = true, IsRowEnabled = true },
                    new ExampleObject { City = "Bergen", IsRowEnabled = true },
                    new ExampleObject { City = "Stockholm", IsCapital = true },
                    new ExampleObject { City = "Reykjavik", IsCapital = true },
                    new ExampleObject { City = "Tokyo", IsCapital = true, Size = CitySize.Metropolis },
                };

        /// <summary>
        /// Initializes a new instance of the <see cref="EnableByAttributeExample" /> class.
        /// </summary>
        public EnableByAttributeExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ExampleObject> ItemsSource => StaticItemsSource;

        public class ExampleObject : Observable
        {
            private bool isRowEnabled;

            private string city;

            private bool isCapital;

            private CitySize size;

            private string metropolInfo;

            public bool IsRowEnabled
            {
                get
                {
                    return this.isRowEnabled;
                }
                set
                {
                    this.SetValue(ref this.isRowEnabled, value, nameof(this.IsRowEnabled));
                }
            }

            [EnableBy("IsRowEnabled")]
            public bool IsCapital
            {
                get
                {
                    return this.isCapital;
                }
                set
                {
                    this.SetValue(ref this.isCapital, value, nameof(this.IsCapital));
                }
            }

            [EnableBy("IsRowEnabled")]
            public string City
            {
                get
                {
                    return this.city;
                }
                set
                {
                    this.SetValue(ref this.city, value, nameof(this.City));
                }
            }

            [EnableBy("IsRowEnabled")]
            public CitySize Size
            {
                get
                {
                    return this.size;
                }
                set
                {
                    this.SetValue(ref this.size, value, nameof(this.Size));
                }
            }

            [EnableBy("Size", CitySize.Metropolis)]
            public string MetropolInfo
            {
                get
                {
                    return this.metropolInfo;
                }
                set
                {
                    this.SetValue(ref this.metropolInfo, value, nameof(this.MetropolInfo));
                }
            }
        }
    }
}