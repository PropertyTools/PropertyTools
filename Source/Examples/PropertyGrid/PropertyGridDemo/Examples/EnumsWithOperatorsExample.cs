using ExampleLibrary;
using PropertyTools.DataAnnotations;

namespace PropertyGridDemo.Examples
{
    [PropertyGridExample]
    public class EnumsWithOperatorsExample : Example
    {
        public enum Fruit1 { Apple, Pear, Banana }

        // Example 1: FilteringMode.Exclude
        private Fruit1 fruit1Filter;
        private Fruit1 fruit1FilterC;
        private Fruit1 fruit1FilterL;

        [Category("RadioButtonList|")]
        [Description("Without 'Banana' ")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Exclude, Fruit1.Banana)]
        public Fruit1 Fruit1Filter { get => this.fruit1Filter; set { this.fruit1Filter = value; this.RaisePropertyChanged(nameof(Fruit1Filter)); } }

        [Category("ComboBox|")]
        [Description("Without 'Banana' ")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Exclude, Fruit1.Banana)]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit1 Fruit1FilterC { get => this.fruit1FilterC; set { this.fruit1FilterC = value; this.RaisePropertyChanged(nameof(Fruit1FilterC)); } }

        [Category("ListBox|")]
        [Description("Without 'Banana' ")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Exclude, Fruit1.Banana)]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit1 Fruit1FilterL { get => this.fruit1FilterL; set { this.fruit1FilterL = value; this.RaisePropertyChanged(nameof(Fruit1FilterL)); } }

        // Example 2: Nullable and FilteringMode.Include
        private Fruit1? nullableFruit1Filter;
        private Fruit1? nulllableFruit1FilterC;
        private Fruit1? nullableFruit1FilterL;

        [Category("RadioButtonList|")]
        [Description("Nullable 'Banana' only ")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Include, Fruit1.Banana)]
        public Fruit1? NullableFruit1Filter { get => this.nullableFruit1Filter; set { this.nullableFruit1Filter = value; this.RaisePropertyChanged(nameof(NullableFruit1Filter)); } }

        [Category("ComboBox|")]
        [Description("Nullable 'Banana' only ")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Include, Fruit1.Banana)]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit1? NulllableFruit1FilterC { get => this.nulllableFruit1FilterC; set { this.nulllableFruit1FilterC = value; this.RaisePropertyChanged(nameof(NulllableFruit1FilterC)); } }

        [Category("ListBox|")]
        [Description("Nullable 'Banana' only ")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Include, Fruit1.Banana)]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit1? NullableFruit1FilterL { get => this.nullableFruit1FilterL; set { this.nullableFruit1FilterL = value; this.RaisePropertyChanged(nameof(NullableFruit1FilterL)); } }

        // Example 3: translated enum values (see \Resources\Translations.resx)
        public enum Fruit2T { [Description("Apple")] Apple, [DisplayName("Pear")] Pear, [System.ComponentModel.DescriptionAttribute("Banana")] Banana }

        [Category("RadioButtonList|")]
        [Description("With translations (see CustomLocalizableOperator)")]        
        public Fruit2T? Fruit2 { get; set; }

        [Category("ComboBox|")]
        [Description("With translations (see CustomLocalizableOperator)")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit2T? Fruit2C { get; set; }

        [Category("ListBox|")]
        [Description("With translations (see CustomLocalizableOperator)")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit2T? Fruit2L { get; set; }
    }
}
