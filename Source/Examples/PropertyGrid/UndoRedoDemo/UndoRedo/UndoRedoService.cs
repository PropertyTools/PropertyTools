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
        private readonly Stack<IUndoRedoAction> undoStack = new Stack<IUndoRedoAction>();
        private readonly Stack<IUndoRedoAction> redoStack = new Stack<IUndoRedoAction>();

        public void Add(IUndoRedoAction redoAction)
        {
            this.undoStack.Push(redoAction);
            this.redoStack.Clear();
            this.RaisePropertyChangedEvents();
        }

        void RaisePropertyChangedEvents()
        {
            this.RaisePropertyChanged("CanUndo");
            this.RaisePropertyChanged("CanRedo");
        }

        public bool CanUndo => this.undoStack.Count > 0;

        public void Undo()
        {
            var item = this.undoStack.Pop();
            Trace.WriteLine("Undo " + item);
            var redoItem = item.Execute();
            this.redoStack.Push(redoItem);
            this.RaisePropertyChangedEvents();
        }
        public bool CanRedo => this.redoStack.Count > 0;

        public void Redo()
        {
            var item = this.redoStack.Pop();
            var undoItem = item.Execute();
            Trace.WriteLine("Redo: " + undoItem);
            this.undoStack.Push(undoItem);
            this.RaisePropertyChangedEvents();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}