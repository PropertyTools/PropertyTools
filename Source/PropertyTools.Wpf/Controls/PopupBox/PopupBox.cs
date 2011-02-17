using System.Windows;
using System.Windows.Controls;

namespace PropertyTools.Wpf
{
    public class PopupBox : ComboBox
    {
        static PopupBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupBox), new FrameworkPropertyMetadata(typeof(PopupBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public DataTemplate PopupTemplate
        {
            get { return (DataTemplate)GetValue(PopupTemplateProperty); }
            set { SetValue(PopupTemplateProperty, value); }
        }

        public static readonly DependencyProperty PopupTemplateProperty =
            DependencyProperty.Register("PopupTemplate", typeof(DataTemplate), typeof(PopupBox), new UIPropertyMetadata(null));

    }

}