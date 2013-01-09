// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBase.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Base class for tabs, categories and properties
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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

        protected void NotifyPropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public int CompareTo(object obj)
        {
            return SortOrder.CompareTo(((PropertyBase)obj).SortOrder);
        }

    }
}