// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalPropertyControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemo
{
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using ExampleLibrary;
    using PropertyTools.Wpf;

    public class CustomControlFactory : PropertyGridControlFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomControlFactory"/> class.
        /// </summary>
        public CustomControlFactory()
        {
            this.Converters.Add(new PropertyConverter(typeof(Length), new LengthConverter()));
        }

        /// <summary>
        /// Creates the control.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates the error control.
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="instance"></param>
        /// <param name="tab"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override ContentControl CreateErrorControl(PropertyItem pi, object instance, Tab tab, PropertyControlFactoryOptions options)
        {
            var dataErrorInfoInstance = instance as IDataErrorInfo;
            var notifyDataErrorInfoInstance = instance as INotifyDataErrorInfo;

            if(Application.Current.TryFindResource("ValidationErrorTemplateEx")!=null)
            {
                options.ValidationErrorTemplate = (DataTemplate)Application.Current.TryFindResource("ValidationErrorTemplateEx");
            }

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
                    tab.UpdateHasErrors(notifyDataErrorInfoInstance);
                    //UpdateTabForValidationResults(notifyDataErrorInfoInstance);
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


            var contentBinding = new Binding(propertyPath)
            {
                Converter = errorConverter,
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

        /// <summary>
        /// Updates the tab for validation results, to include Errors and Warngins both
        /// </summary>
        /// <param name="errorInfo">The error information.</param>
        private void UpdateTabForValidationResults(Tab tab,INotifyDataErrorInfo errorInfo)
        {
            //properties using ValidationResultEx
            tab.HasErrors = tab.Groups.Any(g => g.Properties.Any(p => errorInfo.GetErrors(p.PropertyName).Cast<object>()
               .Any(a => a.GetType() == typeof(ValidationResultEx) && ((ValidationResultEx)a).Severity == Severity.Error)));

            tab.HasWarnings = tab.Groups.Any(g => g.Properties.Any(p => errorInfo.GetErrors(p.PropertyName).Cast<object>()
               .Any(a => a.GetType() == typeof(ValidationResultEx) && ((ValidationResultEx)a).Severity == Severity.Warning)));           
        }
    }
}