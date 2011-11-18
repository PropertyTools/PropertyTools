// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
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
        #region Constructors and Destructors

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

        #endregion

        #region Public Events

        /// <summary>
        ///   The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets Header.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        ///   Gets or sets SortIndex.
        /// </summary>
        public int SortIndex { get; set; }

        /// <summary>
        ///   Gets or sets ToolTip.
        /// </summary>
        public object ToolTip { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets Owner.
        /// </summary>
        protected PropertyEditor Owner { get; private set; }

        #endregion

        #region Public Methods

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

        #endregion

        #region Methods

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

        #endregion
    }
}