// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalPropertyControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemo
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
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

        public override ContentControl CreateErrorControl(PropertyItem pi, object instance, Tab tab, PropertyControlFactoryOptions options)
        {
            var dataErrorInfoInstance = instance as IDataErrorInfo;
            var notifyDataErrorInfoInstance = instance as INotifyDataErrorInfo;

            var errorControl = new ContentControl
            {
                ContentTemplate = options.ValidationErrorTemplate,
                Focusable = false
            };
            IValueConverter errorConverter;
            string propertyPath;
            object source = null;
            if (dataErrorInfoInstance != null)
            {
                errorConverter = new DataErrorInfoConverter(dataErrorInfoInstance, pi.PropertyName);
                propertyPath = pi.PropertyName;
                source = instance;
            }
            else
            {
                errorConverter = new NotifyDataErrorInfoConverter(notifyDataErrorInfoInstance, pi.PropertyName);
                propertyPath = nameof(tab.HasErrors);
                source = tab;
                notifyDataErrorInfoInstance.ErrorsChanged += (s, e) =>
                {
                    //tab.UpdateHasErrors(notifyDataErrorInfoInstance);
                    tab.UpdateTabForValidationResults(notifyDataErrorInfoInstance);
                };
            }

            var visibilityBinding = new Binding(propertyPath)
            {
                Converter = errorConverter,
                NotifyOnTargetUpdated = true,
#if !NET40
                ValidatesOnNotifyDataErrors = false,
#endif
                Source = source,
            };

            //var errorList = notifyDataErrorInfoInstance.GetErrors(pi.PropertyName);
            //if(errorList.Cast<object>().Any

            var contentBinding = new Binding(propertyPath)
            {
                Converter = errorConverter,
                //ConverterParameter=Severity.Warning,
#if !NET40
                ValidatesOnNotifyDataErrors = false,
#endif
                Source = source,
            };

            errorControl.SetBinding(UIElement.VisibilityProperty, visibilityBinding);

            // When the visibility of the error control is changed, updated the HasErrors of the tab
            errorControl.TargetUpdated += (s, e) =>
            {
                if (dataErrorInfoInstance != null)
                    tab.UpdateHasErrors(dataErrorInfoInstance);
            };
            errorControl.SetBinding(ContentControl.ContentProperty, contentBinding);
            return errorControl;
        }
    }
}