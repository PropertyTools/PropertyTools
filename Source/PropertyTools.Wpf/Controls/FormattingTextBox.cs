// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormattingTextBox.cs" company="PropertyTools">
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
//   Represents a TextBox with a bindable StringFormat property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a TextBox with a bindable StringFormat property.
    /// </summary>
    public class FormattingTextBox : TextBox
    {
        /// <summary>
        /// The format provider property.
        /// </summary>
        public static readonly DependencyProperty FormatProviderProperty = DependencyProperty.Register(
            "FormatProvider",
            typeof(IFormatProvider),
            typeof(FormattingTextBox),
            new UIPropertyMetadata(CultureInfo.InvariantCulture));

        /// <summary>
        /// The string format property.
        /// </summary>
        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(
            "StringFormat", typeof(string), typeof(FormattingTextBox), new UIPropertyMetadata(null, StringFormatChanged));

        /// <summary>
        /// The value property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(object),
            typeof(FormattingTextBox),
            new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChangedCallback));

        /// <summary>
        /// The user is changing.
        /// </summary>
        private bool userIsChanging = true;

        /// <summary>
        /// Initializes static members of the <see cref="FormattingTextBox" /> class.
        /// </summary>
        static FormattingTextBox()
        {
            // DefaultStyleKeyProperty.OverrideMetadata(typeof(FormattingTextBox), new FrameworkPropertyMetadata(typeof(FormattingTextBox)));
            TextProperty.OverrideMetadata(typeof(FormattingTextBox), new FrameworkPropertyMetadata(TextChangedCallback));
        }

        /// <summary>
        /// Gets or sets the format provider.
        /// </summary>
        /// <value> The format provider. </value>
        public IFormatProvider FormatProvider
        {
            get
            {
                return (IFormatProvider)this.GetValue(FormatProviderProperty);
            }

            set
            {
                this.SetValue(FormatProviderProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the string format.
        /// </summary>
        /// <value> The string format. </value>
        public string StringFormat
        {
            get
            {
                return (string)this.GetValue(StringFormatProperty);
            }

            set
            {
                this.SetValue(StringFormatProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value> The value. </value>
        public object Value
        {
            get
            {
                return this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// The string format changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void StringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FormattingTextBox)d).UpdateText();
        }

        /// <summary>
        /// The text changed callback.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ftb = (FormattingTextBox)d;

            // if (ftb.userIsChanging)
            ftb.UpdateValue();
        }

        /// <summary>
        /// The value changed callback.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ftb = (FormattingTextBox)d;
            if (ftb.userIsChanging)
            {
                ftb.UpdateText();
            }
        }

        /// <summary>
        /// Unformats the text.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The un format.
        /// </returns>
        private string UnFormat(string s)
        {
            if (this.StringFormat == null)
            {
                return s;
            }

            var r = new Regex(@"(.*)(\{0.*\})(.*)");
            var match = r.Match(this.StringFormat);
            if (!match.Success)
            {
                return s;
            }

            if (match.Groups.Count > 1 && !string.IsNullOrEmpty(match.Groups[1].Value))
            {
                s = s.Replace(match.Groups[1].Value, string.Empty);
            }

            if (match.Groups.Count > 3 && !string.IsNullOrEmpty(match.Groups[3].Value))
            {
                s = s.Replace(match.Groups[3].Value, string.Empty);
            }

            return s;
        }

        /// <summary>
        /// Updates the text.
        /// </summary>
        private void UpdateText()
        {
            this.userIsChanging = false;

            string f = this.StringFormat;
            if (f != null)
            {
                if (!f.Contains("{0"))
                {
                    f = string.Format("{{0:{0}}}", f);
                }

                this.Text = string.Format(this.FormatProvider, f, this.Value);
            }
            else
            {
                this.Text = this.Value != null ? string.Format(this.FormatProvider, "{0}", this.Value) : null;
            }

            this.userIsChanging = true;
        }

        /// <summary>
        /// Updates the value.
        /// </summary>
        private void UpdateValue()
        {
            if (this.userIsChanging)
            {
                this.Value = this.UnFormat(this.Text);
            }
        }
    }
}