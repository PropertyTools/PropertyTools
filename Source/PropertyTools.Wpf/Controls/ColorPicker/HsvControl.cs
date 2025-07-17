// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HsvControl.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   The hsv control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// The hsv control.
    /// </summary>
    /// <remarks>Original code by Ury Jamshy, 21 July 2011.
    /// The Code Project Open License (CPOL)</remarks>
    [TemplatePart(Name = PartThumb, Type = typeof(Thumb))]
    public class HsvControl : Control
    {
        /// <summary>
        /// Identifies the <see cref="Hue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
            nameof(Hue),
            typeof(double),
            typeof(HsvControl),
            new FrameworkPropertyMetadata(
                (double)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHueChanged));

        /// <summary>
        /// Identifies the <see cref="Saturation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
            nameof(Saturation),
            typeof(double),
            typeof(HsvControl),
            new FrameworkPropertyMetadata(
                (double)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSaturationChanged));

        /// <summary>
        /// Identifies the <see cref="SelectedColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            nameof(SelectedColor),
            typeof(Color?),
            typeof(HsvControl),
            new FrameworkPropertyMetadata(Colors.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(HsvControl),
            new FrameworkPropertyMetadata(
                (double)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        /// <summary>
        /// The thumb name.
        /// </summary>
        private const string PartThumb = "PART_Thumb";

        /// <summary>
        /// The thumb transform.
        /// </summary>
        private readonly TranslateTransform thumbTransform = new TranslateTransform();

        /// <summary>
        /// The thumb.
        /// </summary>
        private Thumb thumb;

#pragma warning disable 649

        /// <summary>
        /// The within update flag.
        /// </summary>
        internal bool withinUpdate;
#pragma warning restore 649

        /// <summary>
        /// Initializes static members of the <see cref="HsvControl" /> class.
        /// </summary>
        static HsvControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(HsvControl), new FrameworkPropertyMetadata(typeof(HsvControl)));

            // Register Event Handler for the Thumb
            EventManager.RegisterClassHandler(
                typeof(HsvControl), Thumb.DragDeltaEvent, new DragDeltaEventHandler(OnThumbDragDelta));
            EventManager.RegisterClassHandler(
                typeof(HsvControl), Thumb.DragCompletedEvent, new DragCompletedEventHandler(OnThumbDragCompleted));
        }

        /// <summary>
        /// The on thumb drag completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void OnThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            ((HsvControl)sender).OnThumbDragCompleted(e);
        }

        /// <summary>
        /// The on thumb drag completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void OnThumbDragCompleted(DragCompletedEventArgs sender)
        {
            var editableObject = this.DataContext as IEditableObject;
            if (editableObject != null)
            {
                editableObject.EndEdit();
            }
        }

        /// <summary>
        /// Gets or sets Hue.
        /// </summary>
        public double Hue
        {
            get
            {
                return (double)this.GetValue(HueProperty);
            }

            set
            {
                this.SetValue(HueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Saturation.
        /// </summary>
        public double Saturation
        {
            get
            {
                return (double)this.GetValue(SaturationProperty);
            }

            set
            {
                this.SetValue(SaturationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets SelectedColor.
        /// </summary>
        public Color? SelectedColor
        {
            get
            {
                return (Color?)this.GetValue(SelectedColorProperty);
            }

            set
            {
                this.SetValue(SelectedColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Value.
        /// </summary>
        public double Value
        {
            get
            {
                return (double)this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// The on apply template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.thumb = this.GetTemplateChild(PartThumb) as Thumb;
            if (this.thumb != null)
            {
                this.UpdateThumbPosition();
                this.thumb.RenderTransform = this.thumbTransform;
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" />�routed event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var editableObject = this.DataContext as IEditableObject;
            if (editableObject != null)
            {
                editableObject.BeginEdit();
            }

            if (this.thumb != null)
            {
                Point position = e.GetPosition(this);

                this.UpdatePositionAndSaturationAndValue(position.X, position.Y);

                // Initiate mouse event on thumb so it will start drag
                this.thumb.RaiseEvent(e);
            }

            base.OnMouseLeftButtonDown(e);
        }

        /// <summary>
        /// The on render size changed.
        /// </summary>
        /// <param name="sizeInfo">The size info.</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            this.UpdateThumbPosition();

            base.OnRenderSizeChanged(sizeInfo);
        }

        /// <summary>
        /// The on hue changed.
        /// </summary>
        /// <param name="relatedObject">The related object.</param>
        /// <param name="e">The e.</param>
        private static void OnHueChanged(DependencyObject relatedObject, DependencyPropertyChangedEventArgs e)
        {
            var hsvControl = relatedObject as HsvControl;
            if (hsvControl != null && !hsvControl.withinUpdate)
            {
                hsvControl.UpdateSelectedColor();
            }
        }

        /// <summary>
        /// The on saturation changed.
        /// </summary>
        /// <param name="relatedObject">The related object.</param>
        /// <param name="e">The e.</param>
        private static void OnSaturationChanged(DependencyObject relatedObject, DependencyPropertyChangedEventArgs e)
        {
            var hsvControl = relatedObject as HsvControl;
            if (hsvControl != null && !hsvControl.withinUpdate)
            {
                hsvControl.UpdateThumbPosition();
            }
        }

        /// <summary>
        /// The on thumb drag delta.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            var hsvControl = sender as HsvControl;
            if (hsvControl != null)
            {
                hsvControl.OnThumbDragDelta(e);
            }
        }

        /// <summary>
        /// The on value changed.
        /// </summary>
        /// <param name="relatedObject">The related object.</param>
        /// <param name="e">The e.</param>
        private static void OnValueChanged(DependencyObject relatedObject, DependencyPropertyChangedEventArgs e)
        {
            var hsvControl = relatedObject as HsvControl;
            if (hsvControl != null && !hsvControl.withinUpdate)
            {
                hsvControl.UpdateThumbPosition();
            }
        }

        /// <summary>
        /// Limit value to range (0 , max]
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The max.</param>
        /// <returns>
        /// The limit value.
        /// </returns>
        private double LimitValue(double value, double max)
        {
            if (value < 0)
            {
                value = 0;
            }

            if (value > max)
            {
                value = max;
            }

            return value;
        }

        /// <summary>
        /// The on thumb drag delta.
        /// </summary>
        /// <param name="e">The e.</param>
        private void OnThumbDragDelta(DragDeltaEventArgs e)
        {
            double offsetX = this.thumbTransform.X + e.HorizontalChange;
            double offsetY = this.thumbTransform.Y + e.VerticalChange;

            this.UpdatePositionAndSaturationAndValue(offsetX, offsetY);
        }

        /// <summary>
        /// The update position and saturation and value.
        /// </summary>
        /// <param name="positionX">The position x.</param>
        /// <param name="positionY">The position y.</param>
        private void UpdatePositionAndSaturationAndValue(double positionX, double positionY)
        {
            positionX = this.LimitValue(positionX, this.ActualWidth);
            positionY = this.LimitValue(positionY, this.ActualHeight);

            this.thumbTransform.X = positionX;
            this.thumbTransform.Y = positionY;

            this.Saturation = 100.0 * positionX / this.ActualWidth;
            this.Value = 100.0 * (1 - positionY / this.ActualHeight);

            this.UpdateSelectedColor();
        }

        /// <summary>
        /// The update selected color.
        /// </summary>
        private void UpdateSelectedColor()
        {
            this.SelectedColor = ColorHelper.HsvToColor(this.Hue / 360.0, this.Saturation / 100.0, this.Value / 100.0);

            // ColorUtils.FireSelectedColorChangedEvent(this, SelectedColorChangedEvent, oldColor, newColor);
        }

        /// <summary>
        /// The update thumb position.
        /// </summary>
        private void UpdateThumbPosition()
        {
            this.thumbTransform.X = this.Saturation * 0.01 * this.ActualWidth;
            this.thumbTransform.Y = (100 - this.Value) * 0.01 * this.ActualHeight;

            this.SelectedColor = ColorHelper.HsvToColor(this.Hue / 360.0, this.Saturation / 100.0, this.Value / 100.0);
        }
    }
}