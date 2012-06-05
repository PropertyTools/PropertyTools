namespace UndoRedoDemo
{
    public interface IUndoRedoAction
    {
        IUndoRedoAction Execute();
    }
}