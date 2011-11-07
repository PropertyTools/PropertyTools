// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StackPanelEx.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents a stack panel that counts the number of visible children.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a stack panel that counts the number of visible children.
    /// </summary>
    public class StackPanelEx : StackPanel
    {
        #region Constants and Fields

        /// <summary>
        /// The visible children count property.
        /// </summary>
        public static readonly DependencyProperty VisibleChildrenCountProperty =
            DependencyProperty.Register(
                "VisibleChildrenCount", typeof(int), typeof(StackPanelEx), new UIPropertyMetadata(-1));

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the number of visible children.
        /// </summary>
        /// <value>
        ///   The visible children count.
        /// </value>
        public int VisibleChildrenCount
        {
            get
            {
                return (int)this.GetValue(VisibleChildrenCountProperty);
            }

            private set
            {
                this.SetValue(VisibleChildrenCountProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Arranges the content of a <see cref="T:System.Windows.Controls.StackPanel"/> element.
        /// </summary>
        /// <param name="arrangeSize">
        /// The <see cref="T:System.Windows.Size"/> that this element should use to arrange its child elements.
        /// </param>
        /// <returns>
        /// The <see cref="T:System.Windows.Size"/> that represents the arranged size of this <see cref="T:System.Windows.Controls.StackPanel"/> element and its child elements.
        /// </returns>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            // Arrange is called when Visibility of children is changed, even when the parent object is not visible.
            int count = 0;
            foreach (UIElement child in this.Children)
            {
                if (child.Visibility == Visibility.Visible)
                {
                    count++;
                }
            }

            this.VisibleChildrenCount = count;

            return base.ArrangeOverride(arrangeSize);
        }

        #endregion
    }
}