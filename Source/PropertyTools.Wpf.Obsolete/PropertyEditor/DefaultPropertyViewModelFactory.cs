// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultPropertyViewModelFactory.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Windows;
    using System.Windows.Controls.Primitives;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// The default property view model factory.
    /// </summary>
    public class DefaultPropertyViewModelFactory : IPropertyViewModelFactory
    {
        #region Constants and Fields

        /// <summary>
        ///   The owner.
        /// </summary>
        protected readonly PropertyEditor owner;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPropertyViewModelFactory"/> class.
        /// </summary>
        /// <param name="owner">
        /// The owner PropertyEditor of the factory. 
        ///   This is neccessary in order to get the PropertyTemplateSelector to work.
        /// </param>
        public DefaultPropertyViewModelFactory(PropertyEditor owner)
        {
            this.owner = owner;
            this.IsEnabledPattern = "Is{0}Enabled";
            this.IsVisiblePattern = "Is{0}Visible";
            this.UsePropertyPattern = "Use{0}";
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the IsEnabledPattern.
        /// 
        ///   Example using a "Is{0}Enabled" pattern:
        ///   string City { get; set; }
        ///   bool IsCityEnabled { get; set; }
        ///   The state of the City property will be controlled by the IsCityEnabled property
        /// </summary>
        /// <value>The IsEnabledPattern.</value>
        public string IsEnabledPattern { get; set; }

        /// <summary>
        ///   Gets or sets the IsVisiblePattern.
        /// 
        ///   Example using a "Is{0}Visible" pattern:
        ///   string City { get; set; }
        ///   bool IsCityVisible { get; set; }
        ///   The visibility state of the City property will be controlled by the IsCityVisible property
        /// </summary>
        /// <value>The IsVisiblePattern.</value>
        public string IsVisiblePattern { get; set; }

        /// <summary>
        ///   Gets or sets the UsePattern. This is used to create an "Optional" property.
        /// 
        ///   Example using a "Use{0}" pattern:
        ///   string City { get; set; }
        ///   bool UseCity { get; set; }
        ///   The optional state of the City property will be controlled by the UseCity property
        /// </summary>
        /// <value>The UsePropertyPattern.</value>
        public string UsePropertyPattern { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The create view model.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// </returns>
        public virtual PropertyViewModel CreateViewModel(object instance, PropertyDescriptor descriptor)
        {
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(instance);

            PropertyViewModel propertyViewModel = null;

            string optionalPropertyName;
            if (this.IsOptional(descriptor, out optionalPropertyName))
            {
                propertyViewModel = new OptionalPropertyViewModel(
                    instance, descriptor, optionalPropertyName, this.owner);
            }

            if (this.IsPassword(descriptor))
            {
                propertyViewModel = new PasswordPropertyViewModel(instance, descriptor, this.owner);
            }

            if (this.IsResettable(descriptor))
            {
                propertyViewModel = new ResettablePropertyViewModel(instance, descriptor, this.owner);
            }

            if (this.UsePropertyPattern != null)
            {
                string usePropertyName = string.Format(this.UsePropertyPattern, descriptor.Name);
                PropertyDescriptor useDescriptor = pdc.Find(usePropertyName, false);
                if (useDescriptor != null)
                {
                    propertyViewModel = new OptionalPropertyViewModel(instance, descriptor, useDescriptor, this.owner);
                }
            }

            bool showHeader;
            if (this.IsWide(descriptor, out showHeader))
            {
                propertyViewModel = new WidePropertyViewModel(instance, descriptor, showHeader, this.owner);
            }

            // If bool properties should be shown as checkbox only (no header label), we create a CheckBoxPropertyViewModel
            if (descriptor.PropertyType == typeof(bool) && this.owner != null && !this.owner.ShowBoolHeader)
            {
                propertyViewModel = new CheckBoxPropertyViewModel(instance, descriptor, this.owner);
            }

            double min, max, largeChange, smallChange;
            double tickFrequency;
            bool snapToTicks;
            TickPlacement tickPlacement;
            if (this.IsSlidable(
                descriptor, 
                out min, 
                out max, 
                out largeChange, 
                out smallChange, 
                out tickFrequency, 
                out snapToTicks, 
                out tickPlacement))
            {
                propertyViewModel = new SlidablePropertyViewModel(instance, descriptor, this.owner)
                    {
                        SliderMinimum = min, 
                        SliderMaximum = max, 
                        SliderLargeChange = largeChange, 
                        SliderSmallChange = smallChange, 
                        SliderSnapToTicks = snapToTicks, 
                        SliderTickFrequency = tickFrequency, 
                        SliderTickPlacement = tickPlacement
                    };
            }

            // FilePath
            string filter, defaultExtension;
            bool useOpenDialog;
            if (this.IsFilePath(descriptor, out filter, out defaultExtension, out useOpenDialog))
            {
                propertyViewModel = new FilePathPropertyViewModel(instance, descriptor, this.owner)
                    {
                       Filter = filter, DefaultExtension = defaultExtension, UseOpenDialog = useOpenDialog 
                    };
            }

            // DirectoryPath
            if (this.IsDirectoryPath(descriptor))
            {
                propertyViewModel = new DirectoryPathPropertyViewModel(instance, descriptor, this.owner);
            }

            if (this.IsEnum(descriptor))
            {
                propertyViewModel = new EnumPropertyViewModel(instance, descriptor, this.owner);
            }

            // Default property (using textbox)
            if (propertyViewModel == null)
            {
                var tp = new PropertyViewModel(instance, descriptor, this.owner);
                propertyViewModel = tp;
            }

            // Check if the AutoUpdatingText attribute is set (this will select the template that has a text binding using UpdateSourceTrigger=PropertyChanged)
            if (this.IsAutoUpdatingText(descriptor))
            {
                propertyViewModel.AutoUpdateText = true;
            }

            propertyViewModel.FormatString = this.GetFormatString(descriptor);

            // var ha = AttributeHelper.GetFirstAttribute<HeightAttribute>(descriptor);
            // if (ha != null)
            // propertyViewModel.Height = ha.Height;
            propertyViewModel.Height = this.GetHeight(descriptor);
            propertyViewModel.MaxLength = this.GetMaxLength(descriptor);

            if (propertyViewModel.Height > 0 || this.IsMultilineText(descriptor))
            {
                propertyViewModel.AcceptsReturn = true;
                propertyViewModel.TextWrapping = TextWrapping.Wrap;
            }

            var soa = AttributeHelper.GetFirstAttribute<SortIndexAttribute>(descriptor);
            if (soa != null)
            {
                propertyViewModel.SortIndex = soa.SortIndex;
            }

            if (this.IsEnabledPattern != null)
            {
                string isEnabledName = string.Format(this.IsEnabledPattern, descriptor.Name);
                propertyViewModel.IsEnabledDescriptor = pdc.Find(isEnabledName, false);
            }

            if (this.IsVisiblePattern != null)
            {
                string isVisibleName = string.Format(this.IsVisiblePattern, descriptor.Name);
                propertyViewModel.IsVisibleDescriptor = pdc.Find(isVisibleName, false);
            }

            return propertyViewModel;
        }

        /// <summary>
        /// The is resettable.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The is resettable.
        /// </returns>
        public virtual bool IsResettable(PropertyDescriptor descriptor)
        {
            var oa = AttributeHelper.GetFirstAttribute<ResettableAttribute>(descriptor);
            if (oa != null)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get format string.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The get format string.
        /// </returns>
        protected virtual string GetFormatString(PropertyDescriptor descriptor)
        {
            var fsa = AttributeHelper.GetFirstAttribute<FormatStringAttribute>(descriptor);
            if (fsa == null)
            {
                return null;
            }

            return fsa.FormatString;
        }

        /// <summary>
        /// The get height.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The get height.
        /// </returns>
        protected virtual double GetHeight(PropertyDescriptor descriptor)
        {
            var ha = AttributeHelper.GetFirstAttribute<HeightAttribute>(descriptor);
            if (ha == null)
            {
                return double.NaN;
            }

            return ha.Height;
        }

        /// <summary>
        /// The get max length.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The get max length.
        /// </returns>
        protected virtual int GetMaxLength(PropertyDescriptor descriptor)
        {
            var ha = AttributeHelper.GetFirstAttribute<StringLengthAttribute>(descriptor);
            if (ha == null)
            {
                return int.MaxValue;
            }

            return ha.MaximumLength;
        }

        /// <summary>
        /// The is auto updating text.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The is auto updating text.
        /// </returns>
        protected virtual bool IsAutoUpdatingText(PropertyDescriptor descriptor)
        {
            var a = AttributeHelper.GetFirstAttribute<AutoUpdateTextAttribute>(descriptor);
            return a != null;
        }

        /// <summary>
        /// The is directory path.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The is directory path.
        /// </returns>
        protected virtual bool IsDirectoryPath(PropertyDescriptor descriptor)
        {
            var dpa = AttributeHelper.GetFirstAttribute<DirectoryPathAttribute>(descriptor);
            return dpa != null;
        }

        /// <summary>
        /// The is enum.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The is enum.
        /// </returns>
        protected virtual bool IsEnum(PropertyDescriptor descriptor)
        {
            return descriptor.PropertyType.IsEnum;
        }

        /// <summary>
        /// The is file path.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="defaultExtension">
        /// The default extension.
        /// </param>
        /// <param name="useOpenDialog">
        /// The use open dialog.
        /// </param>
        /// <returns>
        /// The is file path.
        /// </returns>
        protected virtual bool IsFilePath(
            PropertyDescriptor descriptor, out string filter, out string defaultExtension, out bool useOpenDialog)
        {
            var fpa = AttributeHelper.GetFirstAttribute<FilePathAttribute>(descriptor);
            filter = fpa != null ? fpa.Filter : null;
            defaultExtension = fpa != null ? fpa.DefaultExtension : null;
            useOpenDialog = fpa != null ? fpa.UseOpenDialog : true;
            return fpa != null;
        }

        /// <summary>
        /// The is multiline text.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The is multiline text.
        /// </returns>
        protected virtual bool IsMultilineText(PropertyDescriptor descriptor)
        {
            var dta = AttributeHelper.GetFirstAttribute<DataTypeAttribute>(descriptor);
            return dta != null && dta.DataType == DataType.MultilineText;
        }

        /// <summary>
        /// The is optional.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="optionalPropertyName">
        /// The optional property name.
        /// </param>
        /// <returns>
        /// The is optional.
        /// </returns>
        protected virtual bool IsOptional(PropertyDescriptor descriptor, out string optionalPropertyName)
        {
            optionalPropertyName = null;

            // The following code is disabled (will make all Nullable types optional)
            // if (descriptor.PropertyType.IsGenericType && descriptor.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            // return true;

            // Return true only if the [Optional] attribute is set.
            var oa = AttributeHelper.GetFirstAttribute<OptionalAttribute>(descriptor);
            if (oa != null)
            {
                optionalPropertyName = oa.PropertyName;
                return true;
            }

            return false;
        }

        /// <summary>
        /// The is password.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The is password.
        /// </returns>
        protected virtual bool IsPassword(PropertyDescriptor descriptor)
        {
            var pa = AttributeHelper.GetFirstAttribute<DataTypeAttribute>(descriptor);
            return pa != null && pa.DataType == DataType.Password;
        }

        /// <summary>
        /// The is slidable.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="min">
        /// The min.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <param name="largeChange">
        /// The large change.
        /// </param>
        /// <param name="smallChange">
        /// The small change.
        /// </param>
        /// <param name="tickFrequency">
        /// The tick frequency.
        /// </param>
        /// <param name="snapToTicks">
        /// The snap to ticks.
        /// </param>
        /// <param name="tickPlacement">
        /// The tick placement.
        /// </param>
        /// <returns>
        /// The is slidable.
        /// </returns>
        protected virtual bool IsSlidable(
            PropertyDescriptor descriptor, 
            out double min, 
            out double max, 
            out double largeChange, 
            out double smallChange, 
            out double tickFrequency, 
            out bool snapToTicks, 
            out TickPlacement tickPlacement)
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
                result = true;
            }

            return result;
        }

        /// <summary>
        /// The is wide.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="showHeader">
        /// The show header.
        /// </param>
        /// <returns>
        /// The is wide.
        /// </returns>
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

        #endregion
    }
}