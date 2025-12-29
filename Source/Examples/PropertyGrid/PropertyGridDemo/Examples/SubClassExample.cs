// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubClassExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.ComponentModel;

    [PropertyGridExample]
    public class SubClassExample : SuperClass
    {
        private string name2;

        public string Name2 { get => this.name2; set { this.name2 = value; this.RaisePropertyChanged(nameof(Name2)); } }        
    }

    public class SuperClass : Example
    {
        private string name1;

        [Description("Check 'Show declared only' on the 'PropertyGrid' menu.")]
        [Category("SuperClass")]
        public string Name1 { get => this.name1; set { this.name1 = value; this.RaisePropertyChanged(nameof(Name1)); } }
    }
}