// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsBag.cs" company="PropertyTools">
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
//   Represents a bag of items.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    /// <summary>
    /// Represents a bag of items.
    /// </summary>
    [TypeDescriptionProvider(typeof(ItemsBagTypeDescriptionProvider))]
    public class ItemsBag : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsBag"/> class.
        /// </summary>
        /// <param name="objects">
        /// The objects.
        /// </param>
        public ItemsBag(IEnumerable objects)
        {
            this.Objects = objects;
            this.BiggestType = TypeHelper.FindBiggestCommonType(objects);
            this.Subscribe();
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the biggest common type of the objects.
        /// </summary>
        /// <value>The type of the biggest.</value>
        public Type BiggestType { get; private set; }

        /// <summary>
        /// Gets the objects in the bag.
        /// </summary>
        /// <value>The objects.</value>
        public IEnumerable Objects { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to suspend property changed notifications.
        /// </summary>
        /// <value><c>true</c> if notifications are suspended; otherwise, <c>false</c>.</value>
        public bool SuspendNotifications { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Unsubscribe();
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        internal void RaisePropertyChanged(string property)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        /// <summary>
        /// The relay property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RelayPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.SuspendNotifications)
            {
                return;
            }

            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Subscribes to property changed notifications.
        /// </summary>
        private void Subscribe()
        {
            foreach (var o in this.Objects)
            {
                var onpc = o as INotifyPropertyChanged;
                if (onpc != null)
                {
                    onpc.PropertyChanged += this.RelayPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Unsubscribes to property changed notifications.
        /// </summary>
        private void Unsubscribe()
        {
            foreach (var o in this.Objects)
            {
                var onpc = o as INotifyPropertyChanged;
                if (onpc != null)
                {
                    onpc.PropertyChanged -= this.RelayPropertyChanged;
                }
            }
        }

    }
}