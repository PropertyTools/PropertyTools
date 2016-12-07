// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestEnums.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using PropertyTools.DataAnnotations;

    public enum Fruit { Apple, Pear, Banana }

    public enum Fruit2 { }

    public enum Fruit3 {[Browsable(false)] Apple, Pear, Banana }

    public enum Fruit4 {[Browsable(false)] Apple, [Browsable(false)] Pear, [Browsable(false)] Banana }

    public enum Fruit5 {[Description("Eple")] Apple, [Description("Pære")] Pear, [Description("Banan")] Banana }

    [Flags]
    public enum Fruit6 { All = Apple | Pear | Banana, Apple = 1, Pear = 2, Banana = 4 }

    [PropertyGridExample]
    public class EnumsExample : Example
    {
        [Description("Normal enum")]
        public Fruit Fruit { get; set; }

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
        public Fruit? Fruit7 { get; set; }
    }
}