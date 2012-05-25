// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPicker2.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Represents a control that lets the user pick a color.
    /// </summary>
    [TemplatePart(Name = PartColorPickerPanel, Type = typeof(ColorPickerPanel))]
    public class ColorPicker2 : ComboBox
    {

        #region Constants and Fields

        /// <summary>
        ///   The selected color property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            "SelectedColor", 
            typeof(Color?), 
            typeof(ColorPicker2), 
            new FrameworkPropertyMetadata(
                Color.FromArgb(0, 0, 0, 0), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                SelectedColorChanged, 
                CoerceSelectedColorValue));

        /// <summary>
        ///   The color picker panel part constant.
        /// </summary>
        private const string PartColorPickerPanel = "PART_ColorPickerPanel";

        /// <summary>
        ///   The color picker panel.
        /// </summary>
        private ColorPickerPanel colorPickerPanel;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "ColorPicker2" /> class.
        /// </summary>
        static ColorPicker2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ColorPicker2), new FrameworkPropertyMetadata(typeof(ColorPicker2)));
            SelectedValueProperty.OverrideMetadata(
                typeof(ColorPicker2), 
                new FrameworkPropertyMetadata(
                    Color.FromArgb(0, 0, 0, 0), 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                    SelectedColorChanged, 
                    CoerceSelectedColorValue));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the selected color.
        /// </summary>
        /// <value>The color of the selected.</value>
        public Color? SelectedColor
        {
            get
            {
                return (Color?)this.GetValue(SelectedColorProperty);
            }

            set
            {
                this.SetValue(SelectedColorProperty, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Called when <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/> is called.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.colorPickerPanel = this.GetTemplateChild(PartColorPickerPanel) as ColorPickerPanel;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Coerces the selected color value.
        /// </summary>
        /// <param name="basevalue">
        /// The basevalue.
        /// </param>
        /// <returns>
        /// The coerce selected color value.
        /// </returns>
        protected virtual object CoerceSelectedColorValue(object basevalue)
        {
            if (basevalue == null)
            {
                return this.SelectedColor;
            }

            return basevalue;
        }

        /// <summary>
        /// Reports when a combo box's popup opens.
        /// </summary>
        /// <param name="e">
        /// The event data for the <see cref="E:System.Windows.Controls.ComboBox.DropDownOpened"/> event.
        /// </param>
        protected override void OnDropDownOpened(EventArgs e)
        {
            base.OnDropDownOpened(e);
            this.colorPickerPanel.Focus();
        }

        /// <summary>
        /// Handles changes in selected color.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnSelectedColorChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Coerces the selected color value.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="basevalue">
        /// The basevalue.
        /// </param>
        /// <returns>
        /// The coerce selected color value.
        /// </returns>
        private static object CoerceSelectedColorValue(DependencyObject d, object basevalue)
        {
            return ((ColorPicker2)d).CoerceSelectedColorValue(basevalue);
        }

        /// <summary>
        /// Handles changes in selected color.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private static void SelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker2)d).OnSelectedColorChanged(e);
        }

        #endregion
    }
}