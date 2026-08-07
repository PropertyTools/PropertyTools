// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecalculationEngine.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Recomputes formula values for a Sheet in dependency order.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Calculation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Recomputes formula values for a <see cref="Sheet" /> in dependency order, only for the cells
    /// affected by a change (a dirty cell and everything transitively dependent on it).
    /// </summary>
    internal sealed class RecalculationEngine
    {
        private readonly Sheet sheet;
        private readonly DependencyGraph graph;
        private readonly HashSet<CellAddress> dirty = new HashSet<CellAddress>();

        /// <summary>
        /// The addresses currently being visited by <see cref="Visit" />, in call-stack order — used
        /// to find every cell on a cycle when a back-edge is detected.
        /// </summary>
        private readonly List<CellAddress> visitStack = new List<CellAddress>();

        private int deferDepth;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecalculationEngine" /> class.
        /// </summary>
        public RecalculationEngine(Sheet sheet, DependencyGraph graph)
        {
            this.sheet = sheet;
            this.graph = graph;
        }

        /// <summary>
        /// Marks a cell dirty. Triggers an immediate <see cref="Recalculate" /> unless a
        /// <see cref="Defer" /> scope is active.
        /// </summary>
        public void MarkDirty(CellAddress address)
        {
            this.dirty.Add(address);
            if (this.deferDepth == 0)
            {
                this.Recalculate();
            }
        }

        /// <summary>
        /// Defers recalculation until the returned scope is disposed, so that many edits (paste, a
        /// bulk clear, a file load) only trigger one recalculation pass.
        /// </summary>
        public IDisposable Defer()
        {
            this.deferDepth++;
            return new DeferScope(this);
        }

        /// <summary>
        /// Recomputes every dirty cell and everything transitively dependent on it, in dependency
        /// order, detecting circular references.
        /// </summary>
        public void Recalculate()
        {
            if (this.dirty.Count == 0)
            {
                return;
            }

            var affected = this.CollectAffectedCells();
            this.dirty.Clear();

            var order = new List<CellAddress>();
            var circular = new HashSet<CellAddress>();
            var state = new Dictionary<CellAddress, VisitState>();

            foreach (var address in affected)
            {
                this.Visit(address, affected, state, order, circular);
            }

            foreach (var address in order)
            {
                this.RecalculateCell(address, circular);
            }
        }

        private HashSet<CellAddress> CollectAffectedCells()
        {
            var affected = new HashSet<CellAddress>(this.dirty);
            var queue = new Queue<CellAddress>(this.dirty);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var dependent in this.graph.GetDirectDependents(current))
                {
                    if (affected.Add(dependent))
                    {
                        queue.Enqueue(dependent);
                    }
                }
            }

            return affected;
        }

        /// <summary>
        /// Visits a cell's precedents depth-first (restricted to <paramref name="affected" />) before
        /// adding the cell itself to <paramref name="order" />, producing a dependency-respecting
        /// (topological) recalculation order. A back-edge to a cell still being visited means every
        /// cell from that point to the top of <see cref="visitStack" /> is part of a circular reference.
        /// </summary>
        private void Visit(
            CellAddress address,
            HashSet<CellAddress> affected,
            Dictionary<CellAddress, VisitState> state,
            List<CellAddress> order,
            HashSet<CellAddress> circular)
        {
            if (state.TryGetValue(address, out var existing))
            {
                if (existing == VisitState.Visiting)
                {
                    var index = this.visitStack.IndexOf(address);
                    if (index >= 0)
                    {
                        for (var i = index; i < this.visitStack.Count; i++)
                        {
                            circular.Add(this.visitStack[i]);
                        }
                    }
                }

                return;
            }

            state[address] = VisitState.Visiting;
            this.visitStack.Add(address);

            if (this.sheet.TryGetCell(address, out var cell)
                && cell.Content.IsFormula
                && !cell.Content.Formula.HasSyntaxError)
            {
                foreach (var precedent in cell.Content.Formula.Precedents)
                {
                    if (affected.Contains(precedent))
                    {
                        this.Visit(precedent, affected, state, order, circular);
                    }
                }

                foreach (var rangePrecedent in cell.Content.Formula.RangePrecedents)
                {
                    foreach (var precedent in rangePrecedent)
                    {
                        if (affected.Contains(precedent))
                        {
                            this.Visit(precedent, affected, state, order, circular);
                        }
                    }
                }
            }

            this.visitStack.RemoveAt(this.visitStack.Count - 1);
            state[address] = VisitState.Done;
            order.Add(address);
        }

        private void RecalculateCell(CellAddress address, HashSet<CellAddress> circular)
        {
            if (!this.sheet.TryGetCell(address, out var cell))
            {
                return;
            }

            cell.SetValueCore(circular.Contains(address) ? CellValue.FromError(CellError.Circular) : this.sheet.Evaluate(cell.Content));
        }

        private enum VisitState
        {
            Visiting,
            Done
        }

        /// <summary>
        /// Ends a <see cref="Defer" /> scope, triggering a recalculation if this was the outermost one.
        /// </summary>
        private sealed class DeferScope : IDisposable
        {
            private readonly RecalculationEngine owner;
            private bool disposed;

            public DeferScope(RecalculationEngine owner)
            {
                this.owner = owner;
            }

            public void Dispose()
            {
                if (this.disposed)
                {
                    return;
                }

                this.disposed = true;
                this.owner.deferDepth--;
                if (this.owner.deferDepth == 0)
                {
                    this.owner.Recalculate();
                }
            }
        }
    }
}
