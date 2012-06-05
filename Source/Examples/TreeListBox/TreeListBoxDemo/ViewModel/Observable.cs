namespace TreeListBoxDemo
{
    using System.ComponentModel;

    public abstract class Observable : INotifyPropertyChanged
    {
        #region PropertyChanged Block
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion
    }
}