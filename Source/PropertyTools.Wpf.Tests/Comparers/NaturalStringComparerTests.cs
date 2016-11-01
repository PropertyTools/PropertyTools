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
            Assert.AreEqual(0, nsc.Compare(null, null));
            Assert.AreEqual(-1, nsc.Compare(null, "X"));
            Assert.AreEqual(1, nsc.Compare("X", null));
            Assert.AreEqual(0, nsc.Compare("X", "X"));
            Assert.AreEqual(-1, nsc.Compare("X", "Y"));
            Assert.AreEqual(-1, nsc.Compare("2", "10"));
            Assert.AreEqual(-1, nsc.Compare("X2", "X10"));
            Assert.AreEqual(1, nsc.Compare("X2", "X1"));
            Assert.AreEqual(-1, nsc.Compare("v1.0.2", "v1.0.10"));
            Assert.AreEqual(-1, nsc.Compare("1.2", "1.10"));
            Assert.AreEqual(-1, nsc.Compare("1 2", "1 10"));
            Assert.AreEqual(-1, nsc.Compare("1.2", "1 10"));
            Assert.AreEqual(-1, nsc.Compare("1-2", "1/10"));
        }
    }
}