// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpinControl.cs" company="PropertyTools">
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
//   Represents a spinner control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
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
        /// <summary>
        /// The part down.
        /// </summary>
        private const string PartDown = "PART_DOWN";

        /// <summary>
        /// The part up.
        /// </summary>
        private const string PartUp = "PART_UP";

        /// <summary>
        /// Gets or sets the culture used when parsing the LargeChange/SmallChange properties.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture
        {
            get { return (CultureInfo)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Culture"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CultureProperty =
            DependencyProperty.Register("Culture", typeof(CultureInfo), typeof(SpinControl), new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="DownButtonGeometry"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DownButtonGeometryProperty =
            DependencyProperty.Register(
                "DownButtonGeometry", typeof(Geometry), typeof(SpinControl), new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LargeChange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register(
            "LargeChange", typeof(object), typeof(SpinControl), new UIPropertyMetadata(10.0));

        /// <summary>
        /// Identifies the <see cref="Maximum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(object), typeof(SpinControl), new UIPropertyMetadata(100.0));

        /// <summary>
        /// Identifies the <see cref="Minimum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(object), typeof(SpinControl), new UIPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the <see cref="RepeatInterval"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RepeatIntervalProperty = DependencyProperty.Register(
            "RepeatInterval", typeof(int), typeof(SpinControl), new UIPropertyMetadata(50));

        /// <summary>
        /// Identifies the <see cref="SmallChange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(
            "SmallChange", typeof(object), typeof(SpinControl), new UIPropertyMetadata(1.0));

        /// <summary>
        /// Identifies the <see cref="SpinButtonWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinButtonWidthProperty =
            DependencyProperty.Register(
                "SpinButtonWidth", typeof(GridLength), typeof(SpinControl), new UIPropertyMetadata(new GridLength(14)));

        /// <summary>
        /// Identifies the <see cref="UpButtonGeometry"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UpButtonGeometryProperty =
            DependencyProperty.Register(
                "UpButtonGeometry", typeof(Geometry), typeof(SpinControl), new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(object),
            typeof(SpinControl),
            new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SpinnerValueChanged, CoerceSpinnerValue));

        /// <summary>
        /// The down button.
        /// </summary>
        private RepeatButton downbutton;

        /// <summary>
        /// The up button.
        /// </summary>
        private RepeatButton upbutton;

        /// <summary>
        /// Initializes static members of the <see cref="SpinControl" /> class.
        /// </summary>
        static SpinControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SpinControl), new FrameworkPropertyMetadata(typeof(SpinControl)));
        }

        /// <summary>
        /// Gets or sets down button geometry.
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
        /// Gets or sets the large change.
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
        /// Gets or sets the maximum.
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
        /// Gets or sets the minimum.
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
        /// Gets or sets the repeat SmallChange (milliseconds).
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
        /// Gets or sets the SmallChange.
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
        /// Gets or sets the width of the spin buttons.
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
        /// Gets or sets up button geometry.
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
        /// Gets or sets the value.
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

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see
        /// cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
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

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseWheel" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseWheelEventArgs" /> that contains the event data.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            bool shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            if (e.Delta > 0)
            {
                this.ChangeValue(1, ctrl || shift);
            }
            else
            {
                this.ChangeValue(-1, ctrl || shift);
            }

            e.Handled = true;
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.PreviewKeyDown" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data.</param>
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
        /// <param name="d">The d.</param>
        /// <param name="basevalue">The basevalue.</param>
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
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void SpinnerValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Changes the value.
        /// </summary>
        /// <param name="sign">The sign of the change.</param>
        /// <param name="isLargeChange">The is Large Change.</param>
        private void ChangeValue(int sign, bool isLargeChange)
        {
            var change = isLargeChange ? this.LargeChange : this.SmallChange;
            if (change == null)
            {
                return;
            }

            if (change is string)
            {
                object c;
                var type = this.Value.GetType();
                if (ReflectionMath.TryParse(type, (string)change, CultureInfo.InvariantCulture, out c))
                {

                    object result;
                    if (sign > 0)
                    {
                        if (ReflectionMath.TryAdd(this.Value, c, out result))
                        {
                            this.Value = result;
                            return;
                        }
                    }
                    else
                    {
                        if (ReflectionMath.TrySubtract(this.Value, c, out result))
                        {
                            this.Value = result;
                            //this.Value = c.Convert(this.CoerceSpinnerValue(result), typeof(string), null, this.Culture);
                            return;
                        }
                    }
                }
            }
            if (this.Value != null)
            {
                ITypeDescriptorContext context = null;
                var c = TypeDescriptor.GetConverter(this.Value);
                if (c.CanConvertFrom(null, change.GetType()))
                {
                    var convertedChange = c.ConvertFrom(context, this.Culture ?? CultureInfo.InvariantCulture, change);
                    object result;
                    if (sign > 0)
                    {
                        if (ReflectionMath.TryAdd(this.Value, convertedChange, out result))
                        {
                            this.Value = this.CoerceSpinnerValue(result);
                            return;
                        }
                    }
                    else
                    {
                        if (ReflectionMath.TrySubtract(this.Value, convertedChange, out result))
                        {
                            this.Value = this.CoerceSpinnerValue(result);
                            return;
                        }
                    }
                }
            }

            if (this.Value is double)
            {
                var doubleChange = Convert.ToDouble(change);
                var currentValue = (double)this.Value;
                var newValue = sign > 0 ? currentValue + doubleChange : currentValue - doubleChange;
                if (sign > 0 && currentValue > double.MaxValue - doubleChange)
                {
                    newValue = double.MaxValue;
                }

                if (sign < 0 && currentValue < double.MinValue + doubleChange)
                {
                    newValue = double.MinValue;
                }

                this.Value = this.CoerceSpinnerValue(newValue);
                return;
            }

            if (this.Value is int)
            {
                var intChange = Convert.ToInt32(change);
                var currentValue = (int)this.Value;
                var newValue = sign > 0 ? currentValue + intChange : currentValue - intChange;
                if (sign > 0 && currentValue > int.MaxValue - intChange)
                {
                    newValue = int.MaxValue;
                }

                if (sign < 0 && currentValue < int.MinValue + intChange)
                {
                    newValue = int.MinValue;
                }

                this.Value = this.CoerceSpinnerValue(newValue);
                return;
            }

            if (this.Value is long)
            {
                var longChange = Convert.ToInt64(change);
                var currentValue = (long)this.Value;
                var newValue = sign > 0 ? currentValue + longChange : currentValue - longChange;
                if (sign > 0 && currentValue > long.MaxValue - longChange)
                {
                    newValue = long.MaxValue;
                }

                if (sign < 0 && currentValue < long.MinValue + longChange)
                {
                    newValue = long.MinValue;
                }

                this.Value = this.CoerceSpinnerValue(newValue);
                return;
            }

            if (this.Value is short)
            {
                var shortChange = Convert.ToInt16(change);
                var currentValue = (short)this.Value;
                var newValue = (short)(sign > 0 ? currentValue + shortChange : currentValue - shortChange);
                if (sign > 0 && currentValue > short.MaxValue - shortChange)
                {
                    newValue = short.MaxValue;
                }

                if (sign < 0 && currentValue < short.MinValue + shortChange)
                {
                    newValue = short.MinValue;
                }

                this.Value = this.CoerceSpinnerValue(newValue);
                return;
            }

            if (this.Value is ulong)
            {
                var intChange = Convert.ToUInt64(change);
                var currentValue = (ulong)this.Value;
                ulong newValue = sign > 0 ? currentValue + intChange : currentValue - intChange;
                if (sign > 0 && currentValue > ulong.MaxValue - intChange)
                {
                    newValue = ulong.MaxValue;
                }

                if (sign < 0 && currentValue < ulong.MinValue + intChange)
                {
                    newValue = ulong.MinValue;
                }

                this.Value = this.CoerceSpinnerValue(newValue);

                return;
            }

            if (this.Value is uint)
            {
                var intChange = Convert.ToUInt32(change);
                var currentValue = (uint)this.Value;
                uint newValue = sign > 0 ? currentValue + intChange : currentValue - intChange;
                if (sign > 0 && currentValue > uint.MaxValue - intChange)
                {
                    newValue = uint.MaxValue;
                }

                if (sign < 0 && currentValue < uint.MinValue + intChange)
                {
                    newValue = uint.MinValue;
                }

                this.Value = this.CoerceSpinnerValue(newValue);

                return;
            }

            if (this.Value is ushort)
            {
                var intChange = Convert.ToUInt16(change);
                var currentValue = (ushort)this.Value;
                var newValue = (ushort)(sign > 0 ? currentValue + intChange : currentValue - intChange);
                if (sign > 0 && currentValue > ushort.MaxValue - intChange)
                {
                    newValue = ushort.MaxValue;
                }

                if (sign < 0 && currentValue < ushort.MinValue + intChange)
                {
                    newValue = ushort.MinValue;
                }

                this.Value = this.CoerceSpinnerValue(newValue);

                return;
            }

            if (this.Value is byte)
            {
                var intChange = Convert.ToByte(change);
                var currentValue = (byte)this.Value;
                var newValue = (byte)(sign > 0 ? currentValue + intChange : currentValue - intChange);
                if (sign > 0 && currentValue > byte.MaxValue - intChange)
                {
                    newValue = byte.MaxValue;
                }

                if (sign < 0 && currentValue < byte.MinValue + intChange)
                {
                    newValue = byte.MinValue;
                }

                this.Value = this.CoerceSpinnerValue(newValue);

                return;
            }

            if (this.Value is sbyte)
            {
                var intChange = Convert.ToSByte(change);
                var currentValue = (sbyte)this.Value;
                var newValue = (sbyte)(sign > 0 ? currentValue + intChange : currentValue - intChange);
                if (sign > 0 && currentValue > sbyte.MaxValue - intChange)
                {
                    newValue = sbyte.MaxValue;
                }

                if (sign < 0 && currentValue < sbyte.MinValue + intChange)
                {
                    newValue = sbyte.MinValue;
                }

                this.Value = this.CoerceSpinnerValue(newValue);

                return;
            }

            if (this.Value is float)
            {
                var floatChange = Convert.ToSingle(change);
                var currentValue = (float)this.Value;
                float newValue = sign > 0 ? currentValue + floatChange : currentValue - floatChange;
                if (sign > 0 && currentValue > float.MaxValue - floatChange)
                {
                    newValue = float.MaxValue;
                }

                if (sign < 0 && currentValue < float.MinValue + floatChange)
                {
                    newValue = float.MinValue;
                }

                this.Value = this.CoerceSpinnerValue(newValue);
                return;
            }

            if (this.Value is decimal)
            {
                var decimalChange = Convert.ToDecimal(change);
                var currentValue = (decimal)this.Value;
                decimal newValue = sign > 0 ? currentValue + decimalChange : currentValue - decimalChange;
                if (sign > 0 && currentValue > decimal.MaxValue - decimalChange)
                {
                    newValue = decimal.MaxValue;
                }

                if (sign < 0 && currentValue < decimal.MinValue + decimalChange)
                {
                    newValue = decimal.MinValue;
                }

                this.Value = this.CoerceSpinnerValue(newValue);
                return;
            }

            if (this.Value is DateTime)
            {
                object newValue = null;
                if (change is TimeSpan)
                {
                    var span = (TimeSpan)change;
                    newValue = ((DateTime)this.Value).AddDays(span.TotalDays * sign);
                }

                if (this.IsNumeric(change))
                {
                    double doubleChange = Convert.ToDouble(change);
                    if (Math.Abs(doubleChange) > double.Epsilon)
                    {
                        newValue = ((DateTime)this.Value).AddDays(doubleChange * sign);
                    }
                }

                this.Value = this.CoerceSpinnerValue(newValue);
                return;
            }

            if (this.Value is TimeSpan)
            {
                var current = (TimeSpan)this.Value;
                object newValue = null;
                if (change is TimeSpan)
                {
                    var span = (TimeSpan)change;
                    newValue = sign > 0 ? current.Add(span) : current.Subtract(span);
                }

                if (this.IsNumeric(change))
                {
                    double doubleChange = Convert.ToDouble(change);
                    if (Math.Abs(doubleChange) > double.Epsilon)
                    {
                        newValue = current.Add(TimeSpan.FromSeconds(doubleChange * sign));
                    }
                }

                this.Value = this.CoerceSpinnerValue(newValue);
                return;
            }
        }

        /// <summary>
        /// Coerces the spinner value.
        /// </summary>
        /// <param name="basevalue">The basevalue.</param>
        /// <returns>
        /// The coerced value.
        /// </returns>
        private object CoerceSpinnerValue(object basevalue)
        {
            if (basevalue == null)
            {
                return basevalue;
            }

            if (this.Maximum != null)
            {
                var maximumType = this.Maximum.GetType();
                if (maximumType == basevalue.GetType())
                {
                    if (Compare(basevalue, this.Maximum) == 1)
                    {
                        return this.Maximum;
                    }
                }
            }

            if (this.Minimum != null)
            {
                var minimumType = this.Minimum.GetType();
                if (minimumType == basevalue.GetType())
                {
                    if (Compare(basevalue, this.Minimum) == -1)
                    {
                        return this.Minimum;
                    }
                }
            }

            if (this.Value != null)
            {
                ITypeDescriptorContext context = null;
                var c = TypeDescriptor.GetConverter(this.Value);
                if (this.Maximum != null && c != null && c.CanConvertFrom(context, this.Maximum.GetType()))
                {
                    var v = basevalue as IComparable;
                    var maximum =
                        c.ConvertFrom(context, this.Culture ?? CultureInfo.InvariantCulture, this.Maximum) as
                        IComparable;
                    if (v != null && v.CompareTo(maximum) > 0)
                    {
                        return maximum;
                    }
                }

                if (this.Minimum != null && c != null && c.CanConvertFrom(context, this.Minimum.GetType()))
                {
                    var v = basevalue as IComparable;
                    var minimum =
                        c.ConvertFrom(context, this.Culture ?? CultureInfo.InvariantCulture, this.Minimum) as
                        IComparable;
                    if (v != null && v.CompareTo(minimum) < 0)
                    {
                        return minimum;
                    }
                }
            }

            double numericValue = double.NaN;
            if (this.IsNumeric(basevalue))
            {
                numericValue = Convert.ToDouble(basevalue);
            }

            double numericMaximum = double.MaxValue;
            if (this.Maximum != null && this.IsNumeric(this.Maximum))
            {
                numericMaximum = Convert.ToDouble(this.Maximum);
                if (double.IsNaN(numericValue) && numericValue > numericMaximum)
                {
                    return numericMaximum;
                }
            }

            double numericMinimum = double.MinValue;
            if (this.Minimum != null && this.IsNumeric(this.Minimum))
            {
                numericMinimum = Convert.ToDouble(this.Minimum);
                if (double.IsNaN(numericValue) && numericValue < numericMaximum)
                {
                    {
                        return numericMinimum;
                    }
                }
            }

            if (basevalue is double)
            {
                var bv = (double)basevalue;
                if (bv > numericMaximum)
                {
                    return numericMaximum;
                }

                if (bv < numericMinimum)
                {
                    return numericMinimum;
                }

                return bv;
            }

            if (basevalue is float)
            {
                var bv = (float)basevalue;
                if (bv > numericMaximum)
                {
                    return (float)numericMaximum;
                }

                if (bv < numericMinimum)
                {
                    return (float)numericMinimum;
                }

                return bv;
            }

            if (basevalue is int)
            {
                var bv = (int)basevalue;
                if (bv > numericMaximum)
                {
                    return (int)numericMaximum;
                }

                if (bv < numericMinimum)
                {
                    return (int)numericMinimum;
                }

                return bv;
            }

            if (basevalue is DateTime)
            {
                var bv = (DateTime)basevalue;
                if (this.Maximum is DateTime)
                {
                    DateTime dateMaximum = Convert.ToDateTime(this.Maximum);
                    if (bv > dateMaximum)
                    {
                        return dateMaximum;
                    }
                }

                if (this.Minimum is DateTime)
                {
                    DateTime dateMinimum = Convert.ToDateTime(this.Minimum);
                    if (bv < dateMinimum)
                    {
                        return dateMinimum;
                    }
                }

                return bv;
            }

            return basevalue;
        }

        private int Compare(object v1, object v2)
        {
            var c1 = v1 as IComparable;
            if (c1 == null)
            {
                return -2;
            }

            return c1.CompareTo(v2);
        }

        /// <summary>
        /// Handles down button clicks.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void DownbuttonClick(object sender, RoutedEventArgs e)
        {
            bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            this.ChangeValue(-1, ctrl);
        }

        /// <summary>
        /// Check if an object is of a numeric type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True if the value is of a numeric type.
        /// </returns>
        private bool IsNumeric(object value)
        {
            if (value is double || value is int || value is uint || value is long || value is ulong || value is short
                || value is ushort || value is byte || value is sbyte || value is float || value is decimal)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Handles up button clicks.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void UpbuttonClick(object sender, RoutedEventArgs e)
        {
            bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            this.ChangeValue(1, ctrl);
        }
    }
}