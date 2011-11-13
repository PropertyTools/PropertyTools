// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpectrumSlider.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// The spectrum slider.
    /// </summary>
    /// <remarks>
    /// Original code by Ury Jamshy, 21 July 2011.
    ///   See http://www.codeproject.com/KB/WPF/ColorPicker2010.aspx
    ///   The Code Project Open License (CPOL)
    ///   http://www.codeproject.com/info/cpol10.aspx
    /// </remarks>
    public class SpectrumSlider : SliderEx
    {
        #region Constants and Fields

        /// <summary>
        ///   The hue property.
        /// </summary>
        public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
            "Hue", 
            typeof(double), 
            typeof(SpectrumSlider), 
            new FrameworkPropertyMetadata(
                (double)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHuePropertyChanged));

        /// <summary>
        ///   The within changing flag.
        /// </summary>
        private bool withinChanging;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "SpectrumSlider" /> class.
        /// </summary>
        static SpectrumSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SpectrumSlider), new FrameworkPropertyMetadata(typeof(SpectrumSlider)));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SpectrumSlider" /> class.
        /// </summary>
        public SpectrumSlider()
        {
            this.SetBackground();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets Hue.
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

        #endregion

        #region Methods

        /// <summary>
        /// The on value changed.
        /// </summary>
        /// <param name="oldValue">
        /// The old value.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            if (!this.withinChanging && !BindingOperations.IsDataBound(this, HueProperty))
            {
                this.withinChanging = true;
                this.Hue = newValue;
                this.withinChanging = false;
            }
        }

        /// <summary>
        /// The on hue property changed.
        /// </summary>
        /// <param name="relatedObject">
        /// The related object.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void OnHuePropertyChanged(DependencyObject relatedObject, DependencyPropertyChangedEventArgs e)
        {
            var spectrumSlider = relatedObject as SpectrumSlider;
            if (spectrumSlider != null && !spectrumSlider.withinChanging)
            {
                spectrumSlider.withinChanging = true;

                var hue = (double)e.NewValue;
                spectrumSlider.Value = hue;

                spectrumSlider.withinChanging = false;
            }
        }

        /// <summary>
        /// The set background.
        /// </summary>
        private void SetBackground()
        {
            var backgroundBrush = new LinearGradientBrush
                {
                   StartPoint = new Point(0.5, 1), EndPoint = new Point(0.5, 0) 
                };

            const int spectrumColorCount = 30;

            Color[] spectrumColors = ColorHelper.GetSpectrumColors(spectrumColorCount);
            for (int i = 0; i < spectrumColorCount; ++i)
            {
                double offset = i * 1.0 / spectrumColorCount;
                var gradientStop = new GradientStop(spectrumColors[i], offset);
                backgroundBrush.GradientStops.Add(gradientStop);
            }

            this.Background = backgroundBrush;
        }

        #endregion
    }
}