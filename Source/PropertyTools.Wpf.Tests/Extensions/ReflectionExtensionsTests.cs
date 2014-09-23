// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensionsTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ReflectionExtensionsTests
    {
        public enum Enum1
        {
            [Browsable(false)]
            NotShownValue,
            Value1,
            Value2,
            [Browsable(false)]
            [System.ComponentModel.Description("Description of AnotherNotShownValue")]
            AnotherNotShownValue,
            [System.ComponentModel.Description("Description of Value3")]
            Value3
        }

        [Test]
        public void FilterOnBrowsableAttribute()
        {
            Assert.AreEqual(5, Enum.GetValues(typeof(Enum1)).Length);
            Assert.AreEqual(3, Enum.GetValues(typeof(Enum1)).FilterOnBrowsableAttribute().Count());
        }
    }
}