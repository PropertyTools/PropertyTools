// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionChangeUndoRedoAction.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace UndoRedoDemo
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public class CollectionChangeUndoRedoAction : IUndoRedoAction
    {
        public IList Collection { get; set; }

        public NotifyCollectionChangedEventArgs Args { get; set; }

        public CollectionChangeUndoRedoAction(IList collection, NotifyCollectionChangedEventArgs e)
        {
            this.Collection = collection;
            this.Args = e;
        }

        public override string ToString()
        {
            return this.Args.Action.ToString();
        }

        public IUndoRedoAction Execute()
        {
            var ii = this.Collection as ISupportInitialize;
            if (ii != null) ii.BeginInit();
            IUndoRedoAction result = null;
            if (this.Args.Action == NotifyCollectionChangedAction.Add)
            {
                this.Collection.RemoveAt(this.Collection.Count - 1);
                result = new CollectionChangeUndoRedoAction(this.Collection, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Args.NewItems));
            }

            if (this.Args.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var i in Args.OldItems)
                {
                    this.Collection.Add(i);
                }

                result = new CollectionChangeUndoRedoAction(this.Collection, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Args.OldItems));
            }

            if (ii != null) ii.EndInit();
            return result;
        }
    }
}