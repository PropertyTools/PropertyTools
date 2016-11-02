// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderedEntrySliderPage.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for HeaderedEntrySlider page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlDemos
{
    using PropertyTools;

    /// <summary>
    /// Interaction logic for HeaderedEntrySlider page.
    /// </summary>
    public partial class HeaderedEntrySliderPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderedEntrySliderPage"/> class.
        /// </summary>
        public HeaderedEntrySliderPage()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel();
        }

        /// <summary>
        /// The view model for the page.
        /// </summary>
        private class ViewModel : Observable
        {
            /// <summary>
            /// The value
            /// </summary>
            private double value;

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public double Value
            {
                get { return this.value; }
                set { this.SetValue(ref this.value, value); }
            }
        }
    }
}