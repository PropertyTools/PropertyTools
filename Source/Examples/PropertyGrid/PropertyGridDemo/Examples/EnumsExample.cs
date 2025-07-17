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
        [Category("RadioButtonList|")]
        [Description("Normal enum")]
        public Fruit1 Fruit1 { get; set; }

        [Description("Empty enum")]
        public Fruit2 Fruit2 { get; set; }

        [Description("First item is not browsable")]
        public Fruit3 Fruit3 { get; set; }

        [Description("No items are browsable")]
        public Fruit4 Fruit4 { get; set; }

        [Description("With descriptions")]
        public Fruit5 Fruit5 { get; set; }

        [Description("Bit field (FlagsAttribute)")]
        public Fruit6 Fruit6 { get; set; }

        [Description("Nullable enum")]
        public Fruit1? NullableFruit1 { get; set; }

        [Description("First item is not browsable")]
        [SelectorStyle(SelectorStyle.RadioButtons)]
        public Fruit3 RadioButtons3 { get; set; }

        [Category("ComboBox|")]
        [Description("Normal enum")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit1 Fruit1C { get; set; }

        [Description("Empty enum")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit2 Fruit2C { get; set; }

        [Description("First item is not browsable")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit3 Fruit3C { get; set; }

        [Description("No items are browsable")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit4 Fruit4C { get; set; }

        [Description("With descriptions")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit5 Fruit5C { get; set; }

        [Description("Bit field (FlagsAttribute)")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit6 Fruit6C { get; set; }

        [Description("Nullable enum")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        public Fruit1? NullableFruit1C { get; set; }



        [Category("ListBox|")]
        [Description("Normal enum")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit1 Fruit1L { get; set; }

        [Description("Empty enum")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit2 Fruit2L { get; set; }

        [Description("First item is not browsable")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit3 Fruit3L { get; set; }

        [Description("No items are browsable")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit4 Fruit4L { get; set; }

        [Description("With descriptions")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit5 Fruit5L { get; set; }

        [Description("Bit field (FlagsAttribute)")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit6 Fruit6L { get; set; }

        [Description("Nullable enum")]
        [SelectorStyle(SelectorStyle.ListBox)]
        public Fruit1? NullableFruit1L { get; set; }
    }
}