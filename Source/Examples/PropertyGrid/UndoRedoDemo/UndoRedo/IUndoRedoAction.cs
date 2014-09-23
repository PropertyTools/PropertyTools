// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUndoRedoAction.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace UndoRedoDemo
{
    public interface IUndoRedoAction
    {
        IUndoRedoAction Execute();
    }
}