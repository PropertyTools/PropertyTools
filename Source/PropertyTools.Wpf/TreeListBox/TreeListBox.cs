﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeListBox.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
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

    /// <summary>
    /// Represents a hierarchical list box.
    /// </summary>
    public class TreeListBox : ListBox
    {
        #region Constants and Fields

        /// <summary>
        ///   The hierarchy items binding property.
        /// </summary>
        public static readonly DependencyProperty ChildrenBindingProperty =
            DependencyProperty.Register(
                "ChildrenBinding", 
                typeof(BindingBase), 
                typeof(TreeListBox), 
                new UIPropertyMetadata(new Binding("Children")));

        /// <summary>
        ///   The indentation property.
        /// </summary>
        public static readonly DependencyProperty IndentationProperty = DependencyProperty.Register(
            "Indentation", 
            typeof(double), 
            typeof(TreeListBox), 
            new UIPropertyMetadata(10.0, (s, e) => ((TreeListBox)s).IndentationChanged()));

        /// <summary>
        ///   The is expanded binding property.
        /// </summary>
        public static readonly DependencyProperty IsExpandedBindingProperty =
            DependencyProperty.Register(
                "IsExpandedBinding", 
                typeof(BindingBase), 
                typeof(TreeListBox), 
                new UIPropertyMetadata(new Binding("IsExpanded")));

        /// <summary>
        ///   The is selected binding property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedBindingProperty =
            DependencyProperty.Register(
                "IsSelectedBinding", 
                typeof(BindingBase), 
                typeof(TreeListBox), 
                new UIPropertyMetadata(new Binding("IsSelected")));

        /// <summary>
        ///   The tree source property.
        /// </summary>
        public static readonly DependencyProperty TreeSourceProperty = DependencyProperty.Register(
            "HierarchySource", 
            typeof(IEnumerable), 
            typeof(TreeListBox), 
            new UIPropertyMetadata(null, (s, e) => ((TreeListBox)s).HierarchySourceChanged(e)));

        /// <summary>
        ///   A map from item to parent container. This is used to set the Level property of the item once its container is generated.
        /// </summary>
        private readonly Dictionary<object, TreeListBoxItem> parentContainerMap =
            new Dictionary<object, TreeListBoxItem>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "TreeListBox" /> class.
        /// </summary>
        static TreeListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TreeListBox), new FrameworkPropertyMetadata(typeof(TreeListBox)));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the hierarchal items binding.
        /// </summary>
        /// <value>The hierarchy items binding.</value>
        public BindingBase ChildrenBinding
        {
            get
            {
                return (BindingBase)this.GetValue(ChildrenBindingProperty);
            }

            set
            {
                this.SetValue(ChildrenBindingProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the hierarchy source.
        /// </summary>
        /// <value>
        ///   The hierarchy source.
        /// </value>
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
        ///   Gets or sets the indentation.
        /// </summary>
        /// <value>
        ///   The indentation.
        /// </value>
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
        ///   Gets or sets the IsExpanded binding.
        /// </summary>
        /// <value>The is expanded binding.</value>
        public BindingBase IsExpandedBinding
        {
            get
            {
                return (BindingBase)this.GetValue(IsExpandedBindingProperty);
            }

            set
            {
                this.SetValue(IsExpandedBindingProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the IsSelected binding.
        /// </summary>
        /// <value>The is selected binding.</value>
        public BindingBase IsSelectedBinding
        {
            get
            {
                return (BindingBase)this.GetValue(IsSelectedBindingProperty);
            }

            set
            {
                this.SetValue(IsSelectedBindingProperty, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.IndentationChanged();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles child collection changes.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.
        /// </param>
        internal void ChildCollectionChanged(TreeListBoxItem parent, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var container = this.ContainerFromItem(item);
                        if (container.IsExpanded)
                        {
                            container.IsExpanded = false;
                        }

                        this.Items.Remove(item);
                    }

                    break;

                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Reset:
                    var itemsToRemove = new List<TreeListBoxItem>();

                    foreach (var item in this.Items)
                    {
                        var container = this.ContainerFromItem(item);
                        if (container == null || container.ParentItem != parent)
                        {
                            continue;
                        }

                        itemsToRemove.Add(container);
                    }

                    foreach (var container in itemsToRemove)
                    {
                        if (container.IsExpanded)
                        {
                            container.IsExpanded = false;
                        }

                        this.Items.Remove(container.Content);
                    }

                    parent.IsExpanded = false;
                    parent.HasItems = false;
                    break;
                case NotifyCollectionChangedAction.Add:
                    if (parent.IsExpanded)
                    {
                        int i;
                        if (e.NewStartingIndex + 1 < parent.Children.Count)
                        {
                            var item0 = parent.Children[e.NewStartingIndex + 1];
                            var i0 = this.Items.IndexOf(item0);
                            i = i0;
                        }
                        else
                        {
                            var item0 = parent.Children[e.NewStartingIndex - 1];
                            var i0 = this.Items.IndexOf(item0);
                            i = i0 + 1;
                        }

                        foreach (var item in e.NewItems)
                        {
                            this.parentContainerMap[item] = parent;
                            this.Items.Insert(i++, item);
                        }
                    }

                    break;
            }

            parent.HasItems = parent.Children.Cast<object>().Any();
        }

        /// <summary>
        /// Collapses the specified item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        internal void Collapse(TreeListBoxItem item)
        {
            if (item.Children == null)
            {
                return;
            }

            foreach (var child in item.Children)
            {
                var childContainer = this.ContainerFromItem(child);
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

                this.Items.Remove(child);
            }
        }

        /// <summary>
        /// Gets the container from and index.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The container.
        /// </returns>
        internal TreeListBoxItem ContainerFromIndex(int index)
        {
            return this.ItemContainerGenerator.ContainerFromIndex(index) as TreeListBoxItem;
        }

        /// <summary>
        /// Expands the specified item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        internal void Expand(TreeListBoxItem item)
        {
            if (item.Content == null || item.Children == null)
            {
                return;
            }

            int i0 = this.Items.IndexOf(item.Content) + 1;
            foreach (var child in item.Children)
            {
                this.Items.Insert(i0++, child);
                this.parentContainerMap[child] = item;
            }
        }

        /// <summary>
        /// Creates or identifies the element used to display a specified item.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Windows.Controls.ListBoxItem"/>.
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeListBoxItem(this);
        }

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own ItemContainer.
        /// </summary>
        /// <param name="item">
        /// Specified item.
        /// </param>
        /// <returns>
        /// true if the item is its own ItemContainer; otherwise, false.
        /// </returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListBoxItem;
        }

        /// <summary>
        /// Responds to the <see cref="E:System.Windows.UIElement.KeyDown"/> event.
        /// </summary>
        /// <param name="e">
        /// Provides data for <see cref="T:System.Windows.Input.KeyEventArgs"/>.
        /// </param>
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
                            var topLevelContainer = this.ContainerFromItem(topLevelItem);
                            if (topLevelContainer.IsExpanded)
                            {
                                topLevelContainer.IsExpanded = false;
                            }
                        }
                    }
                    else
                    {
                        if (container != null && container.IsExpanded)
                        {
                            container.IsExpanded = false;
                        }
                    }

                    e.Handled = true;
                    break;
                case Key.Right:
                    if (container != null && !container.IsExpanded && container.HasItems)
                    {
                        container.IsExpanded = true;
                    }

                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.Control.MouseDoubleClick"/> routed event.
        /// </summary>
        /// <param name="e">
        /// The event data.
        /// </param>
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            var container = this.SelectedIndex >= 0 ? this.ContainerFromIndex(this.SelectedIndex) : null;
            if (container != null && container.HasItems)
            {
                container.IsExpanded = !container.IsExpanded;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">
        /// Element used to display the specified item.
        /// </param>
        /// <param name="item">
        /// Specified item.
        /// </param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var hli = (TreeListBoxItem)element;
            if (hli != null && item != null)
            {
                hli.ParentItem = this.parentContainerMap[item];
                this.parentContainerMap.Remove(hli);
                hli.Level = hli.ParentItem != null ? hli.ParentItem.Level + 1 : 0;
                hli.SetBinding(ListBoxItem.IsSelectedProperty, this.IsSelectedBinding);
                hli.SetBinding(TreeListBoxItem.IsExpandedProperty, this.IsExpandedBinding);
                hli.SetBinding(TreeListBoxItem.ChildrenProperty, this.ChildrenBinding);
                hli.HasItems = hli.Children != null && hli.Children.Cast<object>().Count() > 0;
            }
        }

        /// <summary>
        /// Gets the container from an item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The container.
        /// </returns>
        private TreeListBoxItem ContainerFromItem(object item)
        {
            return this.ItemContainerGenerator.ContainerFromItem(item) as TreeListBoxItem;
        }

        /// <summary>
        /// Handles changes in the HierarchySource.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void HierarchySourceChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldTreeSource = e.OldValue as IEnumerable;
            if (oldTreeSource != null)
            {
                foreach (var item in oldTreeSource)
                {
                    var container = this.ContainerFromItem(item);
                    if (container.IsExpanded)
                    {
                        container.IsExpanded = false;
                    }

                    if (container.IsSelected)
                    {
                        this.SelectedItems.Remove(item);
                    }
                }
            }

            this.Items.Clear();

            if (this.HierarchySource != null)
            {
                foreach (var item in this.HierarchySource)
                {
                    this.Items.Add(item);
                    this.parentContainerMap[item] = null;
                }
            }
        }

        /// <summary>
        /// Handles changes in indentation.
        /// </summary>
        private void IndentationChanged()
        {
            var converter = this.TryFindResource("LevelToThicknessConverter") as LevelToThicknessConverter;
            if (converter != null)
            {
                converter.Indentation = this.Indentation;
            }
        }

        #endregion
    }
}