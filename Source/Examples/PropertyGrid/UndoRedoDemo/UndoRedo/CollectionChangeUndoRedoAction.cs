// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionChangeUndoRedoAction.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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