// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeListBoxItem.cs" company="PropertyTools">
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
//   Represents a container for items in the <see cref="TreeListBox" /> .
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Represents a container for items in the <see cref="TreeListBox" /> .
    /// </summary>
    public class TreeListBoxItem : ListBoxItem
    {
        /// <summary>
        /// Identifies the <see cref="Children"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register(
            "Children",
            typeof(IList),
            typeof(TreeListBoxItem),
            new UIPropertyMetadata(null, (s, e) => ((TreeListBoxItem)s).ChildrenChanged(e)));

        /// <summary>
        /// Identifies the <see cref="HasItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasItemsProperty = DependencyProperty.Register(
            "HasItems", typeof(bool), typeof(TreeListBoxItem), new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsDropTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDropTargetProperty = DependencyProperty.Register(
            "IsDropTarget", typeof(bool), typeof(TreeListBoxItem), new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsExpanded"/> dependency property.
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
        /// Identifies the <see cref="LevelPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LevelPaddingProperty = DependencyProperty.Register(
            "LevelPadding", typeof(Thickness), typeof(TreeListBoxItem));

        /// <summary>
        /// Identifies the <see cref="Level"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register(
            "Level",
            typeof(int),
            typeof(TreeListBoxItem),
            new UIPropertyMetadata(0, (s, e) => ((TreeListBoxItem)s).LevelOrIndentationChanged()));

        /// <summary>
        /// The collection changed handler.
        /// </summary>
        private NotifyCollectionChangedEventHandler collectionChangedHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeListBoxItem" /> class.
        /// </summary>
        public TreeListBoxItem()
        {
            // The following is not working when TreeListBoxItems are disconnected...
            // ItemsControl.ItemsControlFromItemContainer(this) as TreeListBox;
            this.ChildContainers = new List<TreeListBoxItem>();
        }

        /// <summary>
        /// Gets the expand toggle command.
        /// </summary>
        /// <value>The command.</value>
        public ICommand ToggleExpandCommand
        {
            get
            {
                return new DelegateCommand(() => this.IsExpanded = !this.IsExpanded);
            }
        }

        /// <summary>
        /// Gets or sets the child items.
        /// </summary>
        /// <value>The children.</value>
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
        /// Gets or sets a value indicating whether this item has child items.
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
        /// Gets or sets a value indicating whether this instance should preview as a drop target.
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
        /// Gets or sets a value indicating whether this instance is expanded.
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
        /// Gets or sets the hierarchy level of the item.
        /// </summary>
        /// <value>The level.</value>
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
        /// Gets the padding due to hierarchy level and the parent control Indentation.
        /// </summary>
        /// <value>The level padding.</value>
        public Thickness LevelPadding
        {
            get
            {
                return (Thickness)this.GetValue(LevelPaddingProperty);
            }

            private set
            {
                this.SetValue(LevelPaddingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the child containers.
        /// </summary>
        /// <value>The child items.</value>
        internal IList<TreeListBoxItem> ChildContainers { get; set; }

        /// <summary>
        /// Gets or sets the parent container.
        /// </summary>
        internal TreeListBoxItem ParentContainer { get; set; }

        /// <summary>
        /// Gets the parent <see cref="TreeListBox" />.
        /// </summary>
        internal TreeListBox ParentTreeListBox
        {
            get
            {
                return (TreeListBox)ItemsControl.ItemsControlFromItemContainer(this);
            }
        }

        /// <summary>
        /// Expands the parents of this item.
        /// </summary>
        public void ExpandParents()
        {
            while (this.ParentContainer != null)
            {
                this.ParentContainer.ExpandParents();
                this.ParentContainer.IsExpanded = true;
            }
        }

        /// <summary>
        /// Gets the next sibling.
        /// </summary>
        /// <returns>
        /// The next sibling.
        /// </returns>
        public TreeListBoxItem GetNextSibling()
        {
            var parentItem = this.ParentContainer;
            if (parentItem == null)
            {
                return null;
            }

            int index = parentItem.Children.IndexOf(this.Content);
            if (index + 1 < parentItem.Children.Count)
            {
                var item = this.ParentTreeListBox.GetContainerFromItem(parentItem.Children[index + 1]);
                return item;
            }

            return parentItem.GetNextSibling();
        }

        /// <summary>
        /// Gets the ancestors of the current container.
        /// </summary>
        /// <param name="includeCurrent">Include the current container if set to <c>true</c>.</param>
        /// <returns>A sequence of containers.</returns>
        public IEnumerable<TreeListBoxItem> GetAncestors(bool includeCurrent)
        {
            if (includeCurrent)
            {
                yield return this;
            }

            var parent = this.ParentContainer;
            while (parent != null)
            {
                yield return parent;
                parent = parent.ParentContainer;
            }
        }

        /// <summary>
        /// Gets the descendants of the current container.
        /// </summary>
        /// <param name="includeCurrent">Include the current container if set to <c>true</c>.</param>
        /// <returns>A sequence of containers.</returns>
        public IEnumerable<TreeListBoxItem> GetDescendants(bool includeCurrent)
        {
            var queue = new Queue<TreeListBoxItem>();
            queue.Enqueue(this);
            if (includeCurrent)
            {
                yield return this;
            }

            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                foreach (var childContainer in c.ChildContainers)
                {
                    yield return childContainer;
                    queue.Enqueue(childContainer);
                }
            }
        }

        /// <summary>
        /// Handles changes in <see cref="Level" /> or <see cref="TreeListBox.Indentation" /> (in the parent control).
        /// </summary>
        internal void LevelOrIndentationChanged()
        {
            this.LevelPadding = new Thickness(this.Level * this.ParentTreeListBox.Indentation, 0, 0, 0);
        }

        /// <summary>
        /// Handles changes in the <see cref="Children" /> property.
        /// </summary>
        /// <param name="e">The event arguments.</param>
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
        /// Handles changes in the <see cref="IsExpanded" /> property.
        /// </summary>
        private void IsExpandedChanged()
        {
            if (this.IsExpanded)
            {
                this.ParentTreeListBox.Expand(this);
            }
            else
            {
                this.ParentTreeListBox.Collapse(this);
            }
        }

        /// <summary>
        /// Subscribes for changes on the specified collection.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="collection">The collection to observe.</param>
        private void SubscribeForCollectionChanges(TreeListBoxItem parent, IEnumerable collection)
        {
            if (this.ParentTreeListBox == null)
            {
                throw new InvalidOperationException("Parent not set.");
            }

            var cc = collection as INotifyCollectionChanged;
            if (cc != null)
            {
                var parentTreeListBox = this.ParentTreeListBox;
                this.collectionChangedHandler = (s, e) => parentTreeListBox.ChildCollectionChanged(parent, e);
                cc.CollectionChanged += this.collectionChangedHandler;
            }
        }

        /// <summary>
        /// Removes the change subscription for the specified collection.
        /// </summary>
        /// <param name="collection">The collection to stop observing.</param>
        private void UnsubscribeCollectionChanges(IEnumerable collection)
        {
            var cc = collection as INotifyCollectionChanged;
            if (cc != null)
            {
                cc.CollectionChanged -= this.collectionChangedHandler;
            }
        }
    }
}