using System.ComponentModel;
using System.Windows;
using System;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Base class for tabs, categories and properties 
    /// </summary>
    public abstract class PropertyBase : INotifyPropertyChanged, IDisposable, IComparable
    {
        protected PropertyEditor Owner { get; private set; }

        public string Name { get; set; }
        public string Header { get; set; }
        public object ToolTip { get; set; }
        public int SortOrder { get; set; }

        #region Enabled/Visible

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; NotifyPropertyChanged("IsEnabled"); }
        }

        private Visibility isVisible;
        public Visibility IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; NotifyPropertyChanged("IsVisible"); }
        }

        #endregion

        protected PropertyBase(PropertyEditor owner)
        {
            Owner = owner;
            
            isEnabled = true;
            isVisible = Visibility.Visible;
        }

        public override string ToString()
        {
            return Header;
        }

        #region Notify Property Changed Members

        protected void NotifyPropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        // http://msdn.microsoft.com/en-us/library/ms244737(VS.80).aspx

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~PropertyBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// The bulk of the clean-up code is implemented in Dispose(bool)
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return SortOrder.CompareTo(((PropertyBase)obj).SortOrder);
        }

        #endregion
    }
}
