using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace PropertyTools.Wpf
{
    // http://bea.stollnitz.com/blog/?p=28
    // http://code.msdn.microsoft.com/wpfradiobuttonlist

    public class RadioButtonList : Control
    {
        private const string PartPanel = "PART_Panel";

        private StackPanel panel;

        public RadioButtonList()
        {
            DataContextChanged += RadioButtonList_DataContextChanged;
        }

        void RadioButtonList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateContent();
        }

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
            
            var enumValues = Enum.GetValues(Value.GetType()).FilterOnBrowsableAttribute();
            var converter = new EnumToBooleanConverter { EnumType = Value.GetType() };
            var relativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(RadioButtonList), 1);
            var descriptionConverter = new EnumDescriptionConverter();

            foreach (var itemValue in enumValues )
            {
                var rb = new RadioButton { Content = descriptionConverter.Convert(itemValue, typeof(string), null, CultureInfo.CurrentCulture) };
                // rb.IsChecked = Value.Equals(itemValue);

                var isCheckedBinding = new Binding("Value")
                                           {
                                               Converter = converter,
                                               ConverterParameter = itemValue,
                                               Mode = BindingMode.TwoWay,
                                               RelativeSource = relativeSource
                                           };
                rb.SetBinding(ToggleButton.IsCheckedProperty, isCheckedBinding);

                var itemMarginBinding = new Binding("ItemMargin") { RelativeSource = relativeSource };
                rb.SetBinding(MarginProperty, itemMarginBinding);

                panel.Children.Add(rb);
            }
        }

        static RadioButtonList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonList),
                new FrameworkPropertyMetadata(typeof(RadioButtonList)));
        }

        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(RadioButtonList), new UIPropertyMetadata(new Thickness(0, 4, 0, 4)));

        public Enum Value
        {
            get { return (Enum)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Enum), typeof(RadioButtonList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChanged));

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RadioButtonList)d).UpdateContent();
        }
    }

    //public class EnumRadioButton : RadioButton
    //{


    //    public Enum EnumValue
    //    {
    //        get { return (Enum)GetValue(EnumValueProperty); }
    //        set { SetValue(EnumValueProperty, value); }
    //    }

    //    public static readonly DependencyProperty EnumValueProperty =
    //        DependencyProperty.Register("EnumValue", typeof(object), typeof(EnumRadioButton), new UIPropertyMetadata(null, EnumChanged));

    //    private static void EnumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        ((EnumRadioButton)d).OnEnumChanged();
    //    }

    //    private void OnEnumChanged()
    //    {
    //        IsChecked = EnumValue == Value;
    //    }

    //    public Enum Value
    //    {
    //        get { return (Enum)GetValue(ValueProperty); }
    //        set { SetValue(ValueProperty, value); }
    //    }

    //    public static readonly DependencyProperty ValueProperty =
    //        DependencyProperty.Register("Value", typeof(Enum), typeof(EnumRadioButton), new UIPropertyMetadata(null, EnumChanged));

    //}
}
