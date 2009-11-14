using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenControls
{
    // http://bea.stollnitz.com/blog/?p=28

    public class RadioButtonList : Control
    {
        private const string PartPanel = "PART_Panel";

        private StackPanel _panel;
      
        public override void OnApplyTemplate()
        {
            if (_panel == null)
            {
                _panel = Template.FindName(PartPanel, this) as StackPanel;
            }
            UpdateContent();
        }

        private void UpdateContent()
        {
            if (_panel==null)
                return;
            _panel.Children.Clear();
            if (Value==null)
                return;
            var values=Enum.GetValues(Value.GetType());
            foreach (var value in values)
            {
                var rb = new RadioButton() {Content = value, IsChecked = value == Value};
                var b = new Binding("Value") {Converter = new EnumToBooleanConverter(), ConverterParameter = value};
                // todo: binding doesn't work?
                rb.SetBinding(RadioButton.IsCheckedProperty, b);
                _panel.Children.Add(rb);
            }
        }

        static RadioButtonList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonList), new FrameworkPropertyMetadata(typeof(RadioButtonList)));
        }

        public Enum Value
        {
            get { return (Enum)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Enum), typeof(RadioButtonList), new UIPropertyMetadata(null,ValueChanged));

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RadioButtonList)d).UpdateContent();
        }
    }

    public class EnumRadioButton : RadioButton
    {


        public Enum EnumValue
        {
            get { return (Enum)GetValue(EnumValueProperty); }
            set { SetValue(EnumValueProperty, value); }
        }

        public static readonly DependencyProperty EnumValueProperty =
            DependencyProperty.Register("EnumValue", typeof(object), typeof(EnumRadioButton), new UIPropertyMetadata(null, EnumChanged));

        private static void EnumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EnumRadioButton) d).OnEnumChanged();
        }

        private void OnEnumChanged()
        {
            IsChecked = EnumValue == Value;
        }

        public Enum Value
        {
            get { return (Enum)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Enum), typeof(EnumRadioButton), new UIPropertyMetadata(null,EnumChanged));

    }
}
