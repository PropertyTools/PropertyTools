// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSubClass.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.ComponentModel;

    [PropertyGridExample]
    public class TestSubClass : SuperClass
    {
        public string Name2 { get; set; }

        public override string ToString()
        {
            return "Sub-class";
        }
    }

    public class SuperClass : TestBase
    {
        [Description("Check 'Show declared only' on the 'PropertyGrid' menu.")]
        [Category("SuperClass")]
        public string Name1 { get; set; }
    }
}