// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumFilterAttributeExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for EnumFilterAttributeExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
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
        }
    }

    public class EnumFilterAttributeExampleViewModel : Observable
    {
        public EnumFilterAttributeExampleViewModel()
        {
            this.SelectedObject = new EnumFilterAttributeExampleModel();
        }

        private object selectedObject;

        public object SelectedObject
        {
            get => this.selectedObject;
            set => this.SetValue(ref this.selectedObject, value);
        }
    }

    public class EnumFilterAttributeExampleModel : Observable
    {
        public enum Season { Spring, Summer, Autumn, Winter }

        private Season allSeasons;
        private Season warmSeasons;
        private Season coldSeasons;
        private Season? nullableWarmSeasons;

        [System.ComponentModel.Category("RadioButtons (default)")]
        [System.ComponentModel.Description("All seasons (no filter)")]
        public Season AllSeasons
        {
            get => this.allSeasons;
            set => this.SetValue(ref this.allSeasons, value);
        }

        [System.ComponentModel.Category("RadioButtons (default)")]
        [System.ComponentModel.Description("Warm seasons only (Include: Spring, Summer)")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Include, Season.Spring, Season.Summer)]
        public Season WarmSeasons
        {
            get => this.warmSeasons;
            set => this.SetValue(ref this.warmSeasons, value);
        }

        [System.ComponentModel.Category("RadioButtons (default)")]
        [System.ComponentModel.Description("Cold seasons only (Exclude: Spring, Summer)")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Exclude, Season.Spring, Season.Summer)]
        public Season ColdSeasons
        {
            get => this.coldSeasons;
            set => this.SetValue(ref this.coldSeasons, value);
        }

        [System.ComponentModel.Category("ComboBox")]
        [System.ComponentModel.Description("Nullable warm seasons (Include: Spring, Summer)")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Include, Season.Spring, Season.Summer)]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Season? NullableWarmSeasons
        {
            get => this.nullableWarmSeasons;
            set => this.SetValue(ref this.nullableWarmSeasons, value);
        }
    }
}
