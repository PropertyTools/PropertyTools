// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpinControl.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Represents a spinner control.
    /// </summary>
    [TemplatePart(Name = PartUp, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PartDown, Type = typeof(RepeatButton))]
    public class SpinControl : ContentControl
    {
        #region Constants and Fields

        /// <summary>
        /// The down button geometry property.
        /// </summary>
        public static readonly DependencyProperty DownButtonGeometryProperty =
            DependencyProperty.Register(
                "DownButtonGeometry", typeof(Geometry), typeof(SpinControl), new UIPropertyMetadata(null));

        /// <summary>
        /// The large change property.
        /// </summary>
        public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register(
            "LargeChange", typeof(object), typeof(SpinControl), new UIPropertyMetadata(10.0));

        /// <summary>
        ///   The maximum property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(object), typeof(SpinControl), new UIPropertyMetadata(100.0));

        /// <summary>
        ///   The minimum property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(object), typeof(SpinControl), new UIPropertyMetadata(0.0));

        /// <summary>
        ///   The repeat SmallChange property.
        /// </summary>
        public static readonly DependencyProperty RepeatIntervalProperty = DependencyProperty.Register(
            "RepeatInterval", typeof(int), typeof(SpinControl), new UIPropertyMetadata(50));

        /// <summary>
        ///   The SmallChange property.
        /// </summary>
        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(
            "SmallChange", typeof(object), typeof(SpinControl), new UIPropertyMetadata(1.0));

        /// <summary>
        /// The spin button width property.
        /// </summary>
        public static readonly DependencyProperty SpinButtonWidthProperty =
            DependencyProperty.Register(
                "SpinButtonWidth", typeof(GridLength), typeof(SpinControl), new UIPropertyMetadata(new GridLength(20)));

        /// <summary>
        /// The up button geometry property.
        /// </summary>
        public static readonly DependencyProperty UpButtonGeometryProperty =
            DependencyProperty.Register(
                "UpButtonGeometry", typeof(Geometry), typeof(SpinControl), new UIPropertyMetadata(null));

        /// <summary>
        ///   The value property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", 
            typeof(object), 
            typeof(SpinControl), 
            new FrameworkPropertyMetadata(
                0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SpinnerValueChanged, CoerceSpinnerValue));

        /// <summary>
        ///   The part down.
        /// </summary>
        private const string PartDown = "PART_DOWN";

        /// <summary>
        ///   The part up.
        /// </summary>
        private const string PartUp = "PART_UP";

        /// <summary>
        ///   The down button.
        /// </summary>
        private RepeatButton downbutton;

        /// <summary>
        ///   The up button.
        /// </summary>
        private RepeatButton upbutton;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "SpinControl" /> class.
        /// </summary>
        static SpinControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SpinControl), new FrameworkPropertyMetadata(typeof(SpinControl)));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets down button geometry.
        /// </summary>
        /// <value>Down button geometry.</value>
        public Geometry DownButtonGeometry
        {
            get
            {
                return (Geometry)this.GetValue(DownButtonGeometryProperty);
            }

            set
            {
                this.SetValue(DownButtonGeometryProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the large change.
        /// </summary>
        /// <value>The large change.</value>
        public object LargeChange
        {
            get
            {
                return this.GetValue(LargeChangeProperty);
            }

            set
            {
                this.SetValue(LargeChangeProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public object Maximum
        {
            get
            {
                return this.GetValue(MaximumProperty);
            }

            set
            {
                this.SetValue(MaximumProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public object Minimum
        {
            get
            {
                return this.GetValue(MinimumProperty);
            }

            set
            {
                this.SetValue(MinimumProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the repeat SmallChange (milliseconds).
        /// </summary>
        /// <value>The repeat SmallChange.</value>
        public int RepeatInterval
        {
            get
            {
                return (int)this.GetValue(RepeatIntervalProperty);
            }

            set
            {
                this.SetValue(RepeatIntervalProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the SmallChange.
        /// </summary>
        /// <value>The SmallChange.</value>
        public object SmallChange
        {
            get
            {
                return this.GetValue(SmallChangeProperty);
            }

            set
            {
                this.SetValue(SmallChangeProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the width of the spin buttons.
        /// </summary>
        /// <value>The width of the spin button.</value>
        public GridLength SpinButtonWidth
        {
            get
            {
                return (GridLength)this.GetValue(SpinButtonWidthProperty);
            }

            set
            {
                this.SetValue(SpinButtonWidthProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets up button geometry.
        /// </summary>
        /// <value>Up button geometry.</value>
        public Geometry UpButtonGeometry
        {
            get
            {
                return (Geometry)this.GetValue(UpButtonGeometryProperty);
            }

            set
            {
                this.SetValue(UpButtonGeometryProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get
            {
                return this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
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
            this.upbutton = this.GetTemplateChild(PartUp) as RepeatButton;
            this.downbutton = this.GetTemplateChild(PartDown) as RepeatButton;

            if (this.upbutton != null)
            {
                this.upbutton.Click += this.UpbuttonClick;
            }

            if (this.downbutton != null)
            {
                this.downbutton.Click += this.DownbuttonClick;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on mouse wheel.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            if (e.Delta > 0)
            {
                this.ChangeValue(1, ctrl);
            }
            else
            {
                this.ChangeValue(-1, ctrl);
            }

            e.Handled = true;
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.PreviewKeyDown"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.KeyEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            switch (e.Key)
            {
                case Key.Up:
                    this.ChangeValue(1, false);
                    e.Handled = true;
                    break;
                case Key.Down:
                    this.ChangeValue(-1, false);
                    e.Handled = true;
                    break;
                case Key.PageUp:
                    this.ChangeValue(1, true);
                    e.Handled = true;
                    break;
                case Key.PageDown:
                    this.ChangeValue(-1, true);
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Coerces the spinner value.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="basevalue">
        /// The basevalue.
        /// </param>
        /// <returns>
        /// The coerce spinner value.
        /// </returns>
        private static object CoerceSpinnerValue(DependencyObject d, object basevalue)
        {
            return ((SpinControl)d).CoerceSpinnerValue(basevalue);
        }

        /// <summary>
        /// The spinner value changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void SpinnerValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Changes the value.
        /// </summary>
        /// <param name="sign">
        /// The sign of the change.
        /// </param>
        /// <param name="isLargeChange">
        /// The is Large Change.
        /// </param>
        private void ChangeValue(int sign, bool isLargeChange)
        {
            double numericChange = 0;
            if (isLargeChange)
            {
                if (this.LargeChange is double || this.LargeChange is int)
                {
                    numericChange = Convert.ToDouble(this.LargeChange);
                }
            }
            else
            {
                if (this.SmallChange is double || this.SmallChange is int)
                {
                    numericChange = Convert.ToDouble(this.SmallChange);
                }
            }

            if (this.Value is double)
            {
                var newValue = (double)this.Value + numericChange * sign;
                this.Value = this.CoerceSpinnerValue(newValue);
            }

            if (this.Value is int)
            {
                var newValue = (int)this.Value + (int)(numericChange * sign);
                this.Value = this.CoerceSpinnerValue(newValue);
            }

            if (this.Value is DateTime)
            {
                object newValue = null;
                if (isLargeChange)
                {
                    if (this.LargeChange is TimeSpan)
                    {
                        var span = (TimeSpan)this.LargeChange;
                        newValue = ((DateTime)this.Value).AddDays(span.TotalDays * sign);
                    }
                }
                else
                {
                    if (this.SmallChange is TimeSpan)
                    {
                        var span = (TimeSpan)this.SmallChange;
                        newValue = ((DateTime)this.Value).AddDays(span.TotalDays * sign);
                    }
                }

                if (Math.Abs(numericChange) > double.Epsilon)
                {
                    newValue = ((DateTime)this.Value).AddDays(numericChange * sign);
                }

                this.Value = this.CoerceSpinnerValue(newValue);
            }
        }

        /// <summary>
        /// Coerces the spinner value.
        /// </summary>
        /// <param name="basevalue">
        /// The basevalue.
        /// </param>
        /// <returns>
        /// The coerced value.
        /// </returns>
        private object CoerceSpinnerValue(object basevalue)
        {
            double max = double.MaxValue;
            if (this.Maximum is double || this.Maximum is int)
            {
                max = Convert.ToDouble(this.Maximum);
            }

            double min = double.MinValue;
            if (this.Maximum is double || this.Maximum is int)
            {
                min = Convert.ToDouble(this.Minimum);
            }

            if (basevalue is double)
            {
                var bv = (double)basevalue;
                if (bv > max)
                {
                    return max;
                }

                if (bv < min)
                {
                    return min;
                }

                return bv;
            }

            if (basevalue is int)
            {
                var bv = (int)basevalue;
                if (bv > max)
                {
                    return (int)max;
                }

                if (bv < min)
                {
                    return (int)min;
                }

                return bv;
            }

            if (basevalue is DateTime)
            {
                var bv = (DateTime)basevalue;
                if (this.Maximum is DateTime)
                {
                    DateTime dmax = Convert.ToDateTime(this.Maximum);
                    if (bv > dmax)
                    {
                        return dmax;
                    }
                }

                if (this.Minimum is DateTime)
                {
                    DateTime dmin = Convert.ToDateTime(this.Minimum);
                    if (bv < dmin)
                    {
                        return dmin;
                    }
                }

                return bv;
            }

            return basevalue;
        }

        /// <summary>
        /// Handles down button clicks.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void DownbuttonClick(object sender, RoutedEventArgs e)
        {
            bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            this.ChangeValue(-1, ctrl);
        }

        /// <summary>
        /// Handles up button clicks.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void UpbuttonClick(object sender, RoutedEventArgs e)
        {
            bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            this.ChangeValue(1, ctrl);
        }

        #endregion
    }
}