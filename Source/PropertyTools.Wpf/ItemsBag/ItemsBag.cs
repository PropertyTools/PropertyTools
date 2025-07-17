// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsBag.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
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
    using System.Linq;

    /// <summary>
    /// Represents a bag of items.
    /// </summary>
    [TypeDescriptionProvider(typeof(ItemsBagTypeDescriptionProvider))]
    public class ItemsBag : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsBag" /> class.
        /// </summary>
        /// <param name="objects">The objects.</param>
        public ItemsBag(IEnumerable objects)
        {
            this.Objects = objects as object[] ?? objects.Cast<object>().ToArray();
            this.BiggestType = TypeHelper.FindBiggestCommonType(this.Objects);
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
        public object[] Objects { get; private set; }

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
        /// <param name="property">The property.</param>
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
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void RelayPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Adds subscriptions to property changed notifications.
        /// </summary>
        private void Subscribe()
        {
            foreach (var o in this.Objects)
            {
                var on = o as INotifyPropertyChanged;
                if (on != null)
                {
                    on.PropertyChanged += this.RelayPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Removes the subscriptions to property changed notifications.
        /// </summary>
        private void Unsubscribe()
        {
            foreach (var o in this.Objects)
            {
                var on = o as INotifyPropertyChanged;
                if (on != null)
                {
                    on.PropertyChanged -= this.RelayPropertyChanged;
                }
            }
        }
    }
}