namespace UndoRedoDemo
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public class UndoableCollection<T> : ObservableCollection<T>, ISupportInitialize
    {
        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            if (!isInitializing)
                ShellViewModel.UndoRedoService.Add(new CollectionChangeUndoRedoAction(this, e));
        }

        private bool isInitializing;
        public void BeginInit()
        {
            isInitializing = true;
        }

        public void EndInit()
        {
            isInitializing = false;
        }
    }
}