namespace FeaturesDemo
{
    using System.ComponentModel;

    public class Observable : INotifyPropertyChanged
    {
        #region PropertyChanged Block
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            //    Debug.WriteLine(property + " was changed.");
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion
    }
}