// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyGraph.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Tracks which cells a formula reads, so a changed cell can find everything that depends on it.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Calculation
{
    using System.Collections.Generic;

    /// <summary>
    /// Tracks which cells a formula reads (its precedents), so that a changed cell can find every
    /// cell that (directly or via a range) depends on it.
    /// </summary>
    internal sealed class DependencyGraph
    {
        /// <summary>
        /// For each dependent cell with a formula, the individual cells it currently reads directly
        /// (kept so <see cref="SetPrecedents" /> can remove stale edges when a formula is re-edited).
        /// </summary>
        private readonly Dictionary<CellAddress, List<CellAddress>> currentDirectPrecedents =
            new Dictionary<CellAddress, List<CellAddress>>();

        /// <summary>
        /// For each dependent cell with a formula, the ranges it currently reads.
        /// </summary>
        private readonly Dictionary<CellAddress, List<CellRange>> currentRangePrecedents =
            new Dictionary<CellAddress, List<CellRange>>();

        /// <summary>
        /// For each precedent cell, the set of cells that read it directly.
        /// </summary>
        private readonly Dictionary<CellAddress, HashSet<CellAddress>> directDependents =
            new Dictionary<CellAddress, HashSet<CellAddress>>();

        /// <summary>
        /// Every (range precedent, dependent cell) edge. A linear scan is fine at the scale this demo
        /// targets; see AGENTS.md/the implementation plan for the bucketing optimization noted for later.
        /// </summary>
        private readonly List<(CellRange Range, CellAddress Dependent)> rangeDependents =
            new List<(CellRange Range, CellAddress Dependent)>();

        /// <summary>
        /// Replaces the precedents of <paramref name="cell" />. Call this every time a cell's content
        /// changes, even to an empty list, so stale edges from a previous formula are removed.
        /// </summary>
        public void SetPrecedents(CellAddress cell, IReadOnlyList<CellAddress> precedents, IReadOnlyList<CellRange> rangePrecedents)
        {
            this.Remove(cell);

            if (precedents.Count > 0)
            {
                var list = new List<CellAddress>(precedents);
                this.currentDirectPrecedents[cell] = list;
                foreach (var precedent in list)
                {
                    if (!this.directDependents.TryGetValue(precedent, out var dependents))
                    {
                        dependents = new HashSet<CellAddress>();
                        this.directDependents[precedent] = dependents;
                    }

                    dependents.Add(cell);
                }
            }

            if (rangePrecedents.Count > 0)
            {
                var list = new List<CellRange>(rangePrecedents);
                this.currentRangePrecedents[cell] = list;
                foreach (var range in list)
                {
                    this.rangeDependents.Add((range, cell));
                }
            }
        }

        /// <summary>
        /// Removes all precedent edges for <paramref name="cell" />.
        /// </summary>
        public void Remove(CellAddress cell)
        {
            if (this.currentDirectPrecedents.TryGetValue(cell, out var oldDirect))
            {
                foreach (var precedent in oldDirect)
                {
                    if (this.directDependents.TryGetValue(precedent, out var dependents))
                    {
                        dependents.Remove(cell);
                        if (dependents.Count == 0)
                        {
                            this.directDependents.Remove(precedent);
                        }
                    }
                }

                this.currentDirectPrecedents.Remove(cell);
            }

            if (this.currentRangePrecedents.ContainsKey(cell))
            {
                this.rangeDependents.RemoveAll(edge => edge.Dependent.Equals(cell));
                this.currentRangePrecedents.Remove(cell);
            }
        }

        /// <summary>
        /// Gets every cell that directly depends on <paramref name="changed" /> — reads it as a single
        /// cell reference, or via a range that contains it.
        /// </summary>
        public IEnumerable<CellAddress> GetDirectDependents(CellAddress changed)
        {
            var result = new HashSet<CellAddress>();

            if (this.directDependents.TryGetValue(changed, out var dependents))
            {
                foreach (var dependent in dependents)
                {
                    result.Add(dependent);
                }
            }

            foreach (var edge in this.rangeDependents)
            {
                if (edge.Range.Contains(changed))
                {
                    result.Add(edge.Dependent);
                }
            }

            return result;
        }
    }
}
