namespace UndoRedoDemo
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public class CollectionChangeUndoRedoAction : IUndoRedoAction
    {
        public IList Collection { get; set; }

        public NotifyCollectionChangedEventArgs Args { get; set; }

        public CollectionChangeUndoRedoAction(IList collection, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Collection = collection;
            Args = e;
        }

        public override string ToString()
        {
            return Args.Action.ToString();
        }

        public IUndoRedoAction Execute()
        {
            var ii = Collection as ISupportInitialize;
            if (ii != null) ii.BeginInit();
            IUndoRedoAction result = null;
            if (Args.Action == NotifyCollectionChangedAction.Add)
            {
                Collection.RemoveAt(Collection.Count - 1);
                result = new CollectionChangeUndoRedoAction(Collection, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Args.NewItems));
            }
            if (Args.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var i in Args.OldItems)
                    Collection.Add(i);
                result = new CollectionChangeUndoRedoAction(Collection, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Args.OldItems));
            }
            if (ii != null) ii.EndInit();
            return result;
        }
    }
}