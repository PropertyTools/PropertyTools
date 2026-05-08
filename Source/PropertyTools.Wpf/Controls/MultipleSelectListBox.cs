using PropertyTools.Wpf.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PropertyTools.Wpf.Controls
{
    public class MultipleSelectListBox : ListBox
    {
        public static readonly DependencyProperty BindableSelectedItemsProperty =
            DependencyProperty.Register("BindableSelectedItems",
                typeof(IEnumerable<object>), typeof(MultipleSelectListBox),
                new FrameworkPropertyMetadata(default(IEnumerable<object>),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindableSelectedItemsChanged));

        public IEnumerable<object> BindableSelectedItems
        {
            get => (IEnumerable<object>)GetValue(BindableSelectedItemsProperty);
            set => SetValue(BindableSelectedItemsProperty, value);
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            BindableSelectedItems = SelectedItems.Cast<object>();
        }

        private static void OnBindableSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultipleSelectListBox listBox)
                listBox.SetSelectedItems(listBox.BindableSelectedItems);
        }


        /// <summary>
        /// multi-select control item to selected state converter (for example, checkbox list)
        /// </summary>
        [ValueConversion(typeof(IList), typeof(IEnumerable<object>))]
        public class ListToBindableSelectedItemsConverter : IValueConverter
        {
            private readonly ISelectorDefinition _selectorDefinition;

            public ListToBindableSelectedItemsConverter(ISelectorDefinition selectorDefinition)
            {
                _selectorDefinition = selectorDefinition;
            }

            /// <summary>
            /// Converts a value.
            /// </summary>
            /// <param name="value">The value produced by the binding source.</param>
            /// <param name="targetType">The type of the binding target property.</param>
            /// <param name="parameter">The converter parameter to use.</param>
            /// <param name="culture">The culture to use in the converter.</param>
            /// <returns>
            /// A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
            /// </returns>
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value == null)
                {
                    return Enumerable.Empty<object>();
                }

                if (value is IList sourceList)
                {
                    if (!string.IsNullOrEmpty(_selectorDefinition.SelectedValuePath))
                    {
                        return _selectorDefinition.ItemsSource.Cast<object>()
                            .Where(x =>
                                ReflectionExtensions.TryGetFieldOrPropertyValue(x, _selectorDefinition.SelectedValuePath, out object objTargetValue)
                                && sourceList.Contains(objTargetValue)
                            );
                    }
                    else
                    {
                        return DependencyProperty.UnsetValue;
                    }
                }

                return DependencyProperty.UnsetValue;
            }

            /// <summary>
            /// Converts a value.
            /// </summary>
            /// <param name="value">The value that is produced by the binding target.</param>
            /// <param name="targetType">The type to convert to.</param>
            /// <param name="parameter">The converter parameter to use.</param>
            /// <param name="culture">The culture to use in the converter.</param>
            /// <returns>
            /// A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
            /// </returns>
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is IEnumerable<object> selectedItems)
                {
                    var result = Activator.CreateInstance(targetType) as IList;

                    try
                    {
                        if (!string.IsNullOrEmpty(_selectorDefinition.SelectedValuePath))
                        {
                            var items = selectedItems.Select(x => ReflectionExtensions.TryGetFieldOrPropertyValue(x, _selectorDefinition.SelectedValuePath, out object objTargetValue)
                                ? objTargetValue
                                : null
                                )
                                .Where(x => x != null)
                                .ToList();

                            foreach (var item in items)
                            {
                                result.Add(item);
                            }

                            return result;
                        }
                        else
                        {
                            foreach (var item in selectedItems)
                            {
                                if (item == null)
                                {
                                    result.Add(null);
                                }
                                else if (targetType.GenericTypeArguments.Any() && targetType.GenericTypeArguments[0].IsAssignableFrom(item.GetType()))
                                {
                                    result.Add(item);
                                }
                            }
                        }
                    }
                    catch (ArgumentException)
                    {
                    }
                    catch (FormatException)
                    {
                    }

                    return result;
                }

                return DependencyProperty.UnsetValue;
            }
        }
    }
}
