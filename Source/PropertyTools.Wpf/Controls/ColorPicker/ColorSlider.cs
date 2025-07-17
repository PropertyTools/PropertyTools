// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorSlider.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a color slider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Represents a color slider.
    /// </summary>
    /// <remarks>Original code by Ury Jamshy, 21 July 2011.
    /// The Code Project Open License (CPOL)</remarks>
    public class ColorSlider : SliderEx
    {
        /// <summary>
        /// Identifies the <see cref="LeftColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LeftColorProperty = DependencyProperty.Register(
            nameof(LeftColor),
            typeof(Color?),
            typeof(ColorSlider),
            new UIPropertyMetadata(Colors.Black));

        /// <summary>
        /// Identifies the <see cref="RightColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RightColorProperty = DependencyProperty.Register(
            nameof(RightColor),
            typeof(Color?),
            typeof(ColorSlider),
            new UIPropertyMetadata(Colors.White));

        /// <summary>
        /// Initializes static members of the <see cref="ColorSlider" /> class.
        /// </summary>
        static ColorSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ColorSlider), new FrameworkPropertyMetadata(typeof(ColorSlider)));
        }

        /// <summary>
        /// Gets or sets the left color.
        /// </summary>
        public Color? LeftColor
        {
            get
            {
                return (Color)this.GetValue(LeftColorProperty);
            }

            set
            {
                this.SetValue(LeftColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the right color.
        /// </summary>
        public Color? RightColor
        {
            get
            {
                return (Color?)this.GetValue(RightColorProperty);
            }

            set
            {
                this.SetValue(RightColorProperty, value);
            }
        }
    }
}