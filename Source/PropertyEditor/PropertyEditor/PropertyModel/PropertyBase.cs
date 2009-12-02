using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;

namespace OpenControls
{
    public abstract class PropertyBase : INotifyPropertyChanged, IDisposable
    {
        public string Header { get; set; }
        public object ToolTip { get; set; }
        
        #region Enabled/Visible

        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { _IsEnabled = value; NotifyPropertyChanged("IsEnabled"); }
        }

        private Visibility _Visibility;
        public Visibility Visibility
        {
            get { return _Visibility; }
            set { _Visibility = value; NotifyPropertyChanged("Visibility"); }
        }

        #endregion

        public PropertyBase()
        {
            _IsEnabled = true;
            _Visibility = Visibility.Visible;
        }

        #region Notify Property Changed Members

        protected void NotifyPropertyChanged(string property)
        {
            Debug.IndentLevel++;
            Debug.WriteLine("PropertyBase.NotifyPropertyChanged");
            var handler = PropertyChanged;
            if (handler!= null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
            Debug.IndentLevel--;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IDisposable Members

        private bool _disposed = false;

        protected bool Disposed
        {
            get { return _disposed; }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PropertyBase() { Dispose(false); }

        #endregion
    }
}
