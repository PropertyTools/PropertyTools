// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DockPanelSplitter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a control that lets the user change the size of elements in a DockPanel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Represents a control that lets the user change the size of elements in a <see cref="DockPanel" />.
    /// </summary>
    public class DockPanelSplitter : Control
    {
        /// <summary>
        /// Identifies the <see cref="ProportionalResize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProportionalResizeProperty = DependencyProperty.Register(
            nameof(ProportionalResize),
            typeof(bool),
            typeof(DockPanelSplitter),
            new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="Thickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
            nameof(Thickness),
            typeof(double),
            typeof(DockPanelSplitter),
            new UIPropertyMetadata(4.0, ThicknessChanged));

        /// <summary>
        /// The element.
        /// </summary>
        private FrameworkElement element; // element to resize (target element)

        /// <summary>
        /// The height.
        /// </summary>
        private double height; // current desired height of the element, can be less than minheight

        /// <summary>
        /// The previous parent height.
        /// </summary>
        private double previousParentHeight; // current height of parent element, used for proportional resize

        /// <summary>
        /// The previous parent width.
        /// </summary>
        private double previousParentWidth; // current width of parent element, used for proportional resize

        /// <summary>
        /// The start drag point.
        /// </summary>
        private Point startDragPoint;

        /// <summary>
        /// The width.
        /// </summary>
        private double width; // current desired width of the element, can be less than minwidth

        /// <summary>
        /// Initializes static members of the <see cref="DockPanelSplitter" /> class.
        /// </summary>
        static DockPanelSplitter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DockPanelSplitter), new FrameworkPropertyMetadata(typeof(DockPanelSplitter)));

            // override the Background property
            BackgroundProperty.OverrideMetadata(
                typeof(DockPanelSplitter), new FrameworkPropertyMetadata(Brushes.Transparent));

            // override the Dock property to get notifications when Dock is changed
            DockPanel.DockProperty.OverrideMetadata(
                typeof(DockPanelSplitter), new FrameworkPropertyMetadata(Dock.Left, DockChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DockPanelSplitter" /> class.
        /// </summary>
        public DockPanelSplitter()
        {
            this.Loaded += this.DockPanelSplitterLoaded;
            this.Unloaded += this.DockPanelSplitterUnloaded;

            this.UpdateHeightOrWidth();
        }

        /// <summary>
        /// Gets a value indicating whether this splitter is horizontal.
        /// </summary>
        public bool IsHorizontal
        {
            get
            {
                Dock dock = DockPanel.GetDock(this);
                return dock == Dock.Top || dock == Dock.Bottom;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to resize elements proportionally.
        /// </summary>
        /// <remarks>Set to <c>false</c> if you don't want the element to be resized when the parent is resized.</remarks>
        public bool ProportionalResize
        {
            get
            {
                return (bool)this.GetValue(ProportionalResizeProperty);
            }

            set
            {
                this.SetValue(ProportionalResizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the thickness (height or width, depending on orientation).
        /// </summary>
        /// <value>The thickness.</value>
        public double Thickness
        {
            get
            {
                return (double)this.GetValue(ThicknessProperty);
            }

            set
            {
                this.SetValue(ThicknessProperty, value);
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (!this.IsEnabled)
            {
                return;
            }

            if (!this.IsMouseCaptured)
            {
                this.startDragPoint = e.GetPosition(this.Parent as IInputElement);
                this.UpdateTargetElement();
                if (this.element != null)
                {
                    this.width = this.element.ActualWidth;
                    this.height = this.element.ActualHeight;
                    this.CaptureMouse();
                }
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseEnter" /> attached event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (!this.IsEnabled)
            {
                return;
            }

            this.Cursor = this.IsHorizontal ? Cursors.SizeNS : Cursors.SizeWE;
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseMove" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                Point ptCurrent = e.GetPosition(this.Parent as IInputElement);
                var delta = new Point(ptCurrent.X - this.startDragPoint.X, ptCurrent.Y - this.startDragPoint.Y);
                Dock dock = DockPanel.GetDock(this);

                if (this.IsHorizontal)
                {
                    delta.Y = this.AdjustHeight(delta.Y, dock);
                }
                else
                {
                    delta.X = this.AdjustWidth(delta.X, dock);
                }

                bool isBottomOrRight = dock == Dock.Right || dock == Dock.Bottom;

                // When docked to the bottom or right, the position has changed after adjusting the size
                if (isBottomOrRight)
                {
                    this.startDragPoint = e.GetPosition(this.Parent as IInputElement);
                }
                else
                {
                    this.startDragPoint = new Point(this.startDragPoint.X + delta.X, this.startDragPoint.Y + delta.Y);
                }
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseUp" /> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the mouse button was released.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                this.ReleaseMouseCapture();
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// The dock changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void DockChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DockPanelSplitter)d).UpdateHeightOrWidth();
        }

        /// <summary>
        /// The thickness changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void ThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DockPanelSplitter)d).UpdateHeightOrWidth();
        }

        /// <summary>
        /// Adjusts the height.
        /// </summary>
        /// <param name="dy">The dy.</param>
        /// <param name="dock">The dock.</param>
        /// <returns>
        /// The adjust height.
        /// </returns>
        private double AdjustHeight(double dy, Dock dock)
        {
            if (dock == Dock.Bottom)
            {
                dy = -dy;
            }

            this.height += dy;
            this.SetTargetHeight(this.height);

            return dy;
        }

        /// <summary>
        /// Adjusts the width.
        /// </summary>
        /// <param name="dx">The dx.</param>
        /// <param name="dock">The dock.</param>
        /// <returns>
        /// The adjust width.
        /// </returns>
        private double AdjustWidth(double dx, Dock dock)
        {
            if (dock == Dock.Right)
            {
                dx = -dx;
            }

            this.width += dx;
            this.SetTargetWidth(this.width);

            return dx;
        }

        /// <summary>
        /// Called when the control is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void DockPanelSplitterLoaded(object sender, RoutedEventArgs e)
        {
            var dp = this.Parent as Panel;
            if (dp == null)
            {
                return;
            }

            // Subscribe to the parent's size changed event
            dp.SizeChanged += this.ParentSizeChanged;

            // Store the current size of the parent DockPanel
            this.previousParentWidth = dp.ActualWidth;
            this.previousParentHeight = dp.ActualHeight;

            // Find the target element
            this.UpdateTargetElement();
        }

        /// <summary>
        /// Called when the control is unloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void DockPanelSplitterUnloaded(object sender, RoutedEventArgs e)
        {
            var dp = this.Parent as Panel;
            if (dp == null)
            {
                return;
            }

            // Unsubscribe
            dp.SizeChanged -= this.ParentSizeChanged;
        }

        /// <summary>
        /// Called when the parent element size is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs" /> instance containing the event data.</param>
        private void ParentSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!this.ProportionalResize)
            {
                return;
            }

            var dp = this.Parent as DockPanel;
            if (dp == null)
            {
                return;
            }

            double sx = dp.ActualWidth / this.previousParentWidth;
            double sy = dp.ActualHeight / this.previousParentHeight;

            if (!double.IsInfinity(sx))
            {
                this.SetTargetWidth(this.element.Width * sx);
            }

            if (!double.IsInfinity(sy))
            {
                this.SetTargetHeight(this.element.Height * sy);
            }

            this.previousParentWidth = dp.ActualWidth;
            this.previousParentHeight = dp.ActualHeight;
        }

        /// <summary>
        /// Sets the height of the target.
        /// </summary>
        /// <param name="newHeight">The new height.</param>
        private void SetTargetHeight(double newHeight)
        {
            if (newHeight < this.element.MinHeight)
            {
                newHeight = this.element.MinHeight;
            }

            if (newHeight > this.element.MaxHeight)
            {
                newHeight = this.element.MaxHeight;
            }

            // the height of the element is not constrained to the available client area
            var dp = this.Parent as Panel;
            Dock dock = DockPanel.GetDock(this);
            var t = this.element.TransformToAncestor(dp) as MatrixTransform;
            if (dock == Dock.Top && newHeight > dp.ActualHeight - t.Matrix.OffsetY - this.Thickness)
            {
                newHeight = dp.ActualHeight - t.Matrix.OffsetY - this.Thickness;
            }

            this.element.Height = newHeight;
        }

        /// <summary>
        /// Sets the width of the target.
        /// </summary>
        /// <param name="newWidth">The new width.</param>
        private void SetTargetWidth(double newWidth)
        {
            if (newWidth < this.element.MinWidth)
            {
                newWidth = this.element.MinWidth;
            }

            if (newWidth > this.element.MaxWidth)
            {
                newWidth = this.element.MaxWidth;
            }

            // the height of the element is not constrained to the available client area
            var dp = this.Parent as Panel;
            Dock dock = DockPanel.GetDock(this);
            var t = this.element.TransformToAncestor(dp) as MatrixTransform;
            if (dock == Dock.Left && newWidth > dp.ActualWidth - t.Matrix.OffsetX - this.Thickness)
            {
                newWidth = dp.ActualWidth - t.Matrix.OffsetX - this.Thickness;
            }

            this.element.Width = newWidth;
        }

        /// <summary>
        /// Updates the width or height .
        /// </summary>
        private void UpdateHeightOrWidth()
        {
            if (this.IsHorizontal)
            {
                this.Height = this.Thickness;
                this.Width = double.NaN;
            }
            else
            {
                this.Width = this.Thickness;
                this.Height = double.NaN;
            }
        }

        /// <summary>
        /// Updates the target element (the element the DockPanelSplitter works on).
        /// </summary>
        private void UpdateTargetElement()
        {
            var dp = this.Parent as Panel;
            if (dp == null)
            {
                return;
            }

            int i = dp.Children.IndexOf(this);

            // The splitter cannot be the first child of the parent DockPanel
            // The splitter works on the 'older' sibling
            if (i > 0 && dp.Children.Count > 0)
            {
                this.element = dp.Children[i - 1] as FrameworkElement;
            }
        }
    }
}