// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumsExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using PropertyTools.DataAnnotations;

    public enum Fruit1 { Apple, Pear, Banana }

    public enum Fruit2 { }

    public enum Fruit3 {[Browsable(false)] Apple, Pear, Banana }

    public enum Fruit4 {[Browsable(false)] Apple, [Browsable(false)] Pear, [Browsable(false)] Banana }

    public enum Fruit5 {[Description("Eple")] Apple, [Description("Pære")] Pear, [Description("Banan")] Banana }

    [Flags]
    public enum Fruit6 { All = Apple | Pear | Banana, Apple = 1, Pear = 2, Banana = 4 }

    [PropertyGridExample]
    public class EnumsExample : Example
    {
        private Fruit1 fruit1;
        private Fruit2 fruit2;
        private Fruit3 fruit3;
        private Fruit4 fruit4;
        private Fruit5 fruit5;
        private Fruit6 fruit6;
        private Fruit1? nullableFruit1;
        private Fruit3 radioButtons3;
        private Fruit1 fruit1C;
        private Fruit2 fruit2C;
        private Fruit3 fruit3C;
        private Fruit4 fruit4C;
        private Fruit5 fruit5C;
        private Fruit6 fruit6C;
        private Fruit1? nullableFruit1C;
        private Fruit1 fruit1L;
        private Fruit2 fruit2L;
        private Fruit3 fruit3L;
        private Fruit4 fruit4L;
        private Fruit5 fruit5L;
        private Fruit6 fruit6L;
        private Fruit1? nullableFruit1L;

        [Category("RadioButtonList|")]
        [Description("Normal enum")]
        public Fruit1 Fruit1 { get => this.fruit1; set { this.fruit1 = value; this.RaisePropertyChanged(nameof(Fruit1)); } }

        [Description("Empty enum")]
        public Fruit2 Fruit2 { get => this.fruit2; set { this.fruit2 = value; this.RaisePropertyChanged(nameof(Fruit2)); } }

        [Description("First item is not browsable")]
        public Fruit3 Fruit3 { get => this.fruit3; set { this.fruit3 = value; this.RaisePropertyChanged(nameof(Fruit3)); } }

        [Description("No items are browsable")]
        public Fruit4 Fruit4 { get => this.fruit4; set { this.fruit4 = value; this.RaisePropertyChanged(nameof(Fruit4)); } }

        [Description("With descriptions")]
        public Fruit5 Fruit5 { get => this.fruit5; set { this.fruit5 = value; this.RaisePropertyChanged(nameof(Fruit5)); } }

        [Description("Bit field (FlagsAttribute)")]
        public Fruit6 Fruit6 { get => this.fruit6; set { this.fruit6 = value; this.RaisePropertyChanged(nameof(Fruit6)); } }

        [Description("Nullable enum")]
        public Fruit1? NullableFruit1 { get => this.nullableFruit1; set { this.nullableFruit1 = value; this.RaisePropertyChanged(nameof(NullableFruit1)); } }

        [Description("First item is not browsable")]
        [SelectorStyle(SelectorStyle.RadioButtons)]
        public Fruit3 RadioButtons3 { get => this.radioButtons3; set { this.radioButtons3 = value; this.RaisePropertyChanged(nameof(RadioButtons3)); } }

        [Category("ComboBox|")]
        [Description("Normal enum")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit1 Fruit1C { get => this.fruit1C; set { this.fruit1C = value; this.RaisePropertyChanged(nameof(Fruit1C)); } }

        [Description("Empty enum")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit2 Fruit2C { get => this.fruit2C; set { this.fruit2C = value; this.RaisePropertyChanged(nameof(Fruit2C)); } }

        [Description("First item is not browsable")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit3 Fruit3C { get => this.fruit3C; set { this.fruit3C = value; this.RaisePropertyChanged(nameof(Fruit3C)); } }

        [Description("No items are browsable")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit4 Fruit4C { get => this.fruit4C; set { this.fruit4C = value; this.RaisePropertyChanged(nameof(Fruit4C)); } }

        [Description("With descriptions")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit5 Fruit5C { get => this.fruit5C; set { this.fruit5C = value; this.RaisePropertyChanged(nameof(Fruit5C)); } }

        [Description("Bit field (FlagsAttribute)")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit6 Fruit6C { get => this.fruit6C; set { this.fruit6C = value; this.RaisePropertyChanged(nameof(Fruit6C)); } }

        [Description("Nullable enum")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit1? NullableFruit1C { get => this.nullableFruit1C; set { this.nullableFruit1C = value; this.RaisePropertyChanged(nameof(NullableFruit1C)); } }



        [Category("ListBox|")]
        [Description("Normal enum")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit1 Fruit1L { get => this.fruit1L; set { this.fruit1L = value; this.RaisePropertyChanged(nameof(Fruit1L)); } }

        [Description("Empty enum")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit2 Fruit2L { get => this.fruit2L; set { this.fruit2L = value; this.RaisePropertyChanged(nameof(Fruit2L)); } }

        [Description("First item is not browsable")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit3 Fruit3L { get => this.fruit3L; set { this.fruit3L = value; this.RaisePropertyChanged(nameof(Fruit3L)); } }

        [Description("No items are browsable")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit4 Fruit4L { get => this.fruit4L; set { this.fruit4L = value; this.RaisePropertyChanged(nameof(Fruit4L)); } }

        [Description("With descriptions")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit5 Fruit5L { get => this.fruit5L; set { this.fruit5L = value; this.RaisePropertyChanged(nameof(Fruit5L)); } }

        [Description("Bit field (FlagsAttribute)")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit6 Fruit6L { get => this.fruit6L; set { this.fruit6L = value; this.RaisePropertyChanged(nameof(Fruit6L)); } }

        [Description("Nullable enum")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit1? NullableFruit1L { get => this.nullableFruit1L; set { this.nullableFruit1L = value; this.RaisePropertyChanged(nameof(NullableFruit1L)); } }
    }
}