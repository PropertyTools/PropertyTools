namespace PropertyControlDemo
{
    using System.Windows;

    using PropertyTools.Wpf;

    public class LocalPropertyControlFactory : IPropertyControlFactory
    {
        public FrameworkElement CreateControl(PropertyItem pi, PropertyControlFactoryOptions options)
        {
            //if (property.Is(typeof(DateTime)))
            //{
            //    var dp = new DatePicker() { SelectedDateFormat = DatePickerFormat.Long, DisplayDateStart = DateTime.Now.AddDays(-7) };
            //    dp.SetBinding(DatePicker.SelectedDateProperty,
            //        new Binding(property.Descriptor.Name) { ValidatesOnDataErrors = true });
            //    return dp;
            //}

            return null;
        }
    }
}