// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalPropertyControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemo
{
    using System.Windows;

    using ExampleLibrary;

    using PropertyTools.Wpf;

    public class CustomControlFactory : PropertyGridControlFactory
    {
        public CustomControlFactory()
        {
            this.Converters.Add(new PropertyConverter(typeof(Length), new LengthConverter()));
        }

        public override FrameworkElement CreateControl(PropertyItem pi, PropertyControlFactoryOptions options)
        {
            //if (property.Is(typeof(DateTime)))
            //{
            //    var dp = new DatePicker() { SelectedDateFormat = DatePickerFormat.Long, DisplayDateStart = DateTime.Now.AddDays(-7) };
            //    dp.SetBinding(DatePicker.SelectedDateProperty,
            //        new Binding(property.Descriptor.Name) { ValidatesOnDataErrors = true });
            //    return dp;
            //}

            return base.CreateControl(pi, options);
        }
    }
}