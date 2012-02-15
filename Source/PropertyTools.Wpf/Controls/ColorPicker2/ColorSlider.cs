// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorSlider.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Represents a color slider.
    /// </summary>
    /// <remarks>
    /// Original code by Ury Jamshy, 21 July 2011.
    ///   See http://www.codeproject.com/KB/WPF/ColorPicker2010.aspx
    ///   The Code Project Open License (CPOL)
    ///   http://www.codeproject.com/info/cpol10.aspx
    /// </remarks>
    public class ColorSlider : SliderEx
    {
        #region Constants and Fields

        /// <summary>
        ///   The left color property.
        /// </summary>
        public static readonly DependencyProperty LeftColorProperty = DependencyProperty.Register(
            "LeftColor", typeof(Color?), typeof(ColorSlider), new UIPropertyMetadata(Colors.Black));

        /// <summary>
        ///   The right color property.
        /// </summary>
        public static readonly DependencyProperty RightColorProperty = DependencyProperty.Register(
            "RightColor", typeof(Color?), typeof(ColorSlider), new UIPropertyMetadata(Colors.White));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "ColorSlider" /> class.
        /// </summary>
        static ColorSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ColorSlider), new FrameworkPropertyMetadata(typeof(ColorSlider)));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the left color.
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
        ///   Gets or sets the right color.
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

        #endregion
    }
}