using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// TextBox that updates the binding when Enter is pressed. Also moves focus to the next control.
    /// Todo: replace by behaviour or attached dependency property?
    /// </summary>
    public class TextBoxEx : TextBox
    {
        public bool UpdateBindingOnEnter
        {
            get { return (bool)GetValue(UpdateBindingOnEnterProperty); }
            set { SetValue(UpdateBindingOnEnterProperty, value); }
        }

        public static readonly DependencyProperty UpdateBindingOnEnterProperty =
            DependencyProperty.Register("UpdateBindingOnEnter", typeof(bool), typeof(TextBoxEx), new UIPropertyMetadata(true));


        public bool MoveFocusOnEnter
        {
            get { return (bool)GetValue(MoveFocusOnEnterProperty); }
            set { SetValue(MoveFocusOnEnterProperty, value); }
        }

        public static readonly DependencyProperty MoveFocusOnEnterProperty =
            DependencyProperty.Register("MoveFocusOnEnter", typeof(bool), typeof(TextBoxEx), new UIPropertyMetadata(true));

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Enter && !AcceptsReturn)
            {
                if (UpdateBindingOnEnter)
                {
                    // update binding
                    var b = GetBindingExpression(TextProperty);
                    if (b != null)
                    {
                        b.UpdateSource();
                        b.UpdateTarget();
                    }
                }

                if (MoveFocusOnEnter)
                {
                    // Move focus to next element
                    // http://madprops.org/blog/enter-to-tab-in-wpf/
                    var uie = e.OriginalSource as UIElement;
                    if (uie != null) 
                        uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }
    }
/*    Microsoft.Expression.Interactivity
    public class UpdateOnEnterBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.OnPreviewKeyDown += PreviewKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.OnPreviewKeyDown -= PreviewKeyDown;
        }

        private void PreviewKeyDown(object sender,
            System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (e.Key == Key.Enter && !AcceptsReturn)
            {
                    // update binding
                    var b = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
                    if (b != null)
                    {
                        b.UpdateSource();
                        b.UpdateTarget();
                    }
            }
        }
    }*/
}
