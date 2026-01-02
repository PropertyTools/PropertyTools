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
    /// Represents a bag of items that provides a unified interface for editing properties of multiple objects.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="ItemsBag"/> class works together with custom type descriptors (<see cref="ItemsBagTypeDescriptor"/>)
    /// and property descriptors (<see cref="ItemsBagPropertyDescriptor"/>) to provide a way to edit multiple objects
    /// simultaneously through a single interface.
    /// </para>
    /// <para>
    /// When properties have different values across the objects in the bag (indeterminate state), the property value is
    /// represented as <c>null</c> when using reflection to get property values. For value types, this requires changing
    /// the property type to a nullable version of the type. For example, a <see cref="bool"/> property becomes <see cref="Nullable{Boolean}"/>.
    /// </para>
    /// <para>
    /// It is important that controls used to edit the properties support these nullable types. The controls must be able to:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Display and handle <c>null</c> values for value types (indeterminate state)</description></item>
    /// <item><description>Allow setting a specific value to all objects in the bag</description></item>
    /// <item><description>Support the nullable versions of value types</description></item>
    /// </list>
    /// </remarks>
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