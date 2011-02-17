using System;
using System.Windows;
using System.Windows.Controls;

namespace PropertyTools.Wpf
{
    public class EnumMenuItem : MenuItem
    {
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(Enum), typeof(EnumMenuItem), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedValueChanged));

        public Enum SelectedValue
        {
            get { return (Enum)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        private static void SelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EnumMenuItem)d).OnSelectedValueChanged();
        }

        private void OnSelectedValueChanged()
        {
            Items.Clear();
            if (SelectedValue == null)
            {
                return;
            }

            var enumType = SelectedValue.GetType();
            foreach (var value in Enum.GetValues(enumType))
            {
                var mi = new MenuItem { Header = value.ToString(), Tag = value };
                if (value.Equals(SelectedValue))
                {
                    mi.IsChecked = true;
                }

                mi.Click += ItemClick;
                Items.Add(mi);
            }
        }

        private void ItemClick(object sender, RoutedEventArgs e)
        {
            var newValue = ((MenuItem)sender).Tag as Enum;
            if (newValue != null && !newValue.Equals(SelectedValue))
            {
                SelectedValue = newValue;
            }
        }
    }
}