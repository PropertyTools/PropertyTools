// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeListBox.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
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
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

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
        /// Queue of items to expand.
        /// </summary>
        /// <remarks>Cannot expand the items when preparing the container, so we use a queue to expand later.</remarks>
        private readonly Queue<TreeListBoxItem> expandItemQueue = new Queue<TreeListBoxItem>();

        /// <summary>
        /// A map from item to parent. This is used to set the Level property when the container of the item is generated.
        /// </summary>
        private readonly Dictionary<object, object> parentMap = new Dictionary<object, object>();

        /// <summary>
        /// A map from item to level.
        /// </summary>
        private readonly Dictionary<object, int> itemLevels = new Dictionary<object, int>();

        /// <summary>
        /// Flags if the TreeListBox is preparing a container.
        /// </summary>
        private bool isPreparingContainer;

        /// <summary>
        /// Subscribed to the Rendering event.
        /// </summary>
        private bool isSubscribedToRenderingEvent;

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
        /// Expands the ancestors of the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void ExpandParents(object item)
        {
            var container = this.GetContainerFromItem(item);
            container.ExpandParents();
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
        /// <param name="parentContainer">The parent container.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        internal void ChildCollectionChanged(TreeListBoxItem parentContainer, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                    this.RemoveItems(e.OldItems);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    this.RemoveItems(this.Items);
                    parentContainer.IsExpanded = false;
                    parentContainer.HasItems = false;
                    break;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                    if (parentContainer.IsExpanded)
                    {
                        this.InsertItems(parentContainer, e.NewItems, e.NewStartingIndex);
                    }

                    break;
            }

            parentContainer.HasItems = parentContainer.Children.Cast<object>().Any();
        }

        /// <summary>
        /// Collapses the specified container.
        /// </summary>
        /// <param name="container">The container to collapse.</param>
        internal void Collapse(TreeListBoxItem container)
        {
            if (container.Children == null)
            {
                return;
            }

            foreach (var child in container.Children)
            {
                var childContainer = this.GetContainerFromItem(child);
                if (childContainer != null)
                {
                    if (childContainer.IsExpanded)
                    {
                        childContainer.IsExpanded = false;
                    }

                    if (childContainer.IsSelected)
                    {
                        childContainer.IsSelected = false;
                    }
                }

                this.RemoveItem(child);
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
        /// Expands the specified container.
        /// </summary>
        /// <param name="container">The container to expand.</param>
        internal void Expand(TreeListBoxItem container)
        {
            if (this.isPreparingContainer)
            {
                // cannot expand when preparing the container
                // add the item to a queue, and expand later
                this.expandItemQueue.Enqueue(container);
                if (!this.isSubscribedToRenderingEvent)
                {
                    this.isSubscribedToRenderingEvent = true;
                    CompositionTarget.Rendering += this.CompositionTargetRendering;
                }

                return;
            }

            if (container.Content == null || container.Children == null)
            {
                return;
            }

            lock (this)
            {
                int i0 = this.Items.IndexOf(container.Content) + 1;
                var parent = container.Content;
                foreach (var child in container.Children)
                {
                    this.InsertItem(i0++, child, parent);
                }
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
            var container = this.SelectedIndex >= 0 ? this.ContainerFromIndex(this.SelectedIndex) : null;

            switch (e.Key)
            {
                case Key.Left:
                    if (control)
                    {
                        foreach (var topLevelItem in this.HierarchySource)
                        {
                            var topLevelContainer = this.GetContainerFromItem(topLevelItem);
                            if (topLevelContainer != null)
                            {
                                topLevelContainer.IsExpanded = false;
                            }
                        }
                    }
                    else
                    {
                        if (container != null)
                        {
                            container.IsExpanded = false;
                        }
                    }

                    e.Handled = true;
                    break;
                case Key.Right:
                    if (control)
                    {
                        // TODO: Expand all?
                    }
                    else
                    {
                        if (container != null && !container.IsExpanded && container.HasItems)
                        {
                            container.IsExpanded = true;
                        }
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
            this.isPreparingContainer = true;
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
            if (!this.parentMap.ContainsKey(item))
            {
                throw new InvalidOperationException(string.Format("Missing parent for item {0}", item));
            }
#endif

            var parent = this.parentMap[item];
            if (parent != null)
            {
                var parentContainer = this.GetContainerFromItem(parent);
#if DEBUG
                if (parentContainer == null)
                {
                    throw new InvalidOperationException(string.Format("Missing parent container for {0}", parent));
                }
#endif
                parentContainer.ChildContainers.Add(container);
                container.ParentContainer = parentContainer;
            }

            // Set the level (this will never be changed)
            container.Level = this.itemLevels[item];

            // Set a binding to the IsSelected property
            if (!string.IsNullOrEmpty(this.IsSelectedPath))
            {
                container.SetBinding(ListBoxItem.IsSelectedProperty, new Binding(this.IsSelectedPath));
            }

            // Set a binding to the Children property
            if (!string.IsNullOrEmpty(this.ChildrenPath))
            {
                container.SetBinding(TreeListBoxItem.ChildrenProperty, new Binding(this.ChildrenPath));
            }

            // Set a binding to the IsExpanded property
            if (!string.IsNullOrEmpty(this.IsExpandedPath))
            {
                container.SetBinding(TreeListBoxItem.IsExpandedProperty, new Binding(this.IsExpandedPath));
            }

            // Update the HasItems property
            container.HasItems = container.Children != null && container.Children.Cast<object>().Any();
            this.isPreparingContainer = false;
        }

        /// <summary>
        /// Handles the Rendering event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            // expand the items in the queue
            while (this.expandItemQueue.Count > 0)
            {
                var item = this.expandItemQueue.Dequeue();
                this.Expand(item);
            }

            // unsubscribe
            CompositionTarget.Rendering -= this.CompositionTargetRendering;
            this.isSubscribedToRenderingEvent = false;
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
        /// <param name="parentContainer">The parent container.</param>
        /// <param name="newItems">The new items.</param>
        /// <param name="newStartingIndex">New index of the starting.</param>
        private void InsertItems(TreeListBoxItem parentContainer, IList newItems, int newStartingIndex)
        {
            // Find the index where the new items should be added
            int i;
            if (newStartingIndex + newItems.Count < parentContainer.Children.Count)
            {
                // inserted items
                var followingChild = parentContainer.Children[newStartingIndex + newItems.Count];
                i = this.Items.IndexOf(followingChild);
            }
            else
            {
                // added items
                var parentSibling = parentContainer.GetNextSibling();
                if (parentSibling == null)
                {
                    // No sibling found, so add at the end of the list.
                    i = this.Items.Count;
                }
                else
                {
                    // Found the sibling, so add the items before this item.
                    i = this.Items.IndexOf(parentSibling.Content);
                }
            }

            if (i < 0)
            {
#if !DEBUG
                throw new InvalidOperationException("Could not get parent index in TreeListBox.");
#else
                System.Diagnostics.Trace.WriteLine("Could not get parent index in TreeListBox.");
#endif
            }

            foreach (var item in newItems)
            {
                this.InsertItem(i, item, parentContainer.Content);
            }
        }

        /// <summary>
        /// Removes the specified items from the tree.
        /// </summary>
        /// <param name="itemsToRemove">The items to remove.</param>
        private void RemoveItems(IList itemsToRemove)
        {
            // collapse expanded containers
            foreach (var container in itemsToRemove.Cast<object>().Select(this.GetContainerFromItem).Where(container => container != null))
            {
                container.IsExpanded = false;
            }

            // remove the items
            foreach (var item in itemsToRemove)
            {
                this.RemoveItem(item);
            }
        }

        /// <summary>
        /// Clears the items.
        /// </summary>
        private void ClearItems()
        {
            this.Items.Clear();
            this.parentMap.Clear();
            this.itemLevels.Clear();
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        private void RemoveItem(object item)
        {
            this.Items.Remove(item);
            this.parentMap.Remove(item);
            this.itemLevels.Remove(item);
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
        /// Inserts the specified item.
        /// </summary>
        /// <param name="i">The index to insert at.</param>
        /// <param name="item">The item.</param>
        /// <param name="parent">The parent of the item.</param>
        private void InsertItem(int i, object item, object parent)
        {
#if DEBUG
            if (this.Items.Contains(item))
            {
                throw new InvalidOperationException("The item should not already be added to the TreeListBox.");
            }
#endif

            this.parentMap[item] = parent;
            this.itemLevels[item] = (parent != null ? this.itemLevels[parent] : 0) + 1;
            this.Items.Insert(i, item);
        }
    }
}