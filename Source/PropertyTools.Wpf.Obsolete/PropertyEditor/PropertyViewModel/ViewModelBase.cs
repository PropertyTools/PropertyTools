// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="PropertyTools">
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
namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Base class for tabs, categories and properties
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        protected ViewModelBase(PropertyEditor owner)
        {
            this.Owner = owner;
            this.SortIndex = int.MinValue;
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets Header.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets SortIndex.
        /// </summary>
        public int SortIndex { get; set; }

        /// <summary>
        /// Gets or sets ToolTip.
        /// </summary>
        public object ToolTip { get; set; }

        /// <summary>
        /// Gets Owner.
        /// </summary>
        protected PropertyEditor Owner { get; private set; }

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The compare to.
        /// </returns>
        public int CompareTo(object obj)
        {
            return this.SortIndex.CompareTo(((ViewModelBase)obj).SortIndex);
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The to string.
        /// </returns>
        public override string ToString()
        {
            return this.Header;
        }

        /// <summary>
        /// The notify property changed.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        protected void NotifyPropertyChanged(string property)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}