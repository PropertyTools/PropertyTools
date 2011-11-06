// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsBag.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
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
        #region Constructors and Destructors

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

        #endregion

        #region Public Events

        /// <summary>
        ///   Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the biggest common type of the objects.
        /// </summary>
        /// <value>The type of the biggest.</value>
        public Type BiggestType { get; private set; }

        /// <summary>
        ///   Gets the objects in the bag.
        /// </summary>
        /// <value>The objects.</value>
        public IEnumerable Objects { get; private set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to suspend property changed notifications.
        /// </summary>
        /// <value><c>true</c> if notifications are suspended; otherwise, <c>false</c>.</value>
        public bool SuspendNotifications { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Unsubscribe();
        }

        #endregion

        #region Methods

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

            // Debug.WriteLine("ItemsBag.RelayPropertyChanged on " + e.PropertyName);
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

        #endregion
    }
}