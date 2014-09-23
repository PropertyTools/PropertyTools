// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UndoRedoService.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
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