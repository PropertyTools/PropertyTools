// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PopupBox.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a popup control that provides a data template for the popup.
    /// </summary>
    public class PopupBox : ComboBox
    {
        #region Constants and Fields

        /// <summary>
        /// The popup template property.
        /// </summary>
        public static readonly DependencyProperty PopupTemplateProperty = DependencyProperty.Register(
            "PopupTemplate", typeof(DataTemplate), typeof(PopupBox), new UIPropertyMetadata(null));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="PopupBox"/> class. 
        /// </summary>
        static PopupBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupBox), new FrameworkPropertyMetadata(typeof(PopupBox)));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the popup template.
        /// </summary>
        /// <value>The popup template.</value>
        public DataTemplate PopupTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(PopupTemplateProperty);
            }

            set
            {
                this.SetValue(PopupTemplateProperty, value);
            }
        }

        #endregion
    }
}