using System;
using System.ComponentModel;
using System.Windows;

namespace PropertyEditorLibrary
{
    public class DefaultPropertyViewModelFactory : IPropertyViewModelFactory
    {
        protected readonly PropertyEditor owner;

        /// <summary>
        /// Gets or sets the IsEnabledPattern.
        /// 
        /// Example using a "Is{0}Enabled" pattern:
        ///   string City { get; set; }
        ///   bool IsCityEnabled { get; set; }
        /// The state of the City property will be controlled by the IsCityEnabled property
        /// </summary>
        /// <value>The IsEnabledPattern.</value>
        public string IsEnabledPattern { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPropertyViewModelFactory"/> class.
        /// </summary>
        /// <param name="owner">The owner PropertyEditor of the factory. 
        /// This is neccessary in order to get the PropertyTemplateSelector to work.</param>
        public DefaultPropertyViewModelFactory(PropertyEditor owner)
        {
            this.owner = owner;
            IsEnabledPattern = "Is{0}Enabled";
        }

        public virtual bool IsOptional(PropertyDescriptor descriptor, out string optionalPropertyName)
        {
            optionalPropertyName = null;

            // Optional by Nullable type
            if (descriptor.PropertyType.IsGenericType && descriptor.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;
            var oa = AttributeHelper.GetFirstAttribute<OptionalAttribute>(descriptor);
            if (oa != null)
            {
                optionalPropertyName = oa.PropertyName;
                return true;
            }
            return false;
        }

        public virtual bool IsWide(PropertyDescriptor descriptor, out bool showHeader)
        {
            showHeader = true;
            var wa = AttributeHelper.GetFirstAttribute<WidePropertyAttribute>(descriptor);
            if (wa != null)
            {
                showHeader = wa.ShowHeader;
                return true;
            }
            return false;
        }

        public virtual bool IsSlidable(PropertyDescriptor descriptor, out double min, out double max, out double largeChange, out double smallChange)
        {
            min = max = largeChange = smallChange = 0;
            var sa = AttributeHelper.GetFirstAttribute<SlidableAttribute>(descriptor);
            if (sa == null)
                return false;

            min = sa.Minimum;
            max = sa.Maximum;
            largeChange = sa.LargeChange;
            smallChange = sa.SmallChange;
            return true;
        }
        public virtual string GetFormatString(PropertyDescriptor descriptor)
        {
            var fsa = AttributeHelper.GetFirstAttribute<FormatStringAttribute>(descriptor);
            if (fsa == null)
                return null;
            return fsa.FormatString;
        }

        public virtual double GetHeight(PropertyDescriptor descriptor)
        {
            var ha = AttributeHelper.GetFirstAttribute<HeightAttribute>(descriptor);
            if (ha == null)
                return double.NaN;
                return ha.Height;
        }

        public virtual PropertyViewModel CreateViewModel(object instance, PropertyDescriptor descriptor)
        {
            PropertyViewModel propertyViewModel = null;

            string optionalPropertyName;
            if (IsOptional(descriptor, out optionalPropertyName))
                propertyViewModel = new OptionalPropertyViewModel(instance, descriptor, optionalPropertyName, owner);

            bool showHeader;
            if (IsWide(descriptor, out showHeader))
                propertyViewModel = new WidePropertyViewModel(instance, descriptor, showHeader, owner);

            // If bool properties should be shown as checkbox only (no header label), we create a CheckBoxPropertyViewModel
            if (descriptor.PropertyType == typeof(bool) && owner != null && !owner.ShowBoolHeader)
                propertyViewModel = new CheckBoxPropertyViewModel(instance, descriptor, owner);

            double min, max, largeChange, smallChange;
            if (IsSlidable(descriptor, out min, out max, out largeChange, out smallChange))
                propertyViewModel = new SlidablePropertyViewModel(instance, descriptor, owner)
                                        {
                                            SliderMinimum = min,
                                            SliderMaximum = max,
                                            SliderLargeChange = largeChange,
                                            SliderSmallChange = smallChange
                                        };

            // FilePath
            var fpa = AttributeHelper.GetFirstAttribute<FilePathAttribute>(descriptor);
            if (fpa != null)
                propertyViewModel = new FilePathPropertyViewModel(instance, descriptor, owner) { Filter = fpa.Filter, DefaultExtension = fpa.DefaultExtension };

            // DirectoryPath
            var dpa = AttributeHelper.GetFirstAttribute<DirectoryPathAttribute>(descriptor);
            if (dpa != null)
                propertyViewModel = new DirectoryPathPropertyViewModel(instance, descriptor, owner);

            // Default property (using textbox)
            if (propertyViewModel == null)
            {
                var tp = new PropertyViewModel(instance, descriptor, owner);
                propertyViewModel = tp;
            }

            propertyViewModel.FormatString = GetFormatString(descriptor);

            //var ha = AttributeHelper.GetFirstAttribute<HeightAttribute>(descriptor);
            //if (ha != null)
            //    propertyViewModel.Height = ha.Height;
            propertyViewModel.Height = GetHeight(descriptor);

            if (propertyViewModel.Height > 0)
            {
                propertyViewModel.AcceptsReturn = true;
                propertyViewModel.TextWrapping = TextWrapping.Wrap;
            }

            var soa = AttributeHelper.GetFirstAttribute<SortOrderAttribute>(descriptor);
            if (soa != null)
                propertyViewModel.SortOrder = soa.SortOrder;

            var pdc = TypeDescriptor.GetProperties(instance);
            var isEnabledName = String.Format(IsEnabledPattern, descriptor.Name);
            propertyViewModel.IsEnabledDescriptor = pdc.Find(isEnabledName, false);

            return propertyViewModel;
        }
    }


}