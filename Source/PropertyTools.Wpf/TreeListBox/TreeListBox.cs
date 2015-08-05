// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeListBox.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a hierarchical list box.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// Represents a hierarchical list box.
    /// </summary>
    public class TreeListBox : ListBox
    {
        /// <summary>
        /// Identifies the <see cref="ChildrenPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChildrenPathProperty =
            DependencyProperty.Register(
                "ChildrenPath",
                typeof(string),
                typeof(TreeListBox),
                new UIPropertyMetadata("Children"));

        /// <summary>
        /// Identifies the <see cref="Indentation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IndentationProperty = DependencyProperty.Register(
            "Indentation",
            typeof(double),
            typeof(TreeListBox),
            new UIPropertyMetadata(10.0, (s, e) => ((TreeListBox)s).IndentationChanged()));

        /// <summary>
        /// Identifies the <see cref="IsExpandedPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsExpandedPathProperty = DependencyProperty.Register(
            "IsExpandedPath", typeof(string), typeof(TreeListBox), new UIPropertyMetadata("IsExpanded"));

        /// <summary>
        /// Identifies the <see cref="IsSelectedPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedPathProperty =
            DependencyProperty.Register(
                "IsSelectedPath",
                typeof(string),
                typeof(TreeListBox),
                new UIPropertyMetadata("IsSelected"));

        /// <summary>
        /// Identifies the <see cref="HierarchySource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TreeSourceProperty = DependencyProperty.Register(
            "HierarchySource",
            typeof(IEnumerable),
            typeof(TreeListBox),
            new UIPropertyMetadata(null, (s, e) => ((TreeListBox)s).HierarchySourceChanged(e)));

        /// <summary>
        /// A map from item to parent. This is used to set the Level property when the container of the item is generated.
        /// </summary>
        private readonly Dictionary<object, object> itemToParentMap = new Dictionary<object, object>();

        /// <summary>
        /// A map from item to children. This is used to show the child items.
        /// </summary>
        private readonly Dictionary<object, IList> itemToChildrenMap = new Dictionary<object, IList>();

        /// <summary>
        /// The is expanded map.
        /// </summary>
        private readonly Dictionary<object, bool> isExpanded = new Dictionary<object, bool>();

        /// <summary>
        /// A map from children collection to item.
        /// </summary>
        private readonly Dictionary<IList, object> childrenToItemMap = new Dictionary<IList, object>();

        /// <summary>
        /// A map from item to level. This is used to set the Level property of the containers.
        /// </summary>
        private readonly Dictionary<object, int> itemLevelMap = new Dictionary<object, int>();

        /// <summary>
        /// Initializes static members of the <see cref="TreeListBox" /> class.
        /// </summary>
        static TreeListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeListBox), new FrameworkPropertyMetadata(typeof(TreeListBox)));
        }

        /// <summary>
        /// Gets or sets the binding path to the children of an item.
        /// </summary>
        /// <value>The binding path.</value>
        public string ChildrenPath
        {
            get
            {
                return (string)this.GetValue(ChildrenPathProperty);
            }

            set
            {
                this.SetValue(ChildrenPathProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the hierarchy source.
        /// </summary>
        /// <value>The hierarchy source.</value>
        public IEnumerable HierarchySource
        {
            get
            {
                return (IEnumerable)this.GetValue(TreeSourceProperty);
            }

            set
            {
                this.SetValue(TreeSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the indentation.
        /// </summary>
        /// <value>The indentation.</value>
        public double Indentation
        {
            get
            {
                return (double)this.GetValue(IndentationProperty);
            }

            set
            {
                this.SetValue(IndentationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the binding path to the IsExpanded property of the items.
        /// </summary>
        /// <value>A binding path.</value>
        public string IsExpandedPath
        {
            get
            {
                return (string)this.GetValue(IsExpandedPathProperty);
            }

            set
            {
                this.SetValue(IsExpandedPathProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the binding path to the IsSelected property of the items.
        /// </summary>
        /// <value>A binding path.</value>
        public string IsSelectedPath
        {
            get
            {
                return (string)this.GetValue(IsSelectedPathProperty);
            }

            set
            {
                this.SetValue(IsSelectedPathProperty, value);
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see
        /// cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> .
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.IndentationChanged();
        }

        /// <summary>
        /// Handles child collection changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        internal void ChildCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var children = (IList)sender;
            var item = this.childrenToItemMap[children];

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                    this.RemoveItems(e.OldItems);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    // lookup the items that has been cleared from the item
                    var items = this.itemToParentMap.Where(kvp => kvp.Value == item).Select(kvp => kvp.Key).ToArray();
                    this.RemoveItems(items);
                    break;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                    if (this.isExpanded[item])
                    {
                        this.InsertItems(item, e.NewItems, e.NewStartingIndex);
                    }

                    break;
            }

            var container = this.GetContainerFromItem(item);
            if (container != null)
            {
                // Update the HasItems flag
                container.HasItems = children != null && children.Cast<object>().Any();
            }
        }

        /// <summary>
        /// Gets the container from the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// The container.
        /// </returns>
        internal TreeListBoxItem ContainerFromIndex(int index)
        {
            return this.ItemContainerGenerator.ContainerFromIndex(index) as TreeListBoxItem;
        }

        /// <summary>
        /// Collapses the specified item.
        /// </summary>
        /// <param name="item">The item to collapse.</param>
        internal void Collapse(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (!this.isExpanded[item])
            {
                return;
            }

            var children = this.GetChildren(item);
            if (children == null)
            {
                throw new InvalidOperationException();
            }

            this.RemoveItems(children);

            this.isExpanded[item] = false;

            var container = this.GetContainerFromItem(item);
            if (container != null)
            {
                // Update the IsExpanded flag
                container.IsExpanded = false;
            }
        }

        /// <summary>
        /// Expands the specified item.
        /// </summary>
        /// <param name="item">The item to expand.</param>
        internal void Expand(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (this.isExpanded[item])
            {
                return;
            }

            var children = this.GetChildren(item);
            if (children == null)
            {
                return;
            }

            this.InsertItems(item, children, 0);
            this.isExpanded[item] = true;

            var container = this.GetContainerFromItem(item);
            if (container != null)
            {
                // Update the IsExpanded flag
                container.IsExpanded = true;
            }
        }

        /// <summary>
        /// Gets the container from an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// The container.
        /// </returns>
        internal TreeListBoxItem GetContainerFromItem(object item)
        {
            return (TreeListBoxItem)this.ItemContainerGenerator.ContainerFromItem(item);
        }

        /// <summary>
        /// Creates or identifies the element used to display a specified item.
        /// </summary>
        /// <returns>
        /// A <see cref="TreeListBoxItem" /> .
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeListBoxItem();
        }

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own ItemContainer.
        /// </summary>
        /// <param name="item">Specified item.</param>
        /// <returns>
        /// <c>true</c> if the item is its own ItemContainer; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListBoxItem;
        }

        /// <summary>
        /// Responds to the <see cref="E:System.Windows.UIElement.KeyDown" /> event.
        /// </summary>
        /// <param name="e">Provides data for <see cref="T:System.Windows.Input.KeyEventArgs" /> .</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            var control = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            var item = this.SelectedIndex >= 0 ? this.Items[this.SelectedIndex] : null;

            switch (e.Key)
            {
                case Key.Left:
                    if (control)
                    {
                        // Collapse all items
                        foreach (var topLevelItem in this.HierarchySource)
                        {
                            this.Collapse(topLevelItem);
                        }
                    }
                    else
                    {
                        // Collapse the selected item
                        this.Collapse(item);
                    }

                    e.Handled = true;
                    break;
                case Key.Right:
                    if (control)
                    {
                        // Expand all items
                        foreach (var i in this.Items.Cast<object>().ToArray())
                        {
                            this.Expand(i);
                        }
                    }
                    else
                    {
                        // Expand the selected item
                        this.Expand(item);
                    }

                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">Container used to display the specified item.</param>
        /// <param name="item">The item to display.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var container = (TreeListBoxItem)element;
            if (container == null)
            {
                throw new InvalidOperationException("Container not created.");
            }

            if (item == null)
            {
                throw new InvalidOperationException("Item not specified.");
            }

#if DEBUG
            if (!this.itemToParentMap.ContainsKey(item))
            {
                throw new InvalidOperationException(string.Format("Missing parent for item {0}", item));
            }
#endif

            // Set the level (this will never be changed)
            container.Level = this.itemLevelMap[item];

            // Set a binding to the IsSelected property
            if (!string.IsNullOrEmpty(this.IsSelectedPath))
            {
                container.SetBinding(ListBoxItem.IsSelectedProperty, new Binding(this.IsSelectedPath));
            }

            // Set a binding to the IsExpanded property
            if (!string.IsNullOrEmpty(this.IsExpandedPath))
            {
                container.SetBinding(TreeListBoxItem.IsExpandedProperty, new Binding(this.IsExpandedPath));
            }

            var children = this.itemToChildrenMap[item];
            container.HasItems = children != null && children.Cast<object>().Any();
        }

        /// <summary>
        /// Provides an appropriate <see cref="T:System.Windows.Automation.Peers.ListBoxAutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.
        /// </summary>
        /// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new TreeListBoxAutomationPeer(this);
        }

        /// <summary>
        /// Subscribes for changes on the specified collection.
        /// </summary>
        /// <param name="collection">The collection to observe.</param>
        private void SubscribeForCollectionChanges(IList collection)
        {
            var cc = collection as INotifyCollectionChanged;
            if (cc != null)
            {
                cc.CollectionChanged += this.ChildCollectionChanged;
            }
        }

        /// <summary>
        /// Removes the change subscription for the specified collection.
        /// </summary>
        /// <param name="collection">The collection to stop observing.</param>
        private void UnsubscribeCollectionChanges(IList collection)
        {
            var cc = collection as INotifyCollectionChanged;
            if (cc != null)
            {
                cc.CollectionChanged -= this.ChildCollectionChanged;
            }
        }

        /// <summary>
        /// Handles changes in the HierarchySource.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private void HierarchySourceChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldTreeSource = e.OldValue as IEnumerable;
            if (oldTreeSource != null)
            {
                foreach (var item in oldTreeSource)
                {
                    var container = this.GetContainerFromItem(item);
                    if (container == null)
                    {
                        continue;
                    }

                    if (container.IsSelected)
                    {
                        this.SelectedItems.Remove(item);
                    }
                }
            }

            this.ClearItems();

            var hierarchySource = this.HierarchySource;
            if (hierarchySource != null)
            {
                foreach (var item in hierarchySource)
                {
                    this.AddItem(item, null);
                }
            }
        }

        /// <summary>
        /// Handles changes in indentation.
        /// </summary>
        private void IndentationChanged()
        {
            foreach (var item in this.Items)
            {
                var container = this.GetContainerFromItem(item);
                if (container == null)
                {
                    continue;
                }

                container.LevelOrIndentationChanged();
            }
        }

        /// <summary>
        /// Inserts the specified items in the tree.
        /// </summary>
        /// <param name="parent">The parent item.</param>
        /// <param name="newItems">The new items.</param>
        /// <param name="newStartingIndex">The starting index of the new items.</param>
        /// <exception cref="System.InvalidOperationException">Could not get parent index in TreeListBox.</exception>
        private void InsertItems(object parent, IList newItems, int newStartingIndex)
        {
            var parentChildren = this.GetChildren(parent);

            // Find the index where the new items should be added
            int index;
            if (newStartingIndex + newItems.Count < parentChildren.Count)
            {
                // inserted items should be added just before the next item in the collection
                // note that the items have already been added to the collection, so we need to add the newItems.Count
                var followingChild = parentChildren[newStartingIndex + newItems.Count];
                index = this.Items.IndexOf(followingChild);
            }
            else
            {
                // added items should be added before the next sibling of the parent
                var parentSibling = this.GetNextParentSibling(parent);
                if (parentSibling == null)
                {
                    // No sibling found, so add at the end of the list.
                    index = this.Items.Count;
                }
                else
                {
                    // Found the sibling, so add the items before this item.
                    index = this.Items.IndexOf(parentSibling);
                }
            }

            if (index < 0)
            {
                throw new InvalidOperationException("Could not get parent index in TreeListBox.");
            }

            foreach (var item in newItems)
            {
                this.InsertItem(index++, item, parent);
            }
        }

        /// <summary>
        /// Gets the children of the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A list of children.</returns>
        private IList GetChildren(object item)
        {
            IList children;
            if (this.itemToChildrenMap.TryGetValue(item, out children))
            {
                return children;
            }

            return null;
        }

        /// <summary>
        /// Gets the next sibling to the parent of the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The sibling item.</returns>
        private object GetNextParentSibling(object item)
        {
            var parentItem = this.itemToParentMap[item];
            if (parentItem == null)
            {
                return null;
            }

            var parentChildren = this.itemToChildrenMap[parentItem];

            int index = parentChildren.IndexOf(item);
            if (index + 1 < parentChildren.Count)
            {
                return parentChildren[index + 1];
            }

            return this.GetNextParentSibling(parentItem);
        }

        /// <summary>
        /// Removes the specified items from the tree.
        /// </summary>
        /// <param name="itemsToRemove">The items to remove.</param>
        private void RemoveItems(IList itemsToRemove)
        {
            var queue = new Queue(itemsToRemove);
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (this.Items.Contains(item))
                {
                    if (this.isExpanded[item])
                    {
                        IList children;
                        if (this.itemToChildrenMap.TryGetValue(item, out children) && children != null)
                        {
                            foreach (var child in children)
                            {
                                queue.Enqueue(child);
                            }
                        }
                    }

                    this.RemoveItem(item);
                }
            }
        }

        /// <summary>
        /// Clears the items.
        /// </summary>
        private void ClearItems()
        {
            foreach (var children in this.childrenToItemMap.Keys)
            {
                this.UnsubscribeCollectionChanges(children);
            }

            this.Items.Clear();
            this.itemToParentMap.Clear();
            this.itemToChildrenMap.Clear();
            this.childrenToItemMap.Clear();
            this.itemLevelMap.Clear();
            this.isExpanded.Clear();
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        private void RemoveItem(object item)
        {
            var container = this.GetContainerFromItem(item);
            if (container != null)
            {
                container.IsSelected = false;
            }

            this.Items.Remove(item);
            this.itemToParentMap.Remove(item);
            var children = this.itemToChildrenMap[item];
            this.UnsubscribeCollectionChanges(children);
            this.itemToChildrenMap.Remove(item);
            this.childrenToItemMap.Remove(children);
            this.itemLevelMap.Remove(item);
            this.isExpanded.Remove(item);
        }

        /// <summary>
        /// Inserts the specified item.
        /// </summary>
        /// <param name="index">The index to insert the item at.</param>
        /// <param name="item">The item.</param>
        /// <param name="parent">The parent of the item.</param>
        private void InsertItem(int index, object item, object parent)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

#if DEBUG
            if (this.Items.Contains(item))
            {
                throw new InvalidOperationException("The item is already be added to the TreeListBox.");
            }
#endif

            this.itemToParentMap[item] = parent;

            // Reflect to get the children collection
            var children = this.GetChildrenCollectionByReflection(item);
            this.itemToChildrenMap[item] = children;
            if (this.childrenToItemMap.ContainsKey(children))
            {
                throw new InvalidOperationException("Children collection already observed.");
            }

            this.childrenToItemMap[children] = item;

            this.SubscribeForCollectionChanges(children);
            this.itemLevelMap[item] = (parent != null ? this.itemLevelMap[parent] : -1) + 1;
            this.isExpanded[item] = false;
            this.Items.Insert(index, item);
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="parent">The parent.</param>
        private void AddItem(object item, object parent)
        {
            this.InsertItem(this.Items.Count, item, parent);
        }

        /// <summary>
        /// Gets the children collection of the specified item by reflection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A list of children.</returns>
        private IList GetChildrenCollectionByReflection(object item)
        {
            var pi = item.GetType().GetProperty(this.ChildrenPath);
            var children = (IList)pi.GetValue(item, null);
            return children;
        }
    }
}