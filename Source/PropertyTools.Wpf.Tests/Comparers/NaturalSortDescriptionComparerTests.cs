// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NaturalSortDescriptionComparerTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.ComponentModel;

    using NUnit.Framework;

    [TestFixture]
    public class NaturalSortDescriptionComparerTests
    {
        private class TestItem
        {
            public string Group { get; set; }

            public string Name { get; set; }
        }

        [Test]
        public void Compare_NullItem_ReturnsNullAsLowestValue()
        {
            var comparer = new NaturalSortDescriptionComparer();
            comparer.SortDescriptions.Add(new SortDescription(nameof(TestItem.Name), ListSortDirection.Ascending));

            var result = comparer.Compare(null, new TestItem { Name = "Item1" });

            Assert.That(result, Is.LessThan(0));
        }

        [Test]
        public void Compare_MissingProperty_ThrowsInvalidOperationException()
        {
            var comparer = new NaturalSortDescriptionComparer();
            comparer.SortDescriptions.Add(new SortDescription("MissingProperty", ListSortDirection.Ascending));

            var exception = Assert.Throws<InvalidOperationException>(() => comparer.Compare(new TestItem(), new TestItem()));

            Assert.That(exception.Message, Does.Contain("MissingProperty"));
        }

        [Test]
        public void Compare_MultipleSortDescriptions_UsesSecondarySortDirection()
        {
            var comparer = new NaturalSortDescriptionComparer();
            comparer.SortDescriptions.Add(new SortDescription(nameof(TestItem.Group), ListSortDirection.Ascending));
            comparer.SortDescriptions.Add(new SortDescription(nameof(TestItem.Name), ListSortDirection.Descending));

            var left = new TestItem { Group = "A", Name = "Item2" };
            var right = new TestItem { Group = "A", Name = "Item10" };

            var result = comparer.Compare(left, right);

            Assert.That(result, Is.GreaterThan(0));
        }
    }
}
