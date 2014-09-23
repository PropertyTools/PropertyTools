// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestObject.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace CustomFactoryDemo
{
    public class TestObject
    {
        public string Title { get; set; }

        [Important]
        public Range Range1 { get; set; }

        public Range Range2 { get; set; }
    }
}