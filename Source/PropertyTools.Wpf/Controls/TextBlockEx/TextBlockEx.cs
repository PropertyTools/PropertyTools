// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBlockEx.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a TextBlock than can be disabled.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a TextBlock than can be disabled.
    /// </summary>
    public class TextBlockEx : TextBlock
    {
        /// <summary>
        /// Initializes static members of the <see cref="TextBlockEx" /> class.
        /// </summary>
        static TextBlockEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBlockEx), new FrameworkPropertyMetadata(typeof(TextBlockEx)));
        }
    }
}