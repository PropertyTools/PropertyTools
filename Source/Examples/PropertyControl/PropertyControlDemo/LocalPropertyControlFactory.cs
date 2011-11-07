namespace PropertyControlDemo
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using PropertyTools.Wpf;

    public class LocalPropertyControlFactory : IPropertyControlFactory
    {
        public FrameworkElement CreateControl(PropertyItem pi, PropertyControlFactoryOptions options)
        {
            //if (pi.Is(typeof(DateTime)))
            //{
            //    var dp = new DatePicker() { SelectedDateFormat = DatePickerFormat.Long, DisplayDateStart = DateTime.Now.AddDays(-7) };
            //    dp.SetBinding(DatePicker.SelectedDateProperty,
            //        new Binding(pi.Descriptor.Name) { ValidatesOnDataErrors = true });
            //    return dp;
            //}

            return null;
        }
    }
}