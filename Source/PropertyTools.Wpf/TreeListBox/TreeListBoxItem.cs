// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeListBoxItem.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents a container for items in the <see cref="TreeListBox" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a container for items in the <see cref="TreeListBox"/>.
    /// </summary>
    public class TreeListBoxItem : ListBoxItem
    {
        #region Constants and Fields

        /// <summary>
        ///   The children property.
        /// </summary>
        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register(
            "Children",
            typeof(IList),
            typeof(TreeListBoxItem),
            new UIPropertyMetadata(null, (s, e) => ((TreeListBoxItem)s).ChildrenChanged(e)));

        /// <summary>
        ///   The has items property.
        /// </summary>
        public static readonly DependencyProperty HasItemsProperty = DependencyProperty.Register(
            "HasItems", typeof(bool), typeof(TreeListBoxItem), new UIPropertyMetadata(false));

        /// <summary>
        ///   The is drop target property.
        /// </summary>
        public static readonly DependencyProperty IsDropTargetProperty = DependencyProperty.Register(
            "IsDropTarget", typeof(bool), typeof(TreeListBoxItem), new UIPropertyMetadata(false));

        /// <summary>
        ///   The is expanded property.
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded",
            typeof(bool),
            typeof(TreeListBoxItem),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (s, e) => ((TreeListBoxItem)s).IsExpandedChanged()));

        /// <summary>
        ///   The level property.
        /// </summary>
        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register(
            "Level", typeof(int), typeof(TreeListBoxItem), new UIPropertyMetadata(0));

        /// <summary>
        ///   The handler.
        /// </summary>
        private NotifyCollectionChangedEventHandler collectionChangedHandler;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeListBoxItem"/> class.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public TreeListBoxItem(TreeListBox owner)
        {
            this.Owner = owner;
            this.ChildItems = new List<TreeListBoxItem>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the child items.
        /// </summary>
        /// <value>
        ///   The children.
        /// </value>
        public IList Children
        {
            get
            {
                return (IList)this.GetValue(ChildrenProperty);
            }

            set
            {
                this.SetValue(ChildrenProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this item has child items.
        /// </summary>
        public bool HasItems
        {
            get
            {
                return (bool)this.GetValue(HasItemsProperty);
            }

            set
            {
                this.SetValue(HasItemsProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance should preview as a drop target.
        /// </summary>
        public bool IsDropTarget
        {
            get
            {
                return (bool)this.GetValue(IsDropTargetProperty);
            }

            set
            {
                this.SetValue(IsDropTargetProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return (bool)this.GetValue(IsExpandedProperty);
            }

            set
            {
                this.SetValue(IsExpandedProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the hierarchy level of the item.
        /// </summary>
        /// <value>
        ///   The level.
        /// </value>
        public int Level
        {
            get
            {
                return (int)this.GetValue(LevelProperty);
            }

            set
            {
                this.SetValue(LevelProperty, value);
            }
        }

        /// <summary>
        ///   Gets the owner listbox.
        /// </summary>
        public TreeListBox Owner { get; private set; }

        /// <summary>
        ///   Gets the parent item.
        /// </summary>
        internal TreeListBoxItem ParentItem { get; set; }
        internal IList<TreeListBoxItem> ChildItems { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The children changed.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ChildrenChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                this.UnsubscribeCollectionChanges(e.OldValue as IEnumerable);
            }

            if (e.NewValue != null)
            {
                this.SubscribeForCollectionChanges(this, e.NewValue as IEnumerable);
            }
        }

        /// <summary>
        /// Handles IsExpanded changes.
        /// </summary>
        private void IsExpandedChanged()
        {
            if (this.IsExpanded)
            {
                this.Owner.Expand(this);
            }
            else
            {
                this.Owner.Collapse(this);
            }
        }

        /// <summary>
        /// The subscribe for collection changes.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        private void SubscribeForCollectionChanges(TreeListBoxItem parent, IEnumerable collection)
        {
            var cc = collection as INotifyCollectionChanged;
            if (cc != null)
            {
                this.collectionChangedHandler = (s, e) => this.Owner.ChildCollectionChanged(parent, e);
                cc.CollectionChanged += this.collectionChangedHandler;
            }
        }

        /// <summary>
        /// The unsubscribe collection changes.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        private void UnsubscribeCollectionChanges(IEnumerable collection)
        {
            var cc = collection as INotifyCollectionChanged;
            if (cc != null)
            {
                cc.CollectionChanged -= this.collectionChangedHandler;
            }
        }

        #endregion

        public TreeListBoxItem GetNextSibling()
        {
            if (ParentItem == null) return null;
            int index = ParentItem.ChildItems.IndexOf(this);
            if (index + 1 < ParentItem.ChildItems.Count) return ParentItem.ChildItems[index + 1];
            return null;
        }
    }
}