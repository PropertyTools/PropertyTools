// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for TimeSpanExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using System;
    using System.ComponentModel;
    using ExampleLibrary;
    using PropertyTools;

    /// <summary>
    /// Interaction logic for TimeSpanExample.
    /// </summary>
    [Example("TimeSpan", "Demonstrates built-in TimeSpan editing support in the PropertyGrid.", Tags = new[] { "PropertyGrid", "TimeSpan" })]
    public partial class TimeSpanExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanExample" /> class.
        /// </summary>
        public TimeSpanExample()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    /// View model for the TimeSpan example.
    /// </summary>
    public class TimeSpanExampleViewModel : Observable
    {
        private static readonly object StaticInstance = new TimeSpanExampleModel
        {
            Duration = TimeSpan.FromHours(1.5),
            Timeout = TimeSpan.FromSeconds(30),
            OptionalDelay = null
        };

        private object selectedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanExampleViewModel" /> class.
        /// </summary>
        public TimeSpanExampleViewModel()
        {
            this.SelectedObject = StaticInstance;
        }

        /// <summary>
        /// Gets or sets the selected object.
        /// </summary>
        public object SelectedObject
        {
            get => this.selectedObject;
            internal set => this.SetValue(ref this.selectedObject, value);
        }
    }

    /// <summary>
    /// Model class demonstrating TimeSpan properties.
    /// </summary>
    public class TimeSpanExampleModel : Observable
    {
        private TimeSpan duration;
        private TimeSpan timeout;
        private TimeSpan? optionalDelay;

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        [Category("TimeSpan Properties")]
        [DisplayName("Duration")]
        [Description("A required TimeSpan property representing a duration.")]
        public TimeSpan Duration
        {
            get => this.duration;
            set => this.SetValue(ref this.duration, value);
        }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        [Category("TimeSpan Properties")]
        [DisplayName("Timeout")]
        [Description("A required TimeSpan property representing a timeout period.")]
        public TimeSpan Timeout
        {
            get => this.timeout;
            set => this.SetValue(ref this.timeout, value);
        }

        /// <summary>
        /// Gets or sets the optional delay.
        /// </summary>
        [Category("TimeSpan Properties")]
        [DisplayName("Optional Delay")]
        [Description("A nullable TimeSpan property representing an optional delay. Leave empty to set to null.")]
        public TimeSpan? OptionalDelay
        {
            get => this.optionalDelay;
            set => this.SetValue(ref this.optionalDelay, value);
        }
    }
}
