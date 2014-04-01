// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenGrab.cs" company="PropertyTools">
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
//   Provides a behavior that let you capture the contents of a control to a bitmap and copy it to the clipboard.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Provides a behavior that let you capture the contents of a control to a bitmap and copy it to the clipboard.
    /// </summary>
    public class ScreenGrab
    {
        /// <summary>
        /// Initializes static members of the <see cref="ScreenGrab"/> class.
        /// </summary>
        static ScreenGrab()
        {
            CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ScreenGrab), new PropertyMetadata(null, OnCommandChanged));
            GestureProperty = DependencyProperty.RegisterAttached("Gesture", typeof(KeyGesture), typeof(ScreenGrab), new PropertyMetadata(null, OnGestureCommandBindingChanged));
        }

        /// <summary>
        /// Gets the command property.
        /// </summary>
        /// <value>The command property.</value>
        public static DependencyProperty CommandProperty { get; private set; }

        /// <summary>
        /// Gets the gesture property.
        /// </summary>
        /// <value>The gesture property.</value>
        public static DependencyProperty GestureProperty { get; private set; }

        /// <summary>
        /// Gets the command property.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A KeyCommand.</returns>
        public static ICommand GetCommand(UIElement element)
        {
            return element != null ? (ICommand)element.GetValue(CommandProperty) : null;
        }

        /// <summary>
        /// Gets the gesture property.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A KeyGesture.</returns>
        public static KeyGesture GetGesture(UIElement element)
        {
            return element != null ? (KeyGesture)element.GetValue(GestureProperty) : null;
        }

        /// <summary>
        /// Sets the command property.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetCommand(UIElement element, ICommand value)
        {
            if (element != null)
            {
                element.SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// Sets the gesture property.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetGesture(UIElement element, KeyGesture value)
        {
            if (element != null)
            {
                element.SetValue(GestureProperty, value);
            }
        }

        /// <summary>
        /// Grabs the specified element and copies to clipboard.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="resolution">The resolution (dots per inch).</param>
        private static void Grab(Visual element, double resolution = 96)
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(element);
            var bitmap = new RenderTargetBitmap((int)(bounds.Width * resolution / 96.0), (int)(bounds.Height * resolution / 96.0), resolution, resolution, PixelFormats.Pbgra32);
            var dv = new DrawingVisual();
            using (var context = dv.RenderOpen())
            {
                context.DrawRectangle(Brushes.White, null, new Rect(bounds.Size));
                context.DrawRectangle(new VisualBrush(element), null, new Rect(bounds.Size));
            }

            bitmap.Render(dv);

            Clipboard.SetImage(bitmap);
        }

        /// <summary>
        /// Called when the command has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element != null)
            {
                var command = e.NewValue as ICommand;
                if (command != null)
                {
                    element.CommandBindings.Add(new DelegateCommandBinding(command, () => Grab(element)));
                }
            }
        }

        /// <summary>
        /// Called when gesture has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnGestureCommandBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null)
            {
                return;
            }

            var gesture = e.NewValue as KeyGesture;
            if (gesture != null)
            {
                element.InputBindings.Add(new KeyBinding(new DelegateCommand(() => Grab(element)), gesture));
            }
        }
    }
}