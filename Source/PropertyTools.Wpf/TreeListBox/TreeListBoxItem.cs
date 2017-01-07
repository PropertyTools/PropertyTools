// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeListBoxItem.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a container for items in the TreeListBox .
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Represents a container for items in the <see cref="TreeListBox" /> .
    /// </summary>
    public class TreeListBoxItem : ListBoxItem
    {
        /// <summary>
        /// Identifies the <see cref="HasItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasItemsProperty = DependencyProperty.Register(
            nameof(HasItems),
            typeof(bool),
            typeof(TreeListBoxItem),
            new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsDropTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDropTargetProperty = DependencyProperty.Register(
            nameof(IsDropTarget),
            typeof(bool),
            typeof(TreeListBoxItem),
            new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsExpanded"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            nameof(IsExpanded),
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
            nameof(LevelPadding),
            typeof(Thickness),
            typeof(TreeListBoxItem));

        /// <summary>
        /// Identifies the <see cref="Level"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register(
            nameof(Level),
            typeof(int),
            typeof(TreeListBoxItem),
            new UIPropertyMetadata(0, (s, e) => ((TreeListBoxItem)s).LevelOrIndentationChanged()));

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
        /// Handles changes in <see cref="Level" /> or <see cref="TreeListBox.Indentation" /> (in the parent control).
        /// </summary>
        internal void LevelOrIndentationChanged()
        {
            this.LevelPadding = new Thickness(this.Level * this.ParentTreeListBox.Indentation, 0, 0, 0);
        }

        /// <summary>
        /// Handles changes in the <see cref="IsExpanded" /> property.
        /// </summary>
        private void IsExpandedChanged()
        {
            if (this.IsExpanded)
            {
                this.ParentTreeListBox.Expand(this.Content);
            }
            else
            {
                this.ParentTreeListBox.Collapse(this.Content);
            }
        }
    }
}