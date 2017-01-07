// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderedEntrySlider.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a slider with header and value entry.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a slider with header and value entry.
    /// </summary>
    public class HeaderedEntrySlider : Control
    {
        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(HeaderedEntrySlider),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EntryStringFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EntryStringFormatProperty = DependencyProperty.Register(
            nameof(EntryStringFormat),
            typeof(string),
            typeof(HeaderedEntrySlider),
            new UIPropertyMetadata("0.##"));

        /// <summary>
        /// Identifies the <see cref="EntryContentAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EntryContentAlignmentProperty = DependencyProperty.Register(
            nameof(EntryContentAlignment),
            typeof(HorizontalAlignment),
            typeof(HeaderedEntrySlider),
            new UIPropertyMetadata(HorizontalAlignment.Right));

        /// <summary>
        /// Identifies the <see cref="LargeChange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register(
            nameof(LargeChange),
            typeof(double),
            typeof(HeaderedEntrySlider),
            new UIPropertyMetadata(10d));

        /// <summary>
        /// Identifies the <see cref="Maximum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            nameof(Maximum),
            typeof(double),
            typeof(HeaderedEntrySlider),
            new UIPropertyMetadata(100d));

        /// <summary>
        /// Identifies the <see cref="Minimum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            nameof(Minimum),
            typeof(double),
            typeof(HeaderedEntrySlider),
            new UIPropertyMetadata(0d));

        /// <summary>
        /// Identifies the <see cref="SmallChange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(
            nameof(SmallChange),
            typeof(double),
            typeof(HeaderedEntrySlider),
            new UIPropertyMetadata(1d));

        /// <summary>
        /// Identifies the <see cref="HeaderWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderWidthProperty = DependencyProperty.Register(
            nameof(HeaderWidth),
            typeof(GridLength),
            typeof(HeaderedEntrySlider),
            new UIPropertyMetadata(GridLength.Auto));

        /// <summary>
        /// Identifies the <see cref="EntryWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EntryWidthProperty = DependencyProperty.Register(
            nameof(EntryWidth),
            typeof(GridLength),
            typeof(HeaderedEntrySlider),
            new UIPropertyMetadata(new GridLength(80)));

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(HeaderedEntrySlider),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Initializes static members of the <see cref="HeaderedEntrySlider" /> class.
        /// </summary>
        static HeaderedEntrySlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderedEntrySlider), new FrameworkPropertyMetadata(typeof(HeaderedEntrySlider)));
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        [Category("Style")]
        [Bindable(true)]
        public string Header
        {
            get
            {
                return (string)this.GetValue(HeaderProperty);
            }

            set
            {
                this.SetValue(HeaderProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the string format for the entry control.
        /// </summary>
        /// <value>The header.</value>
        [Category("Style")]
        [Bindable(true)]
        public string EntryStringFormat
        {
            get
            {
                return (string)this.GetValue(EntryStringFormatProperty);
            }

            set
            {
                this.SetValue(EntryStringFormatProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the content alignment for the entry control.
        /// </summary>
        /// <value>The alignment.</value>
        [Category("Style")]
        [Bindable(true)]
        public HorizontalAlignment EntryContentAlignment
        {
            get
            {
                return (HorizontalAlignment)this.GetValue(EntryContentAlignmentProperty);
            }

            set
            {
                this.SetValue(EntryStringFormatProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the large change.
        /// </summary>
        /// <value>The large change.</value>
        [Category("Behavior")]
        [Bindable(true)]
        public double LargeChange
        {
            get
            {
                return (double)this.GetValue(LargeChangeProperty);
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
        [Category("Behavior")]
        [Bindable(true)]
        public double Maximum
        {
            get
            {
                return (double)this.GetValue(MaximumProperty);
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
        [Category("Behavior")]
        [Bindable(true)]
        public double Minimum
        {
            get
            {
                return (double)this.GetValue(MinimumProperty);
            }

            set
            {
                this.SetValue(MinimumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the SmallChange.
        /// </summary>
        /// <value>The SmallChange.</value>
        [Category("Behavior")]
        [Bindable(true)]
        public double SmallChange
        {
            get
            {
                return (double)this.GetValue(SmallChangeProperty);
            }

            set
            {
                this.SetValue(SmallChangeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the header part.
        /// </summary>
        /// <value>The width.</value>
        [Category("Style")]
        [Bindable(true)]
        public GridLength HeaderWidth
        {
            get
            {
                return (GridLength)this.GetValue(HeaderWidthProperty);
            }

            set
            {
                this.SetValue(HeaderWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the entry control.
        /// </summary>
        /// <value>The width.</value>
        [Category("Style")]
        [Bindable(true)]
        public GridLength EntryWidth
        {
            get
            {
                return (GridLength)this.GetValue(EntryWidthProperty);
            }

            set
            {
                this.SetValue(EntryWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Category("Behavior")]
        [Bindable(true)]
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
    }
}