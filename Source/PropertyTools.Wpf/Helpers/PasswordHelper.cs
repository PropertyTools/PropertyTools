// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordHelper.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   The password helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Security;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The password helper.
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Identifies the <see cref="Attach"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached(
            "Attach", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

        /// <summary>
        /// Identifies the Password attached dependency property.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password",
            typeof(string),
            typeof(PasswordHelper),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        /// <summary>
        /// Identifies the IsUpdating attached dependency property.
        /// </summary>
        private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached(
            "IsUpdating", typeof(bool), typeof(PasswordHelper));

        /// <summary>
        /// Gets the attach state.
        /// </summary>
        /// <param name="dp">The dp.</param>
        /// <returns>
        /// <c>true</c> if the control is attached.
        /// </returns>
        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <param name="dp">The dp.</param>
        /// <returns>The password.</returns>
        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        /// <summary>
        /// Attaches the behaviour.
        /// </summary>
        /// <param name="dp">The dp.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        /// <summary>
        /// The set password.
        /// </summary>
        /// <param name="dp">The dp.</param>
        /// <param name="value">The value.</param>
        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        /// <summary>
        /// Attaches the password changed event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;

            if (passwordBox == null)
            {
                return;
            }

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        /// <summary>
        /// The get is updating.
        /// </summary>
        /// <param name="dp">The dp.</param>
        /// <returns>
        /// The get is updating.
        /// </returns>
        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        /// <summary>
        /// The on password property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!GetIsUpdating(passwordBox))
            {
                if (e.NewValue is SecureString)
                {
                    // passwordBox.SecurePassword = (SecureString)e.NewValue;
                }
                else
                {
                    passwordBox.Password = (string)e.NewValue;
                }
            }

            passwordBox.PasswordChanged += PasswordChanged;
        }

        /// <summary>
        /// The password changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }

        /// <summary>
        /// The set is updating.
        /// </summary>
        /// <param name="dp">The dp.</param>
        /// <param name="value">The value.</param>
        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }
    }
}