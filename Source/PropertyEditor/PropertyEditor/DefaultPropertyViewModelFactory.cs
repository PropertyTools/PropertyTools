using System;
using System.ComponentModel;
using System.Windows;

namespace PropertyEditorLibrary
{
    public class DefaultPropertyViewModelFactory : IPropertyViewModelFactory
    {
        private readonly PropertyEditor owner;

        /// <summary>
        /// Gets or sets the IsEnabledPattern.
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
            var oa = AttributeHelper.GetAttribute<OptionalAttribute>(descriptor);
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
            var wa = AttributeHelper.GetAttribute<WidePropertyAttribute>(descriptor);
            if (wa != null)
            {
                showHeader = wa.ShowHeader;
                return true;
            }
            return false;
        }

        public virtual bool IsSlidable(PropertyDescriptor descriptor, out double min)
        {
            min = 0;
            return false;
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

            // If bool properties should be shown as checkbox only (no header label), we create 
            // a CheckBoxPropertyViewModel
            if (descriptor.PropertyType == typeof(bool) && owner != null && !owner.ShowBoolHeader)
                propertyViewModel = new CheckBoxPropertyViewModel(instance, descriptor, owner);

            // Properties with the Slidable attribute set
            var sa = AttributeHelper.GetAttribute<SlidableAttribute>(descriptor);
            if (sa != null)
                propertyViewModel = new SlidablePropertyViewModel(instance, descriptor, owner)
                                        {
                                            SliderMinimum = sa.Minimum,
                                            SliderMaximum = sa.Maximum,
                                            SliderLargeChange = sa.LargeChange,
                                            SliderSmallChange = sa.SmallChange
                                        };

            // FilePath
            var fpa = AttributeHelper.GetAttribute<FilePathAttribute>(descriptor);
            if (fpa != null)
                propertyViewModel = new FilePathPropertyViewModel(instance, descriptor, owner) { Filter = fpa.Filter, DefaultExtension = fpa.DefaultExtension };

            // DirectoryPath
            var dpa = AttributeHelper.GetAttribute<DirectoryPathAttribute>(descriptor);
            if (dpa != null)
                propertyViewModel = new DirectoryPathPropertyViewModel(instance, descriptor, owner);

            // Default text property
            if (propertyViewModel == null)
            {
                var tp = new PropertyViewModel(instance, descriptor, owner);

                propertyViewModel = tp;
            }

            var fsa = AttributeHelper.GetAttribute<FormatStringAttribute>(descriptor);
            if (fsa != null)
                propertyViewModel.FormatString = fsa.FormatString;

            var ha = AttributeHelper.GetAttribute<HeightAttribute>(descriptor);
            if (ha != null)
                propertyViewModel.Height = ha.Height;
            if (propertyViewModel.Height > 0)
            {
                propertyViewModel.AcceptsReturn = true;
                propertyViewModel.TextWrapping = TextWrapping.Wrap;
            }

            var soa = AttributeHelper.GetAttribute<SortOrderAttribute>(descriptor);
            if (soa != null)
                propertyViewModel.SortOrder = soa.SortOrder;

            var pdc = TypeDescriptor.GetProperties(instance);
            var isEnabledName = String.Format(IsEnabledPattern, descriptor.Name);
            propertyViewModel.IsEnabledDescriptor = pdc.Find(isEnabledName, false);

            return propertyViewModel;
        }
    }


}