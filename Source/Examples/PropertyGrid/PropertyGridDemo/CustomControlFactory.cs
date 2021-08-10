// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalPropertyControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemo
{
    using System;
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

        /// <inheritdoc />
        public override FrameworkElement CreateControl(PropertyItem property, PropertyControlFactoryOptions options)
        {
            if (property.Is(typeof(DateTime)))
            {
                var dp = new DatePicker() { SelectedDateFormat = DatePickerFormat.Long, DisplayDateStart = DateTime.Now.AddDays(-7) };
                dp.SetBinding(DatePicker.SelectedDateProperty,
                    new Binding(property.Descriptor.Name) { ValidatesOnDataErrors = true });
                return dp;
            }

            return base.CreateControl(property, options);
        }

        /// <inheritdoc />
        public override ContentControl CreateErrorControl(PropertyItem pi, object instance, Tab tab, PropertyControlFactoryOptions options)
        {
            var dataErrorInfoInstance = instance as IDataErrorInfo;
            var notifyDataErrorInfoInstance = instance as INotifyDataErrorInfo;

            if (Application.Current.TryFindResource("ValidationErrorTemplateEx") != null)
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
                    UpdateTabForValidationResults(tab, notifyDataErrorInfoInstance);
                    //needed to refresh error control's binding also when error changes (i.e from Error to Warning)
                    errorControl.GetBindingExpression(ContentControl.ContentProperty).UpdateTarget();
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

            var warningBinding = new Binding(nameof(tab.HasWarnings))
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

            errorControl.SetBinding(ContentControl.ContentProperty, warningBinding);
            return errorControl;
        }

        /// <inheritdoc />
        public override void UpdateTabForValidationResults(Tab tab, object errorInfo)
        {
            if (errorInfo is INotifyDataErrorInfo ndei)
            {
                tab.HasErrors = tab.Groups.Any(g => g.Properties.Any(p => ndei.GetErrors(p.PropertyName).Cast<object>()
                   .Any(a => a != null && a.GetType() == typeof(ValidationResultEx) && ((ValidationResultEx)a).Severity == Severity.Error)));

                tab.HasWarnings = tab.Groups.Any(g => g.Properties.Any(p => ndei.GetErrors(p.PropertyName).Cast<object>()
                   .Any(a => a != null && a.GetType() == typeof(ValidationResultEx) && ((ValidationResultEx)a).Severity == Severity.Warning)));
            }
            else if (errorInfo is IDataErrorInfo dei)
            {
                tab.HasErrors = tab.Groups.Any(g => g.Properties.Any(p => !string.IsNullOrEmpty(dei[p.PropertyName])));
            }
        }

        /// <inheritdoc />
        public override void SetValidationErrorStyle(FrameworkElement control, PropertyControlFactoryOptions options)
        {
            if (Application.Current.TryFindResource("ErrorInToolTipStyleEx") != null)
            {
                options.ValidationErrorStyle = (Style)Application.Current.TryFindResource("ErrorInToolTipStyleEx");
                control.Style = options.ValidationErrorStyle;
            }
        }
    }
}