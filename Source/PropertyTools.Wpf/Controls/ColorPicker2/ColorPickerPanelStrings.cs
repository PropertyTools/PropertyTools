// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPickerPanelStrings.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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
//   The color picker panel strings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    /// <summary>
    /// The color picker panel strings.
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
            this.Values = "Values";
            this.HSV = "HSV";
            this.ColorPickerToolTip = "Toggle this button and pick a color by pressing 'shift'.";
        }

        /// <summary>
        /// Gets or sets ColorPickerToolTip.
        /// </summary>
        public string ColorPickerToolTip { get; set; }

        /// <summary>
        /// Gets or sets HSV.
        /// </summary>
        public string HSV { get; set; }

        /// <summary>
        /// Gets or sets RecentColors.
        /// </summary>
        public string RecentColors { get; set; }

        /// <summary>
        /// Gets or sets StandardColors.
        /// </summary>
        public string StandardColors { get; set; }

        /// <summary>
        /// Gets or sets ThemeColors.
        /// </summary>
        public string ThemeColors { get; set; }

        /// <summary>
        /// Gets or sets Values.
        /// </summary>
        public string Values { get; set; }
    }
}