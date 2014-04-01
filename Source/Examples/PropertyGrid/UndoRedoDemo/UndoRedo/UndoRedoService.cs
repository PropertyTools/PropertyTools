// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UndoRedoService.cs" company="PropertyTools">
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;

    public class UndoRedoService : INotifyPropertyChanged
    {
        private Stack<IUndoRedoAction> UndoStack = new Stack<IUndoRedoAction>();
        private Stack<IUndoRedoAction> RedoStack = new Stack<IUndoRedoAction>();

        public void Add(IUndoRedoAction redoAction)
        {
            UndoStack.Push(redoAction);
            RedoStack.Clear();
            this.RaisePropertyChangedEvents();
        }

        void RaisePropertyChangedEvents()
        {
            RaisePropertyChanged("CanUndo");
            RaisePropertyChanged("CanRedo");
        }

        public bool CanUndo
        {
            get
            {
                return UndoStack.Count > 0;
            }
        }

        public void Undo()
        {
            var item = UndoStack.Pop();
            Trace.WriteLine("Undo " + item);
            var redoItem = item.Execute();
            RedoStack.Push(redoItem);
            this.RaisePropertyChangedEvents();
        }
        public bool CanRedo
        {
            get
            {
                return RedoStack.Count > 0;
            }
        }

        public void Redo()
        {
            var item = RedoStack.Pop();
            var undoItem = item.Execute();
            Trace.WriteLine("Redo: " + undoItem);
            UndoStack.Push(undoItem);
            this.RaisePropertyChangedEvents();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}