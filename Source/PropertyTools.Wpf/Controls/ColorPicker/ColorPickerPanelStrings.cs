// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPickerPanelStrings.cs" company="PropertyTools">
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