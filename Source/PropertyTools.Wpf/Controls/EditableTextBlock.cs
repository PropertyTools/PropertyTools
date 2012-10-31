// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableTextBlock.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Provides an editable text block.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// Provides an editable text block.
    /// </summary>
    public class EditableTextBlock : TextBlock
    {
        #region Constants and Fields

        /// <summary>
        ///   The is editing property.
        /// </summary>
        public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register(
            "IsEditing",
            typeof(bool),
            typeof(EditableTextBlock),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (s, e) => ((EditableTextBlock)s).IsEditingChanged()));

        /// <summary>
        ///   The oldfocus.
        /// </summary>
        private IInputElement oldfocus;

        /// <summary>
        ///   The text box.
        /// </summary>
        private TextBox textBox;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "EditableTextBlock" /> class.
        /// </summary>
        static EditableTextBlock()
        {
            TextProperty.OverrideMetadata(
                typeof(EditableTextBlock),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is editing.
        /// </summary>
        public bool IsEditing
        {
            get
            {
                return (bool)this.GetValue(IsEditingProperty);
            }

            set
            {
                this.SetValue(IsEditingProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Begins the edit.
        /// </summary>
        private void BeginEdit()
        {
            if (this.textBox != null)
            {
                throw new InvalidOperationException();
            }

            var scope = FocusManager.GetFocusScope(this);
            this.oldfocus = FocusManager.GetFocusedElement(scope);
            this.textBox = new TextBox();
            this.textBox.SetBinding(
                TextBox.TextProperty,
                new Binding("Text") { Source = this, UpdateSourceTrigger = UpdateSourceTrigger.Explicit });
            Grid.SetColumn(this.textBox, Grid.GetColumn(this));
            Grid.SetColumnSpan(this.textBox, Grid.GetColumnSpan(this));
            this.Visibility = Visibility.Collapsed;
            var p = this.Parent as Panel;
            if (p != null)
            {
                int index = p.Children.IndexOf(this);
                p.Children.Insert(index, this.textBox);
            }

            this.textBox.LostFocus += this.TextBoxLostFocus;
            this.textBox.KeyDown += this.TextBoxKeyDown;
            this.textBox.CaretIndex = this.textBox.Text.Length;
            this.textBox.SelectAll();
            this.textBox.Focus();
        }

        /// <summary>
        /// Ends the edit.
        /// </summary>
        /// <param name="commit">
        /// if set to <c>true</c> [commit].
        /// </param>
        private void EndEdit(bool commit)
        {
            if (this.textBox == null)
            {
                return;
            }

            BindingExpression be = this.textBox.GetBindingExpression(TextBox.TextProperty);
            if (commit)
            {
                be.UpdateSource();
            }

            this.IsEditing = false;
            var p = this.Parent as Panel;
            p.Children.Remove(this.textBox);
            this.textBox = null;
            this.Visibility = Visibility.Visible;
            if (this.oldfocus != null)
            {
                this.oldfocus.Focus();
            }
        }

        /// <summary>
        /// Handles changes in IsEditing.
        /// </summary>
        private void IsEditingChanged()
        {
            if (this.IsEditing)
            {
                this.BeginEdit();
            }
            else
            {
                this.EndEdit(true);
            }
        }

        /// <summary>
        /// TextBox key down handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.
        /// </param>
        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.EndEdit(true);
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                this.EndEdit(false);
                e.Handled = true;
            }
        }

        /// <summary>
        /// TextBox lost focus handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            this.EndEdit(true);
        }

        #endregion
    }
}