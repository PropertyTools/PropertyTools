using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace PropertyTools.Wpf
{
    public class DefaultPropertyViewModelFactory : IPropertyViewModelFactory
    {
        protected readonly PropertyEditor owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPropertyViewModelFactory"/> class.
        /// </summary>
        /// <param name="owner">The owner PropertyEditor of the factory. 
        /// This is neccessary in order to get the PropertyTemplateSelector to work.</param>
        public DefaultPropertyViewModelFactory(PropertyEditor owner)
        {
            this.owner = owner;
            IsEnabledPattern = "Is{0}Enabled";
            IsVisiblePattern = "Is{0}Visible";
            UsePropertyPattern = "Use{0}";
        }

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
        /// Gets or sets the IsVisiblePattern.
        /// 
        /// Example using a "Is{0}Visible" pattern:
        ///   string City { get; set; }
        ///   bool IsCityVisible { get; set; }
        /// The visibility state of the City property will be controlled by the IsCityVisible property
        /// </summary>
        /// <value>The IsVisiblePattern.</value>
        public string IsVisiblePattern { get; set; }

        /// <summary>
        /// Gets or sets the UsePattern. This is used to create an "Optional" property.
        /// 
        /// Example using a "Use{0}" pattern:
        ///   string City { get; set; }
        ///   bool UseCity { get; set; }
        /// The optional state of the City property will be controlled by the UseCity property
        /// </summary>
        /// <value>The UsePropertyPattern.</value>
        public string UsePropertyPattern { get; set; }

        #region IPropertyViewModelFactory Members

        public virtual PropertyViewModel CreateViewModel(object instance, PropertyDescriptor descriptor)
        {
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(instance);

            PropertyViewModel propertyViewModel = null;

            string optionalPropertyName;
            if (IsOptional(descriptor, out optionalPropertyName))
                propertyViewModel = new OptionalPropertyViewModel(instance, descriptor, optionalPropertyName, owner);

            if (IsPassword(descriptor))
                propertyViewModel = new PasswordPropertyViewModel(instance, descriptor, owner);

            if (IsResettable(descriptor))
                propertyViewModel = new ResettablePropertyViewModel(instance, descriptor, owner);

            if (UsePropertyPattern != null)
            {
                string usePropertyName = String.Format(UsePropertyPattern, descriptor.Name);
                PropertyDescriptor useDescriptor = pdc.Find(usePropertyName, false);
                if (useDescriptor != null)
                    propertyViewModel = new OptionalPropertyViewModel(instance, descriptor, useDescriptor, owner);
            }

            bool showHeader;
            if (IsWide(descriptor, out showHeader))
                propertyViewModel = new WidePropertyViewModel(instance, descriptor, showHeader, owner);

            // If bool properties should be shown as checkbox only (no header label), we create a CheckBoxPropertyViewModel
            if (descriptor.PropertyType == typeof(bool) && owner != null && !owner.ShowBoolHeader)
                propertyViewModel = new CheckBoxPropertyViewModel(instance, descriptor, owner);

            double min, max, largeChange, smallChange;
            double tickFrequency;
            bool snapToTicks;
            TickPlacement tickPlacement;
            if (IsSlidable(descriptor, out min, out max, out largeChange, out smallChange, out tickFrequency,
                           out snapToTicks, out tickPlacement))
                propertyViewModel = new SlidablePropertyViewModel(instance, descriptor, owner)
                                        {
                                            SliderMinimum = min,
                                            SliderMaximum = max,
                                            SliderLargeChange = largeChange,
                                            SliderSmallChange = smallChange,
                                            SliderSnapToTicks = snapToTicks,
                                            SliderTickFrequency = tickFrequency,
                                            SliderTickPlacement = tickPlacement
                                        };

            // FilePath
            string filter, defaultExtension;
            bool useOpenDialog;
            if (IsFilePath(descriptor, out filter, out defaultExtension, out useOpenDialog))
                propertyViewModel = new FilePathPropertyViewModel(instance, descriptor, owner) { Filter = filter, DefaultExtension = defaultExtension, UseOpenDialog = useOpenDialog };

            // DirectoryPath
            if (IsDirectoryPath(descriptor))
                propertyViewModel = new DirectoryPathPropertyViewModel(instance, descriptor, owner);

            // Default property (using textbox)
            if (propertyViewModel == null)
            {
                var tp = new PropertyViewModel(instance, descriptor, owner);
                propertyViewModel = tp;
            }

            // Check if the AutoUpdatingText attribute is set (this will select the template that has a text binding using UpdateSourceTrigger=PropertyChanged)
            if (IsAutoUpdatingText(descriptor))
                propertyViewModel.AutoUpdateText = true;

            propertyViewModel.FormatString = GetFormatString(descriptor);

            //var ha = AttributeHelper.GetFirstAttribute<HeightAttribute>(descriptor);
            //if (ha != null)
            //    propertyViewModel.Height = ha.Height;
            propertyViewModel.Height = GetHeight(descriptor);
            propertyViewModel.MaxLength = GetMaxLength(descriptor);

            if (propertyViewModel.Height > 0 || IsMultilineText(descriptor))
            {
                propertyViewModel.AcceptsReturn = true;
                propertyViewModel.TextWrapping = TextWrapping.Wrap;
            }

            var soa = AttributeHelper.GetFirstAttribute<SortOrderAttribute>(descriptor);
            if (soa != null)
                propertyViewModel.SortOrder = soa.SortOrder;

            if (IsEnabledPattern != null)
            {
                string isEnabledName = String.Format(IsEnabledPattern, descriptor.Name);
                propertyViewModel.IsEnabledDescriptor = pdc.Find(isEnabledName, false);
            }

            if (IsVisiblePattern != null)
            {
                string isVisibleName = String.Format(IsVisiblePattern, descriptor.Name);
                propertyViewModel.IsVisibleDescriptor = pdc.Find(isVisibleName, false);
            }

            return propertyViewModel;
        }

        protected virtual bool IsMultilineText(PropertyDescriptor descriptor)
        {
            var dta = AttributeHelper.GetFirstAttribute<DataTypeAttribute>(descriptor);
            return dta != null && dta.DataType==DataType.MultilineText;
        }

        #endregion

        protected virtual bool IsDirectoryPath(PropertyDescriptor descriptor)
        {
            var dpa = AttributeHelper.GetFirstAttribute<DirectoryPathAttribute>(descriptor);
            return dpa != null;
        }

        protected virtual bool IsFilePath(PropertyDescriptor descriptor, out string filter, out string defaultExtension, out bool useOpenDialog)
        {
            var fpa = AttributeHelper.GetFirstAttribute<FilePathAttribute>(descriptor);
            filter = fpa != null ? fpa.Filter : null;
            defaultExtension = fpa != null ? fpa.DefaultExtension : null;
            useOpenDialog = fpa != null ? fpa.UseOpenDialog : true;
            return fpa != null;
        }

        protected virtual bool IsOptional(PropertyDescriptor descriptor, out string optionalPropertyName)
        {
            optionalPropertyName = null;

            // The following code is disabled (will make all Nullable types optional)
            //  if (descriptor.PropertyType.IsGenericType && descriptor.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            //      return true;

            // Return true only if the [Optional] attribute is set.
            var oa = AttributeHelper.GetFirstAttribute<OptionalAttribute>(descriptor);
            if (oa != null)
            {
                optionalPropertyName = oa.PropertyName;
                return true;
            }
            return false;
        }

        protected virtual bool IsPassword(PropertyDescriptor descriptor)
        {
            var pa = AttributeHelper.GetFirstAttribute<DataTypeAttribute>(descriptor);
            return (pa != null && pa.DataType == DataType.Password);
        }

        public virtual bool IsResettable(PropertyDescriptor descriptor)
        {
            var oa = AttributeHelper.GetFirstAttribute<ResettableAttribute>(descriptor);
            if (oa != null)
                return true;
            return false;
        }

        protected virtual bool IsWide(PropertyDescriptor descriptor, out bool showHeader)
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

        protected virtual bool IsSlidable(PropertyDescriptor descriptor, out double min, out double max,
                                          out double largeChange, out double smallChange, out double tickFrequency,
                                          out bool snapToTicks, out TickPlacement tickPlacement)
        {
            min = max = largeChange = smallChange = 0;
            tickFrequency = 1;
            snapToTicks = false;
            tickPlacement = TickPlacement.None;

            bool result = false;

            var sa = AttributeHelper.GetFirstAttribute<SlidableAttribute>(descriptor);
            if (sa != null)
            {
                min = sa.Minimum;
                max = sa.Maximum;
                largeChange = sa.LargeChange;
                smallChange = sa.SmallChange;
                snapToTicks = sa.SnapToTicks;
                tickFrequency = sa.TickFrequency;
                tickPlacement = sa.TickPlacement;
                result = true;
            }
            return result;
        }

        protected virtual string GetFormatString(PropertyDescriptor descriptor)
        {
            var fsa = AttributeHelper.GetFirstAttribute<FormatStringAttribute>(descriptor);
            if (fsa == null)
                return null;
            return fsa.FormatString;
        }

        protected virtual double GetHeight(PropertyDescriptor descriptor)
        {
            var ha = AttributeHelper.GetFirstAttribute<HeightAttribute>(descriptor);
            if (ha == null)
                return double.NaN;
            return ha.Height;
        }

        protected virtual int GetMaxLength(PropertyDescriptor descriptor)
        {
            var ha = AttributeHelper.GetFirstAttribute<StringLengthAttribute>(descriptor);
            if (ha == null)
                return int.MaxValue;
            return ha.MaximumLength;
        }

        protected virtual bool IsAutoUpdatingText(PropertyDescriptor descriptor)
        {
            var a = AttributeHelper.GetFirstAttribute<AutoUpdateTextAttribute>(descriptor);
            return a != null;
        }
    }
}