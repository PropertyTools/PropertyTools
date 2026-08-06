// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumFilterAttributeExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for EnumFilterAttributeExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    using PropertyTools;
    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Interaction logic for EnumFilterAttributeExample.
    /// </summary>
    public partial class EnumFilterAttributeExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumFilterAttributeExample"/> class.
        /// </summary>
        public EnumFilterAttributeExample()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel();
        }

        public class ViewModel
        {
            public ViewModel()
            {
                this.Items.Add(new Item());
                this.Items.Add(new Item());
                this.Items.Add(new Item());
            }

            public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();
        }

        public enum Season { Spring, Summer, Autumn, Winter }

        public class Item : Observable
        {
            private Season allSeasons;
            private Season warmSeasons;
            private Season? coldSeasons;

            /// <summary>All seasons — no filter applied.</summary>
            public Season AllSeasons
            {
                get => this.allSeasons;
                set => this.SetValue(ref this.allSeasons, value);
            }

            /// <summary>Warm seasons only (Include: Spring, Summer).</summary>
            [EnumFilter(EnumFilterAttribute.FilteringMode.Include, Season.Spring, Season.Summer)]
            public Season WarmSeasons
            {
                get => this.warmSeasons;
                set => this.SetValue(ref this.warmSeasons, value);
            }

            /// <summary>Nullable cold seasons (Exclude: Spring, Summer).</summary>
            [EnumFilter(EnumFilterAttribute.FilteringMode.Exclude, Season.Spring, Season.Summer)]
            public Season? ColdSeasons
            {
                get => this.coldSeasons;
                set => this.SetValue(ref this.coldSeasons, value);
            }
        }
    }
}
