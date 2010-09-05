using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PropertyEditorLibrary
{
    // http://bea.stollnitz.com/blog/?p=28
    // http://code.msdn.microsoft.com/wpfradiobuttonlist

    public class RadioButtonList : Control
    {
        private const string PartPanel = "PART_Panel";

        private StackPanel panel;

        public override void OnApplyTemplate()
        {
            if (panel == null)
            {
                panel = Template.FindName(PartPanel, this) as StackPanel;
            }
            UpdateContent();
        }

        private void UpdateContent()
        {
            if (panel == null)
                return;
            panel.Children.Clear();
            if (Value == null)
                return;
            var values = Enum.GetValues(Value.GetType());
            foreach (var value in values)
            {
                var rb = new RadioButton { Content = value, IsChecked = value == Value, Margin = new Thickness(0, 4, 0, 4) };
                var b = new Binding("Value") { Converter = new EnumToBooleanConverter(), ConverterParameter = value };
                
                // todo: binding doesn't work?
                rb.SetBinding(RadioButton.IsCheckedProperty, b);
                panel.Children.Add(rb);
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
            DependencyProperty.Register("Value", typeof(Enum), typeof(RadioButtonList), new UIPropertyMetadata(null, ValueChanged));

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
            ((EnumRadioButton)d).OnEnumChanged();
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
            DependencyProperty.Register("Value", typeof(Enum), typeof(EnumRadioButton), new UIPropertyMetadata(null, EnumChanged));

    }
}
