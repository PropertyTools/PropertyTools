using System;
using System.ComponentModel;
using System.Windows;

namespace PropertyEditorLibrary
{
    public class DefaultPropertyViewModelFactory : IPropertyViewModelFactory
    {
        private readonly PropertyEditor owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPropertyViewModelFactory"/> class.
        /// </summary>
        /// <param name="owner">The owner PropertyEditor of the factory. 
        /// This is neccessary in order to get the PropertyTemplateSelector to work.</param>
        public DefaultPropertyViewModelFactory(PropertyEditor owner)
        {
            this.owner = owner;
        }

        public virtual PropertyViewModel CreateViewModel(object instance, PropertyDescriptor descriptor)
        {
            PropertyViewModel propertyViewModel = null;

            // Optional by Nullable type
            var nullable = descriptor.PropertyType.IsGenericType &&
                           descriptor.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (nullable)
                propertyViewModel = new OptionalPropertyViewModel(instance, descriptor, null, owner);

            // Optional by Attribute
            var oa = AttributeHelper.GetAttribute<OptionalAttribute>(descriptor);
            if (oa != null)
                propertyViewModel = new OptionalPropertyViewModel(instance, descriptor, oa.PropertyName, owner);

            // Wide
            var wa = AttributeHelper.GetAttribute<WidePropertyAttribute>(descriptor);
            if (wa != null)
                propertyViewModel = new WidePropertyViewModel(instance, descriptor, wa.ShowHeader, owner);

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
            if (propertyViewModel.Height>0)
            {
                propertyViewModel.AcceptsReturn = true;
                propertyViewModel.TextWrapping = TextWrapping.Wrap;
            }

            var soa = AttributeHelper.GetAttribute<SortOrderAttribute>(descriptor);
            if (soa != null)
                propertyViewModel.SortOrder = soa.SortOrder;

            return propertyViewModel;
        }
    }


}