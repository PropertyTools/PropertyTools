// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WpfTestBase.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Threading;

    using NUnit.Framework;

    /// <summary>
    /// Base class for headless in-process WPF component tests.
    /// Provides STA apartment setup, dispatcher pumping and layout helpers so that
    /// controls can be tested without showing a window (works on headless CI runners).
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [Category("WpfHeadless")]
    public abstract class WpfTestBase
    {
        /// <summary>
        /// Applies the template and forces a full layout pass on the specified element,
        /// so that the visual tree (including item containers) is created without showing a window.
        /// </summary>
        /// <param name="element">The element to lay out.</param>
        /// <param name="width">The available width.</param>
        /// <param name="height">The available height.</param>
        protected static void PrepareForLayout(FrameworkElement element, double width = 800, double height = 600)
        {
            element.ApplyTemplate();
            element.Measure(new Size(width, height));
            element.Arrange(new Rect(0, 0, width, height));
            element.UpdateLayout();
        }

        /// <summary>
        /// Pumps the dispatcher queue so that pending background operations
        /// (e.g. data binding updates and generated containers) are processed.
        /// </summary>
        protected static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new DispatcherOperationCallback(f => { ((DispatcherFrame)f).Continue = false; return null; }),
                frame);
            Dispatcher.PushFrame(frame);
        }

        /// <summary>
        /// Finds all visual children of the specified type in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of children to find.</typeparam>
        /// <param name="parent">The root of the visual tree to search.</param>
        /// <returns>The children of the specified type.</returns>
        protected static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
            {
                yield break;
            }

            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                {
                    yield return typedChild;
                }

                foreach (var descendant in FindVisualChildren<T>(child))
                {
                    yield return descendant;
                }
            }
        }
    }
}
