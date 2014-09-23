// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPickerPanelStrings.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides localized strings for the color picker panel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Provides localized strings for the color picker panel.
    /// </summary>
    public class ColorPickerPanelStrings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPickerPanelStrings" /> class.
        /// </summary>
        public ColorPickerPanelStrings()
        {
            this.ThemeColors = "Theme colors";
            this.StandardColors = "Standard colors";
            this.RecentColors = "Recent colors";
            this.OpacityVariations = "Opacity variations";
            this.Values = "Values";
            this.HSV = "HSV";
            this.ColorPickerToolTip = "Toggle this button and pick a color by pressing 'shift'.";
            this.TogglePanelToolTip = "Toggle between palette and color value panels.";
        }

        /// <summary>
        /// Gets or sets the tool tip for the 'color picker' button.
        /// </summary>
        public string ColorPickerToolTip { get; set; }

        /// <summary>
        /// Gets or sets the tool tip for the 'toggle panel' button.
        /// </summary>
        public string TogglePanelToolTip { get; set; }

        /// <summary>
        /// Gets or sets the 'HSV' string.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string HSV { get; set; }

        /// <summary>
        /// Gets or sets 'RecentColors' string.
        /// </summary>
        public string RecentColors { get; set; }

        /// <summary>
        /// Gets or sets the 'Opacity variations' string.
        /// </summary>
        public string OpacityVariations { get; set; }

        /// <summary>
        /// Gets or sets the 'Standard colors' string.
        /// </summary>
        public string StandardColors { get; set; }

        /// <summary>
        /// Gets or sets the 'Theme colors' string.
        /// </summary>
        public string ThemeColors { get; set; }

        /// <summary>
        /// Gets or sets the 'Values' string.
        /// </summary>
        public string Values { get; set; }
    }
}