// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeListBoxDragDropHelper.cs" company="PropertyTools">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
//   Drag/drop helper class for the <see cref="TreeListBox"/>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Drag/drop helper class for the <see cref="TreeListBox"/>.
    /// </summary>
    /// <remarks>
    /// Based on http://bea.stollnitz.com/blog/?p=53
    /// </remarks>
    public class TreeListBoxDragDropHelper
    {
        /// <summary>
        /// The drag drop template property.
        /// </summary>
        public static readonly DependencyProperty DragDropTemplateProperty =
            DependencyProperty.RegisterAttached(
                "DragDropTemplate",
                typeof(DataTemplate),
                typeof(TreeListBoxDragDropHelper),
                new UIPropertyMetadata(null));

        /// <summary>
        /// The is drag source property.
        /// </summary>
        public static readonly DependencyProperty IsDragSourceProperty =
            DependencyProperty.RegisterAttached(
                "IsDragSource",
                typeof(bool),
                typeof(TreeListBoxDragDropHelper),
                new UIPropertyMetadata(false, IsDragSourceChanged));

        /// <summary>
        /// The is drop target property.
        /// </summary>
        public static readonly DependencyProperty IsDropTargetProperty =
            DependencyProperty.RegisterAttached(
                "IsDropTarget",
                typeof(bool),
                typeof(TreeListBoxDragDropHelper),
                new UIPropertyMetadata(false, IsDropTargetChanged));

        /// <summary>
        /// The instance.
        /// </summary>
        private static TreeListBoxDragDropHelper instance;

        /// <summary>
        /// The format.
        /// </summary>
        private readonly DataFormat format = DataFormats.GetDataFormat("TreeListBox");

        /// <summary>
        /// The dragged data.
        /// </summary>
        private IList draggedData;

        /// <summary>
        /// The drop position.
        /// </summary>
        private DropPosition dropPosition;

        /// <summary>
        /// The has vertical orientation.
        /// </summary>
        private bool hasVerticalOrientation;

        /// <summary>
        /// The initial mouse position.
        /// </summary>
        private Point initialMousePosition;

        /// <summary>
        /// The insertion adorner.
        /// </summary>
        private InsertionAdorner insertionAdorner;

        /// <summary>
        /// The is in first half.
        /// </summary>
        private bool isInFirstHalf;

        /// <summary>
        /// The source item container.
        /// </summary>
        private TreeListBoxItem sourceItemContainer;

        /// <summary>
        /// The source items control.
        /// </summary>
        private TreeListBox sourceItemsControl;

        // target

        /// <summary>
        /// The target item container.
        /// </summary>
        private TreeListBoxItem targetItemContainer;

        /// <summary>
        /// The target items control.
        /// </summary>
        private TreeListBox targetItemsControl;

        /// <summary>
        /// The top window.
        /// </summary>
        private Window topWindow;

        /// <summary>
        /// Gets the instance singleton.
        /// </summary>
        private static TreeListBoxDragDropHelper Instance
        {
            get
            {
                return instance ?? (instance = new TreeListBoxDragDropHelper());
            }
        }

        /// <summary>
        /// Gets the drag/drop template.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// A data template.
        /// </returns>
        public static DataTemplate GetDragDropTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(DragDropTemplateProperty);
        }

        /// <summary>
        /// Gets the IsDragSource value.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The value.
        /// </returns>
        public static bool GetIsDragSource(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragSourceProperty);
        }

        /// <summary>
        /// Gets the IsDropTarget value.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The value.
        /// </returns>
        public static bool GetIsDropTarget(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDropTargetProperty);
        }

        /// <summary>
        /// Gets the relative position.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="clickedPoint">
        /// The clicked point.
        /// </param>
        /// <param name="hasVerticalOrientation">
        /// The vertical orientation.
        /// </param>
        /// <returns>
        /// The relative position.
        /// </returns>
        public static double GetRelativePosition(
            FrameworkElement container, Point clickedPoint, bool hasVerticalOrientation)
        {
            if (hasVerticalOrientation)
            {
                return clickedPoint.Y / container.ActualHeight;
            }

            return clickedPoint.X / container.ActualWidth;
        }

        /// <summary>
        /// Determines if the specified  movement is big enough.
        /// </summary>
        /// <param name="initialMousePosition">
        /// The initial mouse position.
        /// </param>
        /// <param name="currentPosition">
        /// The current position.
        /// </param>
        /// <returns>
        /// True if the movement is big enough.
        /// </returns>
        public static bool IsMovementBigEnough(Point initialMousePosition, Point currentPosition)
        {
            return Math.Abs(currentPosition.X - initialMousePosition.X)
                    >= SystemParameters.MinimumHorizontalDragDistance
                    ||
                    Math.Abs(currentPosition.Y - initialMousePosition.Y) >= SystemParameters.MinimumVerticalDragDistance;
        }

        /// <summary>
        /// The set drag drop template.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetDragDropTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(DragDropTemplateProperty, value);
        }

        /// <summary>
        /// The set is drag source.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetIsDragSource(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragSourceProperty, value);
        }

        /// <summary>
        /// The set is drop target.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetIsDropTarget(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDropTargetProperty, value);
        }

        /// <summary>
        /// Determines if the drag source changed.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private static void IsDragSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var dragSource = obj as ItemsControl;
            if (dragSource != null)
            {
                if (Equals(e.NewValue, true))
                {
                    dragSource.PreviewMouseLeftButtonDown += Instance.DragSourcePreviewMouseLeftButtonDown;
                    dragSource.PreviewMouseLeftButtonUp += Instance.DragSourcePreviewMouseLeftButtonUp;
                    dragSource.PreviewMouseRightButtonDown += Instance.DragSourcePreviewMouseLeftButtonDown;
                    dragSource.PreviewMouseRightButtonUp += Instance.DragSourcePreviewMouseLeftButtonUp;
                    dragSource.PreviewMouseMove += Instance.DragSourcePreviewMouseMove;
                }
                else
                {
                    dragSource.PreviewMouseLeftButtonDown -= Instance.DragSourcePreviewMouseLeftButtonDown;
                    dragSource.PreviewMouseLeftButtonUp -= Instance.DragSourcePreviewMouseLeftButtonUp;
                    dragSource.PreviewMouseRightButtonDown -= Instance.DragSourcePreviewMouseLeftButtonDown;
                    dragSource.PreviewMouseRightButtonUp -= Instance.DragSourcePreviewMouseLeftButtonUp;
                    dragSource.PreviewMouseMove -= Instance.DragSourcePreviewMouseMove;
                }
            }
        }

        /// <summary>
        /// Determines if the drop target changed.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private static void IsDropTargetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var dropTarget = obj as ItemsControl;
            if (dropTarget != null)
            {
                if (Equals(e.NewValue, true))
                {
                    dropTarget.AllowDrop = true;
                    dropTarget.PreviewDrop += Instance.DropTargetPreviewDrop;
                    dropTarget.PreviewDragEnter += Instance.DropTargetPreviewDragEnter;
                    dropTarget.PreviewDragOver += Instance.DropTargetPreviewDragOver;
                    dropTarget.PreviewDragLeave += Instance.DropTargetPreviewDragLeave;
                }
                else
                {
                    dropTarget.AllowDrop = false;
                    dropTarget.PreviewDrop -= Instance.DropTargetPreviewDrop;
                    dropTarget.PreviewDragEnter -= Instance.DropTargetPreviewDragEnter;
                    dropTarget.PreviewDragOver -= Instance.DropTargetPreviewDragOver;
                    dropTarget.PreviewDragLeave -= Instance.DropTargetPreviewDragLeave;
                }
            }
        }

        /// <summary>
        /// Creates or updates the insertion adorner.
        /// </summary>
        private void CreateOrUpdateInsertionAdorner()
        {
            if (this.insertionAdorner != null)
            {
                this.insertionAdorner.IsInFirstHalf = this.isInFirstHalf;
                this.insertionAdorner.InvalidateVisual();
            }
            else
            {
                if (this.targetItemContainer != null)
                {
                    // Here, I need to get adorner layer from targetItemContainer and not targetItemsControl.
                    // This way I get the AdornerLayer within ScrollContentPresenter, and not the one under AdornerDecorator (Snoop is awesome).
                    // If I used targetItemsControl, the adorner would hang out of ItemsControl when there's a horizontal scroll bar.
                    var adornerLayer = AdornerLayer.GetAdornerLayer(this.targetItemContainer);
                    this.insertionAdorner = new InsertionAdorner(
                        this.hasVerticalOrientation, this.isInFirstHalf, this.targetItemContainer, adornerLayer);
                }
            }
        }

        /// <summary>
        /// Decides the drop target.
        /// </summary>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void DecideDropTarget(DragEventArgs e)
        {
            // If the types of the dragged data and ItemsControl's source are compatible,
            // there are 3 situations to have into account when deciding the drop target:
            // 1. mouse is over an items container
            // 2. mouse is over the empty part of an ItemsControl, but ItemsControl is not empty
            // 3. mouse is over an empty ItemsControl.
            // The goal of this method is to decide on the values of the following properties:
            // targetItemContainer, insertionIndex and isInFirstHalf.
            int targetItemsControlCount = this.targetItemsControl.Items.Count;
            var draggedItems = e.Data.GetData(this.format.Name) as IList;

            if (targetItemsControlCount > 0)
            {
                this.hasVerticalOrientation = true;
                this.targetItemContainer =
                    this.targetItemsControl.ContainerFromElement((DependencyObject)e.OriginalSource) as TreeListBoxItem;

                if (this.targetItemContainer != null)
                {
                    Point positionRelativeToItemContainer = e.GetPosition(this.targetItemContainer);
                    double relativePosition = GetRelativePosition(
                        this.targetItemContainer, positionRelativeToItemContainer, this.hasVerticalOrientation);
                    this.isInFirstHalf = relativePosition < 0.5;

                    if (relativePosition > 0.3 && relativePosition < 0.7)
                    {
                        this.dropPosition = DropPosition.Add;
                    }
                    else
                    {
                        this.dropPosition = relativePosition < 0.5
                                                ? DropPosition.InsertBefore
                                                : DropPosition.InsertAfter;
                    }
                }
                else
                {
                    this.targetItemContainer = this.targetItemsControl.ContainerFromIndex(targetItemsControlCount - 1);
                    this.isInFirstHalf = false;
                    this.dropPosition = DropPosition.InsertAfter;
                }
            }
            else
            {
                this.targetItemContainer = null;
                this.dropPosition = DropPosition.InsertBefore;
            }

            if (this.targetItemContainer != null && draggedItems != null)
            {
                var dropTarget = this.targetItemContainer.DataContext as IDropTarget;
                bool copy = (e.Effects & DragDropEffects.Copy) != 0;

                foreach (var draggedItem in draggedItems)
                {
                    var dragSource = draggedItem as IDragSource;
                    if ((dragSource == null || !dragSource.IsDraggable) || (dropTarget == null || !dropTarget.CanDrop(dragSource, this.dropPosition, copy)))
                    {
                        this.targetItemContainer = null;
                        e.Effects = DragDropEffects.None;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the PreviewMouseLeftButtonDown event on the drag source.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void DragSourcePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.sourceItemsControl = (TreeListBox)sender;
            var visual = e.OriginalSource as Visual;

            this.topWindow = Window.GetWindow(this.sourceItemsControl);
            this.initialMousePosition = e.GetPosition(this.topWindow);

            this.sourceItemContainer = visual != null ? this.sourceItemsControl.ContainerFromElement(visual) as TreeListBoxItem : null;
            if (this.sourceItemContainer != null)
            {
                this.draggedData = new List<IDragSource>();
                this.draggedData.Add(this.sourceItemContainer.DataContext);

                // todo: how to drag multiple items?
                // must set e.Handled = true to avoid items being deselected
                // foreach (var si in sourceItemsControl.SelectedItems) this.draggedData.Add(si);
            }
            else
            {
                this.draggedData = null;
            }
        }

        // Drag = mouse down + move by a certain amount

        /// <summary>
        /// Handles the PreviewMouseLeftButtonUp event on the drag source.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void DragSourcePreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.draggedData = null;
        }

        /// <summary>
        /// Handles the PreviewMouseMove event on the drag source.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void DragSourcePreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.draggedData != null)
            {
                // Only drag when user moved the mouse by a reasonable amount.
                if (IsMovementBigEnough(this.initialMousePosition, e.GetPosition(this.topWindow)))
                {
                    var data = new DataObject(this.format.Name, this.draggedData);

                    var control = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

                    DragDrop.DoDragDrop((DependencyObject)sender, data, control ? DragDropEffects.Copy : DragDropEffects.Move);

                    this.draggedData = null;
                }
            }
        }

        /// <summary>
        /// Handles the PreviewDragEnter event on the drop target.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void DropTargetPreviewDragEnter(object sender, DragEventArgs e)
        {
            this.targetItemsControl = (TreeListBox)sender;
            object draggedItems = e.Data.GetData(this.format.Name);

            if (draggedItems != null)
            {
                this.DecideDropTarget(e);
                this.UpdateAdorner();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the PreviewDragLeave event on the drop target.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void DropTargetPreviewDragLeave(object sender, DragEventArgs e)
        {
            // Dragged Adorner is only created once on DragEnter + every time we enter the window.
            // It's only removed once on the DragDrop, and every time we leave the window. (so no need to remove it here)
            this.RemoveAdorners();

            object draggedItems = e.Data.GetData(this.format.Name);
            if (draggedItems != null)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the PreviewDragOver event on the drop target.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void DropTargetPreviewDragOver(object sender, DragEventArgs e)
        {
            object draggedItems = e.Data.GetData(this.format.Name);

            if (draggedItems != null)
            {
                this.DecideDropTarget(e);
                this.UpdateAdorner();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the PreviewDrop event on the drop target.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void DropTargetPreviewDrop(object sender, DragEventArgs e)
        {
            var draggedItems = e.Data.GetData(this.format.Name) as IList;

            if (draggedItems != null)
            {
                this.RemoveAdorners();

                if (this.targetItemContainer == null)
                {
                    return;
                }

                var dropTarget = this.targetItemContainer.DataContext as IDropTarget;
                foreach (var draggedItem in draggedItems)
                {
                    if (dropTarget == draggedItem)
                    {
                        return;
                    }
                }

                bool copy = (e.Effects & DragDropEffects.Copy) != 0;
                foreach (var draggedItem in draggedItems)
                {
                    var dragSource = draggedItem as IDragSource;
                    if (dragSource == null || !dragSource.IsDraggable)
                    {
                        continue;
                    }

                    if (dropTarget == null || !dropTarget.CanDrop(dragSource, this.dropPosition, copy))
                    {
                        continue;
                    }

                    if (!copy)
                    {
                        dragSource.Detach();
                    }

                    dropTarget.Drop(dragSource, this.dropPosition, copy);
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// Removes the adorners.
        /// </summary>
        private void RemoveAdorners()
        {
            this.RemoveInsertionAdorner();
            if (this.targetItemContainer != null)
            {
                this.targetItemContainer.IsDropTarget = false;
            }
        }

        /// <summary>
        /// Removes the insertion adorner.
        /// </summary>
        private void RemoveInsertionAdorner()
        {
            if (this.insertionAdorner != null)
            {
                this.insertionAdorner.Detach();
                this.insertionAdorner = null;
            }
        }

        /// <summary>
        /// Updates the adorner.
        /// </summary>
        private void UpdateAdorner()
        {
            if (this.dropPosition == DropPosition.Add)
            {
                if (this.targetItemContainer != null)
                {
                    this.targetItemContainer.IsDropTarget = true;
                }

                this.RemoveInsertionAdorner();
                return;
            }

            if (this.targetItemContainer != null)
            {
                this.targetItemContainer.IsDropTarget = false;
            }

            this.CreateOrUpdateInsertionAdorner();
        }

    }
}