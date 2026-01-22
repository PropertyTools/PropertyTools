// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionalAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Windows.Media;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class OptionalAttributeExample : Example
    {
        private string name;
        private bool useName;
        private int age;
        private bool useAge;
        private double weight;
        private bool specifyWeight;
        private int? number;
        private string @string;
        private double @double;
        private Color? color;
        private DateTime? dateTime;

        [Category("By convention")]
        [Description("The UseName property specifies if Name is active or not.")]
        public string Name { get => this.name; set { this.name = value; this.RaisePropertyChanged(nameof(Name)); } }

        [Browsable(false)]
        public bool UseName { get => this.useName; set { this.useName = value; this.RaisePropertyChanged(nameof(UseName)); } }

        [Description("The UseAge property specifies if Age is active or not.")]
        public int Age { get => this.age; set { this.age = value; this.RaisePropertyChanged(nameof(Age)); } }

        [Browsable(false)]
        public bool UseAge { get => this.useAge; set { this.useAge = value; this.RaisePropertyChanged(nameof(UseAge)); } }

        [Category("By OptionalAttribute")]
        [Optional("SpecifyWeight")]
        [Description("The SpecifyWeight property specifies if the Weight is active or not.")]
        public double Weight { get => this.weight; set { this.weight = value; this.RaisePropertyChanged(nameof(Weight)); } }

        [Browsable(false)]
        public bool SpecifyWeight { get => this.specifyWeight; set { this.specifyWeight = value; this.RaisePropertyChanged(nameof(SpecifyWeight)); } }

        [Optional]
        [Description("The Nullable property is inactive if set to null.")]
        public int? Number { get => this.number; set { this.number = value; this.RaisePropertyChanged(nameof(Number)); } }

        [Optional]
        [Description("The property is inactive if set to null.")]
        public string String { get => this.@string; set { this.@string = value; this.RaisePropertyChanged(nameof(String)); } }

        [Optional]
        [Description("The property is inactive if set to NaN.")]
        public double Double { get => this.@double; set { this.@double = value; this.RaisePropertyChanged(nameof(Double)); } }

        [Optional]
        [Description("The property is inactive if set to null.")]
        public Color? Color { get => this.color; set { this.color = value; this.RaisePropertyChanged(nameof(Color)); } }

        [Optional]
        [Description("The property is inactive if set to null.")]
        public DateTime? DateTime { get => this.dateTime; set { this.dateTime = value; this.RaisePropertyChanged(nameof(DateTime)); } }
    }
}