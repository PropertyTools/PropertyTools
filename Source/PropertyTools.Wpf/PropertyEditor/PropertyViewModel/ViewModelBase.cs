using System;
using System.ComponentModel;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Base class for tabs, categories and properties 
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IComparable
    {
        protected PropertyEditor Owner { get; private set; }

        public string Header { get; set; }
        public object ToolTip { get; set; }
        public int SortOrder { get; set; }

        protected ViewModelBase(PropertyEditor owner)
        {
            Owner = owner;
            SortOrder = int.MinValue;
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

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return SortOrder.CompareTo(((ViewModelBase)obj).SortOrder);
        }

        #endregion
    }
}
