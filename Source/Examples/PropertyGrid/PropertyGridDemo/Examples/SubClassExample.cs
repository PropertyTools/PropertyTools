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
        public string Name2 { get; set; }        
    }

    public class SuperClass : Example
    {
        [Description("Check 'Show declared only' on the 'PropertyGrid' menu.")]
        [Category("SuperClass")]
        public string Name1 { get; set; }
    }
}