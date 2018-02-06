// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGridOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Cretes a model for the PropertyGrid control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Creates a model for the <see cref="PropertyGrid" /> control.
    /// </summary>
    public class PropertyGridOperator : IPropertyGridOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGridOperator" /> class.
        /// </summary>
        public PropertyGridOperator()
        {
            this.EnabledPattern = "Is{0}Enabled";
            this.VisiblePattern = "Is{0}Visible";
            this.OptionalPattern = "Use{0}";
            this.ModifyCamelCaseDisplayNames = true;
            this.InheritCategories = true;
        }

        /// <summary>
        /// Gets or sets the default name of the category.
        /// </summary>
        /// <value>The default name of the category.</value>
        public string DefaultCategoryName { get; set; }

        /// <summary>
        /// Gets or sets the default name of the tab.
        /// </summary>
        /// <value>The default name of the tab.</value>
        public string DefaultTabName { get; set; }

        /// <summary>
        /// Gets or sets the enabled pattern.
        /// </summary>
        /// <value>The enabled pattern.</value>
        public string EnabledPattern { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether each property should inherit the category attribute from the property declared before.
        /// </summary>
        public bool InheritCategories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to add spaces at the camel bumps of the display names.
        /// </summary>
        /// <value><c>true</c> if display names should be modified; otherwise, <c>false</c> .</value>
        public bool ModifyCamelCaseDisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the optional pattern.
        /// </summary>
        /// <value>The optional pattern.</value>
        public string OptionalPattern { get; set; }

        /// <summary>
        /// Gets or sets the visible pattern.
        /// </summary>
        /// <value>The visible pattern.</value>
        public string VisiblePattern { get; set; }

        /// <summary>
        /// Gets or sets the current category.
        /// </summary>
        /// <value>The current category.</value>
        protected string CurrentCategory { get; set; }

        /// <summary>
        /// Gets or sets the declaring type of the current category.
        /// </summary>
        /// <value>The type of the current category.</value>
        protected Type CurrentCategoryDeclaringType { get; set; }

        /// <summary>
        /// Gets or sets the type of the current component.
        /// </summary>
        /// <value>The type of the current component.</value>
        /// <remarks>This is used to avoid that Category attributes are inherited from superclass to subclass.</remarks>
        protected Type CurrentDeclaringType { get; set; }

        /// <summary>
        /// Creates the property model.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="isEnumerable">if set to <c>true</c> [is enumerable].</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// A list of <see cref="Tab" /> .
        /// </returns>
        public virtual IEnumerable<Tab> CreateModel(object instance, bool isEnumerable, IPropertyGridOptions options)
        {
            if (instance == null)
            {
                return null;
            }

            this.Reset();

            var tabs = new Dictionary<string, Tab>();
            foreach (var pi in this.CreatePropertyItems(instance, options).OrderBy(t => t.SortIndex))
            {
                var tabHeader = pi.Tab ?? string.Empty;
                if (!tabs.ContainsKey(tabHeader))
                {
                    tabs.Add(tabHeader, new Tab { Header = pi.Tab });
                }

                var tab = tabs[tabHeader];
                var category = pi.Category;
                var group = tab.Groups.FirstOrDefault(g => g.Header == category);
                if (group == null)
                {
                    group = new Group { Header = pi.Category };
                    tab.Groups.Add(group);
                }

                group.Properties.Add(pi);
            }

            return tabs.Values.ToList();
        }

        /// <summary>
        /// Creates a property item.
        /// </summary>
        /// <param name="pd">The property descriptor.</param>
        /// <param name="propertyDescriptors">The property descriptors.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// A property item.
        /// </returns>
        public virtual PropertyItem CreatePropertyItem(PropertyDescriptor pd, PropertyDescriptorCollection propertyDescriptors, object instance)
        {
            var pi = this.CreateCore(pd, propertyDescriptors);
            this.SetProperties(pi, instance);
            return pi;
        }

        /// <summary>
        /// Resets this factory.
        /// </summary>
        public void Reset()
        {
            this.CurrentCategory = null;
            this.CurrentDeclaringType = null;
            this.CurrentCategoryDeclaringType = null;
        }

        /// <summary>
        /// Creates property items for all properties in the specified object.
        /// </summary>
        /// <param name="instance">The object instance.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// Enumeration of PropertyItem.
        /// </returns>
        protected virtual IEnumerable<PropertyItem> CreatePropertyItems(object instance, IPropertyGridOptions options)
        {
            var instanceType = instance.GetType();

            // check if the MetadataTypeAttribute is set
            var metadataTypeAttribute = instanceType.GetCustomAttributes(typeof(MetadataTypeAttribute), true)
                                     .OfType<MetadataTypeAttribute>().FirstOrDefault();
            PropertyDescriptorCollection properties;
            if (metadataTypeAttribute != null)
            {
                // use the metadata type for reflection
                instanceType = metadataTypeAttribute.MetadataClassType;
                properties = TypeDescriptor.GetProperties(instanceType);
            }
            else
            {
                properties = TypeDescriptor.GetProperties(instance);
            }

            foreach (PropertyDescriptor pd in properties)
            {
                if (options.ShowDeclaredOnly && pd.ComponentType != instanceType)
                {
                    continue;
                }

                // Skip properties marked with [PropertyTools.DataAnnotations.Browsable(false)]
                var ba = pd.GetFirstAttributeOrDefault<DataAnnotations.BrowsableAttribute>();
                if (ba != null && !ba.Browsable)
                {
                    continue;
                }

                // Skip properties marked with [System.ComponentModel.Browsable(false)]
                if (!pd.IsBrowsable)
                {
                    continue;
                }

                // Read-only properties
                if (!options.ShowReadOnlyProperties && pd.IsReadOnly())
                {
                    continue;
                }

                // If RequiredAttribute is set, skip properties that don't have the given attribute
                if (options.RequiredAttribute != null && pd.GetFirstAttributeOrDefault(options.RequiredAttribute) == null)
                {
                    continue;
                }

                yield return this.CreatePropertyItem(pd, properties, instance);
            }
        }

        /// <summary>
        /// Creates the property item instance.
        /// </summary>
        /// <param name="pd">The property descriptor.</param>
        /// <param name="propertyDescriptors">The collection of property descriptors.</param>
        /// <returns>
        /// A property item.
        /// </returns>
        protected virtual PropertyItem CreateCore(PropertyDescriptor pd, PropertyDescriptorCollection propertyDescriptors)
        {
            return new PropertyItem(pd, propertyDescriptors);
        }

        /// <summary>
        /// Gets the category for the specified property.
        /// </summary>
        /// <param name="pd">The property descriptor.</param>
        /// <param name="declaringType">The declaring type.</param>
        /// <returns>
        /// A category string.
        /// </returns>
        protected virtual string GetCategory(PropertyDescriptor pd, Type declaringType)
        {
            return pd.GetCategory();
        }

        /// <summary>
        /// Gets the description for the specified property.
        /// </summary>
        /// <param name="pd">The property descriptor.</param>
        /// <param name="declaringType">The declaring type.</param>
        /// <returns>
        /// A description string.
        /// </returns>
        protected virtual string GetDescription(PropertyDescriptor pd, Type declaringType)
        {
            return pd.GetDescription();
        }

        /// <summary>
        /// Gets the display name for the specified property.
        /// </summary>
        /// <param name="pd">The property descriptor.</param>
        /// <param name="declaringType">The declaring type.</param>
        /// <returns>
        /// A display name string.
        /// </returns>
        protected virtual string GetDisplayName(PropertyDescriptor pd, Type declaringType)
        {
            var displayName = pd.GetDisplayName();

            if (this.ModifyCamelCaseDisplayNames && displayName == pd.Name)
            {
                displayName = StringUtilities.FromCamelCase(displayName);
            }

            return displayName;
        }

        /// <summary>
        /// Gets the localized description.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="declaringType">Type of the declaring.</param>
        /// <returns>
        /// The localized description.
        /// </returns>
        protected virtual string GetLocalizedDescription(string key, Type declaringType)
        {
            return key;
        }

        /// <summary>
        /// Gets the localized string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="declaringType">The declaring type.</param>
        /// <returns>
        /// The localized string.
        /// </returns>
        protected virtual string GetLocalizedString(string key, Type declaringType)
        {
            return key;
        }

        /// <summary>
        /// Sets the properties.
        /// </summary>
        /// <param name="pi">The property item.</param>
        /// <param name="instance">The instance.</param>
        protected virtual void SetProperties(PropertyItem pi, object instance)
        {
            var tabName = this.DefaultTabName ?? instance.GetType().Name;
            var categoryName = this.DefaultCategoryName;

            // find the declaring type
            var declaringType = pi.Descriptor.ComponentType;
            var propertyInfo = instance.GetType().GetProperty(pi.Descriptor.Name);
            if (propertyInfo != null)
            {
                declaringType = propertyInfo.DeclaringType;
            }

            if (declaringType != this.CurrentDeclaringType)
            {
                this.CurrentCategory = null;
            }

            this.CurrentDeclaringType = declaringType;

            if (!this.InheritCategories)
            {
                this.CurrentCategory = null;
            }

            var ca = pi.GetAttribute<System.ComponentModel.CategoryAttribute>();
            if (ca != null)
            {
                this.CurrentCategory = ca.Category;
                this.CurrentCategoryDeclaringType = declaringType;
            }

            var ca2 = pi.GetAttribute<DataAnnotations.CategoryAttribute>();
            if (ca2 != null)
            {
                this.CurrentCategory = ca2.Category;
                this.CurrentCategoryDeclaringType = declaringType;
            }

            var category = this.CurrentCategory ?? (this.DefaultCategoryName ?? this.GetCategory(pi.Descriptor, declaringType));

            if (category != null)
            {
                var items = category.Split('|');
                if (items.Length == 2)
                {
                    tabName = items[0];
                    categoryName = items[1];
                }

                if (items.Length == 1)
                {
                    categoryName = items[0];
                }
            }

            var displayName = this.GetDisplayName(pi.Descriptor, declaringType);
            var description = this.GetDescription(pi.Descriptor, declaringType);

            // Localize the strings
            pi.DisplayName = this.GetLocalizedString(displayName, declaringType);
            pi.Description = this.GetLocalizedDescription(description, declaringType);
            pi.Category = this.GetLocalizedString(categoryName, this.CurrentCategoryDeclaringType);
            pi.Tab = this.GetLocalizedString(tabName, this.CurrentCategoryDeclaringType);

            pi.IsReadOnly = pi.Descriptor.IsReadOnly();

            // Find descriptors by convention
            pi.IsEnabledDescriptor = pi.GetDescriptor(string.Format(this.EnabledPattern, pi.PropertyName));
            var isVisibleDescriptor = pi.GetDescriptor(string.Format(this.VisiblePattern, pi.PropertyName));
            if (isVisibleDescriptor != null)
            {
                pi.IsVisibleDescriptor = isVisibleDescriptor;
                pi.IsVisibleValue = true;
            }

            pi.OptionalDescriptor = pi.GetDescriptor(string.Format(this.OptionalPattern, pi.PropertyName));

            foreach (Attribute attribute in pi.Descriptor.Attributes)
            {
                this.SetAttribute(attribute, pi, instance);
            }

            pi.IsOptional = pi.IsOptional || pi.OptionalDescriptor != null;

            if (pi.Descriptor.PropertyType == typeof(TimeSpan) && pi.Converter == null)
            {
                pi.Converter = new TimeSpanToStringConverter();
                pi.ConverterParameter = pi.FormatString;
            }
        }

        /// <summary>
        /// Sets the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="pi">The pi.</param>
        /// <param name="instance">The instance.</param>
        protected virtual void SetAttribute(Attribute attribute, PropertyItem pi, object instance)
        {
            var ssa = attribute as SelectorStyleAttribute;
            if (ssa != null)
            {
                pi.SelectorStyle = ssa.SelectorStyle;
            }

            var svpa = attribute as SelectedValuePathAttribute;
            if (svpa != null)
            {
                pi.SelectedValuePath = svpa.Path;
            }

            var dmpa = attribute as DisplayMemberPathAttribute;
            if (dmpa != null)
            {
                pi.DisplayMemberPath = dmpa.Path;
            }

            var f = attribute as FontAttribute;
            if (f != null)
            {
                pi.FontSize = f.FontSize;
                pi.FontWeight = f.FontWeight;
                pi.FontFamily = f.FontFamily;
            }

            var fp = attribute as FontPreviewAttribute;
            if (fp != null)
            {
                pi.PreviewFonts = true;
                pi.FontSize = fp.Size;
                pi.FontWeight = fp.Weight;
                pi.FontFamilyPropertyDescriptor = pi.GetDescriptor(fp.FontFamilyPropertyName);
            }

            if (attribute is FontFamilySelectorAttribute)
            {
                pi.IsFontFamilySelector = true;
            }

            var ifpa = attribute as InputFilePathAttribute;
            if (ifpa != null)
            {
                pi.IsFilePath = true;
                pi.IsFileOpenDialog = true;
                pi.FilePathFilter = ifpa.Filter;
                pi.FilePathDefaultExtension = ifpa.DefaultExtension;
            }

            var ofpa = attribute as OutputFilePathAttribute;
            if (ofpa != null)
            {
                pi.IsFilePath = true;
                pi.IsFileOpenDialog = false;
                pi.FilePathFilter = ofpa.Filter;
                pi.FilePathDefaultExtension = ofpa.DefaultExtension;
            }

            if (attribute is DirectoryPathAttribute)
            {
                pi.IsDirectoryPath = true;
            }

            var da = attribute as DataTypeAttribute;
            if (da != null)
            {
                pi.DataTypes.Add(da.DataType);
                switch (da.DataType)
                {
                    case DataType.MultilineText:
                        pi.AcceptsReturn = true;
                        break;
                    case DataType.Password:
                        pi.IsPassword = true;
                        break;
                }
            }

            var cpa = attribute as ColumnsPropertyAttribute;
            if (cpa != null)
            {
                var descriptor = pi.GetDescriptor(cpa.PropertyName);
                var columns = descriptor?.GetValue(instance) as IEnumerable<Column>;

                if (columns != null)
                {
                    var glc = new GridLengthConverter();
                    foreach (Column column in columns)
                    {
                        var cd = new ColumnDefinition
                        {
                            PropertyName = column.PropertyName,
                            Header = column.Header,
                            FormatString = column.FormatString,
                            Width = (GridLength)(glc.ConvertFromInvariantString(column.Width) ?? GridLength.Auto),
                            IsReadOnly = column.IsReadOnly,
                            HorizontalAlignment = StringUtilities.ToHorizontalAlignment(column.Alignment.ToString(CultureInfo.InvariantCulture))
                        };

                        pi.Columns.Add(cd);
                    }
                }
            }

            var la = attribute as ListAttribute;
            if (la != null)
            {
                pi.ListCanAdd = la.CanAdd;
                pi.ListCanRemove = la.CanRemove;
                pi.ListMaximumNumberOfItems = la.MaximumNumberOfItems;
            }

            var ida = attribute as InputDirectionAttribute;
            if (ida != null)
            {
                pi.InputDirection = ida.InputDirection;
            }

            var eia = attribute as EasyInsertAttribute;
            if (eia != null)
            {
                pi.IsEasyInsertByKeyboardEnabled = eia.EasyInsertByKeyboard;
                pi.IsEasyInsertByMouseEnabled = eia.EasyInsertByMouse;
            }

            var sia = attribute as SortIndexAttribute;
            if (sia != null)
            {
                pi.SortIndex = sia.SortIndex;
            }

            var eba = attribute as EnableByAttribute;
            if (eba != null)
            {
                pi.IsEnabledDescriptor = pi.GetDescriptor(eba.PropertyName);
                pi.IsEnabledValue = eba.PropertyValue;
            }

            var vba = attribute as VisibleByAttribute;
            if (vba != null)
            {
                pi.IsVisibleDescriptor = pi.GetDescriptor(vba.PropertyName);
                pi.IsVisibleValue = vba.PropertyValue;
            }

            var oa = attribute as OptionalAttribute;
            if (oa != null)
            {
                pi.IsOptional = true;
                if (oa.PropertyName != null)
                {
                    pi.OptionalDescriptor = pi.GetDescriptor(oa.PropertyName);
                }
            }

            var ila = attribute as IndentationLevelAttribute;
            if (ila != null)
            {
                pi.IndentationLevel = ila.IndentationLevel;
            }

            var ra = attribute as EnableByRadioButtonAttribute;
            if (ra != null)
            {
                pi.RadioDescriptor = pi.GetDescriptor(ra.PropertyName);
                pi.RadioValue = ra.Value;
            }

            if (attribute is CommentAttribute)
            {
                pi.IsComment = true;
            }

            if (attribute is ContentAttribute)
            {
                pi.IsContent = true;
            }

            var ea = attribute as System.ComponentModel.DataAnnotations.EditableAttribute;
            if (ea != null)
            {
                pi.IsEditable = ea.AllowEdit;
            }

            var ea2 = attribute as DataAnnotations.EditableAttribute;
            if (ea2 != null)
            {
                pi.IsEditable = ea2.AllowEdit;
            }

            if (attribute is AutoUpdateTextAttribute)
            {
                pi.AutoUpdateText = true;
            }

            var ispa = attribute as ItemsSourcePropertyAttribute;
            if (ispa != null)
            {
                pi.ItemsSourceDescriptor = pi.GetDescriptor(ispa.PropertyName);
            }

            var liispa = attribute as ListItemItemsSourcePropertyAttribute;
            if (liispa != null)
            {
                var p = TypeDescriptor.GetProperties(instance)[liispa.PropertyName];
                var listItemItemsSource = p != null ? p.GetValue(instance) as IEnumerable : null;
                pi.ListItemItemsSource = listItemItemsSource;
            }

            var clpa = attribute as CheckableItemsAttribute;
            if (clpa != null)
            {
                pi.CheckableItemsIsCheckedPropertyName = clpa.IsCheckedPropertyName;
                pi.CheckableItemsContentPropertyName = clpa.ContentPropertyName;
            }

            var rpa = attribute as BasePathPropertyAttribute;
            if (rpa != null)
            {
                pi.RelativePathDescriptor = pi.GetDescriptor(rpa.BasePathPropertyName);
            }

            var fa = attribute as FilterPropertyAttribute;
            if (fa != null)
            {
                pi.FilterDescriptor = pi.GetDescriptor(fa.PropertyName);
            }

            var dea = attribute as DefaultExtensionPropertyAttribute;
            if (dea != null)
            {
                pi.DefaultExtensionDescriptor = pi.GetDescriptor(dea.PropertyName);
            }

            var fsa = attribute as FormatStringAttribute;
            if (fsa != null)
            {
                pi.FormatString = fsa.FormatString;
            }

            var coa = attribute as ConverterAttribute;
            if (coa != null)
            {
                pi.Converter = Activator.CreateInstance(coa.ConverterType) as IValueConverter;
            }

            var sa = attribute as SlidableAttribute;
            if (sa != null)
            {
                pi.IsSlidable = true;
                pi.SliderMinimum = sa.Minimum;
                pi.SliderMaximum = sa.Maximum;
                pi.SliderSmallChange = sa.SmallChange;
                pi.SliderLargeChange = sa.LargeChange;
                pi.SliderSnapToTicks = sa.SnapToTicks;
                pi.SliderTickFrequency = sa.TickFrequency;
            }

            var spa = attribute as SpinnableAttribute;
            if (spa != null)
            {
                pi.IsSpinnable = true;
                pi.SpinMinimum = spa.Minimum;
                pi.SpinMaximum = spa.Maximum;
                pi.SpinSmallChange = spa.SmallChange;
                pi.SpinLargeChange = spa.LargeChange;
            }

            var wpa = attribute as WidePropertyAttribute;
            if (wpa != null)
            {
                pi.HeaderPlacement = wpa.ShowHeader ? HeaderPlacement.Above : HeaderPlacement.Hidden;
            }

            var wia = attribute as WidthAttribute;
            if (wia != null)
            {
                pi.Width = wia.Width;
            }

            var hpa = attribute as HeaderPlacementAttribute;
            if (hpa != null)
            {
                pi.HeaderPlacement = hpa.HeaderPlacement;
            }

            var ha = attribute as HorizontalAlignmentAttribute;
            if (ha != null)
            {
                pi.HorizontalAlignment = ha.HorizontalAlignment;
            }

            var hea = attribute as HeightAttribute;
            if (hea != null)
            {
                pi.Height = hea.Height;
                pi.MinimumHeight = hea.MinimumHeight;
                pi.MaximumHeight = hea.MaximumHeight;
                pi.AcceptsReturn = true;
            }

            var fta = attribute as FillTabAttribute;
            if (fta != null)
            {
                pi.FillTab = true;
                pi.AcceptsReturn = true;
            }
        }
    }
}