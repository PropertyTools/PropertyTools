// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPickerPanelStrings.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The color picker panel strings.
    /// </summary>
    public class ColorPickerPanelStrings
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ColorPickerPanelStrings" /> class.
        /// </summary>
        public ColorPickerPanelStrings()
        {
            this.ThemeColors = "Theme colors";
            this.StandardColors = "Standard colors";
            this.RecentColors = "Recent colors";
            this.Values = "Values";
            this.HSV = "HSV";
            this.ColorPickerToolTip = "Toggle this button and pick a color by pressing 'shift'.";
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets ColorPickerToolTip.
        /// </summary>
        public string ColorPickerToolTip { get; set; }

        /// <summary>
        ///   Gets or sets HSV.
        /// </summary>
        public string HSV { get; set; }

        /// <summary>
        ///   Gets or sets RecentColors.
        /// </summary>
        public string RecentColors { get; set; }

        /// <summary>
        ///   Gets or sets StandardColors.
        /// </summary>
        public string StandardColors { get; set; }

        /// <summary>
        ///   Gets or sets ThemeColors.
        /// </summary>
        public string ThemeColors { get; set; }

        /// <summary>
        ///   Gets or sets Values.
        /// </summary>
        public string Values { get; set; }

        #endregion
    }
}