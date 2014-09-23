// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckMark.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a control that shows a check mark (not editable CheckBox without the box).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a control that shows a check mark (not editable CheckBox without the box).
    /// </summary>
    public class CheckMark : Control
    {
        /// <summary>
        /// Identifies the <see cref="IsChecked"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
            "IsChecked", typeof(bool), typeof(CheckMark), new PropertyMetadata(false));

        /// <summary>
        /// Initializes static members of the <see cref="CheckMark" /> class.
        /// </summary>
        static CheckMark()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CheckMark), new FrameworkPropertyMetadata(typeof(CheckMark)));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value><c>true</c> if this instance is checked; otherwise, <c>false</c>.</value>
        public bool IsChecked
        {
            get
            {
                return (bool)this.GetValue(IsCheckedProperty);
            }

            set
            {
                this.SetValue(IsCheckedProperty, value);
            }
        }
    }
}