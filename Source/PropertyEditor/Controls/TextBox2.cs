using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// (don't know what to call this)
    /// TextBox that updates the binding when Enter is pressed. Also moves focus to the next control.
    /// </summary>
    public class TextBox2 : TextBox
    {
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Enter && !AcceptsReturn)
            {
                // update binding
                var b = GetBindingExpression(TextProperty);
                if (b != null)
                {
                    b.UpdateSource();
                    b.UpdateTarget();
                }

                // Move focus to next element
                // http://madprops.org/blog/enter-to-tab-in-wpf/
                var uie = e.OriginalSource as UIElement;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
    }
}
