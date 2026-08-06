// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyGraphTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.Linq;

    using SpreadsheetDemo.Spreadsheet.Model;
    using SpreadsheetDemo.Spreadsheet.Model.Calculation;

    using NUnit.Framework;

    [TestFixture]
    public class DependencyGraphTests
    {
        [Test]
        public void GetDirectDependents_DirectPrecedent_ReturnsDependent()
        {
            var graph = new DependencyGraph();
            var b1 = new CellAddress(0, 1);
            var a1 = new CellAddress(0, 0);

            graph.SetPrecedents(b1, new[] { a1 }, Array.Empty<CellRange>());

            Assert.That(graph.GetDirectDependents(a1), Is.EquivalentTo(new[] { b1 }));
        }

        [Test]
        public void GetDirectDependents_RangePrecedentContainingCell_ReturnsDependent()
        {
            var graph = new DependencyGraph();
            var sumCell = new CellAddress(5, 0);
            var range = new CellRange(new CellAddress(0, 0), new CellAddress(2, 0));

            graph.SetPrecedents(sumCell, Array.Empty<CellAddress>(), new[] { range });

            Assert.That(graph.GetDirectDependents(new CellAddress(1, 0)), Is.EquivalentTo(new[] { sumCell }));
        }

        [Test]
        public void GetDirectDependents_CellOutsideRange_ReturnsEmpty()
        {
            var graph = new DependencyGraph();
            var range = new CellRange(new CellAddress(0, 0), new CellAddress(2, 0));
            graph.SetPrecedents(new CellAddress(5, 0), Array.Empty<CellAddress>(), new[] { range });

            Assert.That(graph.GetDirectDependents(new CellAddress(10, 0)), Is.Empty);
        }

        [Test]
        public void SetPrecedents_CalledAgainWithDifferentPrecedent_RemovesOldEdge()
        {
            var graph = new DependencyGraph();
            var dependent = new CellAddress(0, 2);
            var oldPrecedent = new CellAddress(0, 0);
            var newPrecedent = new CellAddress(0, 1);

            graph.SetPrecedents(dependent, new[] { oldPrecedent }, Array.Empty<CellRange>());
            graph.SetPrecedents(dependent, new[] { newPrecedent }, Array.Empty<CellRange>());

            Assert.That(graph.GetDirectDependents(oldPrecedent), Is.Empty);
            Assert.That(graph.GetDirectDependents(newPrecedent), Is.EquivalentTo(new[] { dependent }));
        }

        [Test]
        public void SetPrecedents_CalledAgainWithNoPrecedents_ClearsDependents()
        {
            var graph = new DependencyGraph();
            var dependent = new CellAddress(0, 1);
            var precedent = new CellAddress(0, 0);

            graph.SetPrecedents(dependent, new[] { precedent }, Array.Empty<CellRange>());
            graph.SetPrecedents(dependent, Array.Empty<CellAddress>(), Array.Empty<CellRange>());

            Assert.That(graph.GetDirectDependents(precedent), Is.Empty);
        }

        [Test]
        public void Remove_RemovesBothDirectAndRangeEdges()
        {
            var graph = new DependencyGraph();
            var dependent = new CellAddress(0, 2);
            var precedent = new CellAddress(0, 0);
            var range = new CellRange(new CellAddress(1, 0), new CellAddress(1, 5));

            graph.SetPrecedents(dependent, new[] { precedent }, new[] { range });
            graph.Remove(dependent);

            Assert.That(graph.GetDirectDependents(precedent), Is.Empty);
            Assert.That(graph.GetDirectDependents(new CellAddress(1, 2)), Is.Empty);
        }

        [Test]
        public void GetDirectDependents_MultipleDependents_ReturnsAllWithoutDuplicates()
        {
            var graph = new DependencyGraph();
            var precedent = new CellAddress(0, 0);
            var d1 = new CellAddress(0, 1);
            var d2 = new CellAddress(0, 2);
            var range = new CellRange(precedent, precedent);

            graph.SetPrecedents(d1, new[] { precedent }, Array.Empty<CellRange>());
            graph.SetPrecedents(d2, Array.Empty<CellAddress>(), new[] { range });

            var dependents = graph.GetDirectDependents(precedent).ToList();

            Assert.That(dependents, Is.EquivalentTo(new[] { d1, d2 }));
        }
    }
}
