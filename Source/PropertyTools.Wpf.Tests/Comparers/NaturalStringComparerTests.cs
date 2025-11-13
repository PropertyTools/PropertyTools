// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NaturalStringComparerTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class NaturalStringComparerTests
    {
        [Test]
        public void Compare()
        {
            var nsc = new NaturalStringComparer();
            Assert.That(nsc.Compare(null, null), Is.EqualTo(0));
            Assert.That(nsc.Compare(null, "X"), Is.EqualTo(-1));
            Assert.That(nsc.Compare("X", null), Is.EqualTo(1));
            Assert.That(nsc.Compare("X", "X"), Is.EqualTo(0));
            Assert.That(nsc.Compare("X", "Y"), Is.EqualTo(-1));
            Assert.That(nsc.Compare("2", "10"), Is.EqualTo(-1));
            Assert.That(nsc.Compare("X2", "X10"), Is.EqualTo(-1));
            Assert.That(nsc.Compare("X2", "X1"), Is.EqualTo(1));
            Assert.That(nsc.Compare("v1.0.2", "v1.0.10"), Is.EqualTo(-1));
            Assert.That(nsc.Compare("1.2", "1.10"), Is.EqualTo(-1));
            Assert.That(nsc.Compare("1 2", "1 10"), Is.EqualTo(-1));
            Assert.That(nsc.Compare("1.2", "1 10"), Is.EqualTo(-1));
            Assert.That(nsc.Compare("1-2", "1/10"), Is.EqualTo(-1));
        }
    }
}