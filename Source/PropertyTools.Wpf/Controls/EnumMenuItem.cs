// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumMenuItem.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a menu item with a SelectedValue property that can bind to Enum values.
    /// </summary>
    public class EnumMenuItem : MenuItem
    {
        #region Constants and Fields

        /// <summary>
        /// The selected value property.
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
            "SelectedValue", 
            typeof(object), 
            typeof(EnumMenuItem), 
            new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedValueChanged));

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the selected value.
        /// </summary>
        /// <value>The selected value.</value>
        public object SelectedValue
        {
            get
            {
                return this.GetValue(SelectedValueProperty);
            }

            set
            {
                this.SetValue(SelectedValueProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The selected value changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void SelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EnumMenuItem)d).OnSelectedValueChanged();
        }

        /// <summary>
        /// Called when a menu item is clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void ItemClick(object sender, RoutedEventArgs e)
        {
            var newValue = ((MenuItem)sender).Tag as Enum;
            if (newValue != null && !newValue.Equals(this.SelectedValue))
            {
                this.SelectedValue = newValue;
            }
        }

        /// <summary>
        /// Called when selected value changed.
        /// </summary>
        private void OnSelectedValueChanged()
        {
            this.Items.Clear();
            if (this.SelectedValue == null)
            {
                return;
            }

            var enumType = this.SelectedValue.GetType();
            foreach (var value in Enum.GetValues(enumType))
            {
                var mi = new MenuItem { Header = value.ToString(), Tag = value };
                if (value.Equals(this.SelectedValue))
                {
                    mi.IsChecked = true;
                }

                mi.Click += this.ItemClick;
                this.Items.Add(mi);
            }
        }

        #endregion
    }
}