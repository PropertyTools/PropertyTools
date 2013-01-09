// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultPropertyItemFactory.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Provides a default property item factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Windows.Data;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Provides a default property item factory.
    /// </summary>
    public class DefaultPropertyItemFactory : IPropertyItemFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPropertyItemFactory" /> class.
        /// </summary>
        public DefaultPropertyItemFactory()
        {
            this.EnabledPattern = "Is{0}Enabled";
            this.VisiblePattern = "Is{0}Visible";
            this.OptionalPattern = "Use{0}";
            this.NicifyDisplayNames = true;
            this.InheritCategories = true;
        }

        /// <summary>
        /// Gets or sets the default name of the category.
        /// </summary>
        /// <value> The default name of the category. </value>
        public string DefaultCategoryName { get; set; }

        /// <summary>
        /// Gets or sets the default name of the tab.
        /// </summary>
        /// <value> The default name of the tab. </value>
        public string DefaultTabName { get; set; }

        /// <summary>
        /// Gets or sets the enabled pattern.
        /// </summary>
        /// <value> The enabled pattern. </value>
        public string EnabledPattern { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether each property should inherit the category attribute from the property declared before.
        /// </summary>
        public bool InheritCategories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to 'nicify' display names.
        /// </summary>
        /// <remarks>
        /// The 'nicifiying' adds spaces at the camel bumps.
        /// </remarks>
        /// <value> <c>true</c> if display names should be nicified; otherwise, <c>false</c> . </value>
        public bool NicifyDisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the optional pattern.
        /// </summary>
        /// <value> The optional pattern. </value>
        public string OptionalPattern { get; set; }

        /// <summary>
        /// Gets or sets the visible pattern.
        /// </summary>
        /// <value> The visible pattern. </value>
        public string VisiblePattern { get; set; }

        /// <summary>
        /// Gets or sets the current category.
        /// </summary>
        /// <value> The current category. </value>
        protected string CurrentCategory { get; set; }

        /// <summary>
        /// Gets or sets the declaring type of the current category.
        /// </summary>
        /// <value> The type of the current category. </value>
        protected Type CurrentCategoryDeclaringType { get; set; }

        /// <summary>
        /// Gets or sets the type of the current component.
        /// </summary>
        /// <remarks>
        /// This is used to avoid that Category attributes are inherited from superclass to subclass.
        /// </remarks>
        /// <value> The type of the current component. </value>
        protected Type CurrentDeclaringType { get; set; }

        /// <summary>
        /// Creates the property model.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="isEnumerable">
        /// if set to <c>true</c> [is enumerable].
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// A list of <see cref="Tab"/> .
        /// </returns>
        public virtual IEnumerable<Tab> CreateModel(object instance, bool isEnumerable, IPropertyControlOptions options)
        {
            if (instance == null)
            {
                return null;
            }

            var instanceType = instance.GetType();
            var properties = TypeDescriptor.GetProperties(instance);

            this.Reset();

            var items = new List<PropertyItem>();
            var tabs = new Dictionary<string, Tab>();
            foreach (PropertyDescriptor pd in properties)
            {
                if (options.ShowDeclaredOnly && pd.ComponentType != instanceType)
                {
                    continue;
                }

                // Skip properties marked with [Browsable(false)]
                if (!pd.IsBrowsable)
                {
                    continue;
                }

                // Read-only properties
                if (!options.ShowReadOnlyProperties && pd.IsReadOnly)
                {
                    continue;
                }

                // If RequiredAttribute is set, skip properties that don't have the given attribute
                if (options.RequiredAttribute != null
                    && !AttributeHelper.ContainsAttributeOfType(pd.Attributes, options.RequiredAttribute))
                {
                    continue;
                }

                var pi = this.CreatePropertyItem(pd, properties, instance);
                items.Add(pi);
            }

            foreach (var pi in items.OrderBy(t => t.SortIndex))
            {
                var tabHeader = pi.Tab ?? string.Empty;
                if (!tabs.ContainsKey(tabHeader))
                {
                    tabs.Add(tabHeader, new Tab { Header = pi.Tab });
                }

                var tab = tabs[tabHeader];
                var group = tab.Groups.FirstOrDefault(g => g.Header == pi.Category);
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
        /// <param name="pd">
        /// The property descriptor.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
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
        /// Variables the display name of the name to.
        /// </summary>
        /// <param name="variableName">
        /// Name of the variable.
        /// </param>
        /// <returns>
        /// The nicify string.
        /// </returns>
        protected static string NicifyString(string variableName)
        {
            var sb = new StringBuilder();
            Func<char, bool> isUpper = ch => ch.ToString() == ch.ToString().ToUpper();
            for (int i = 0; i < variableName.Length; i++)
            {
                if (i > 0 && isUpper(variableName[i]) && !isUpper(variableName[i - 1]))
                {
                    sb.Append(" ");
                    if (i == variableName.Length - 1 || isUpper(variableName[i + 1]))
                    {
                        sb.Append(variableName[i]);
                    }
                    else
                    {
                        sb.Append(variableName[i].ToString().ToLower());
                    }

                    continue;
                }

                sb.Append(variableName[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates the property item instance.
        /// </summary>
        /// <param name="pd">The pd.</param>
        /// <param name="propertyDescriptors">The property descriptors.</param>
        /// <returns>
        /// The core.
        /// </returns>
        protected virtual PropertyItem CreateCore(PropertyDescriptor pd, PropertyDescriptorCollection propertyDescriptors)
        {
            return new PropertyItem(pd, propertyDescriptors);
        }

        /// <summary>
        /// Gets the tool tip for the specified property.
        /// </summary>
        /// <param name="pd">
        /// The property descriptor.
        /// </param>
        /// <param name="declaringType">
        /// The declaring type.
        /// </param>
        /// <returns>
        /// A tool tip.
        /// </returns>
        protected virtual string GetDescription(PropertyDescriptor pd, Type declaringType)
        {
            return pd.Description;
        }

        /// <summary>
        /// Gets the display name for the specified property.
        /// </summary>
        /// <param name="pd">
        /// The property descriptor.
        /// </param>
        /// <param name="declaringType">
        /// The declaring type.
        /// </param>
        /// <returns>
        /// A display name string.
        /// </returns>
        protected virtual string GetDisplayName(PropertyDescriptor pd, Type declaringType)
        {
            var displayName = pd.DisplayName;
            if (this.NicifyDisplayNames && pd.DisplayName == pd.Name)
            {
                displayName = NicifyString(displayName);
            }

            return displayName;
        }

        /// <summary>
        /// Gets the localized description.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="declaringType">
        /// Type of the declaring.
        /// </param>
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
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="declaringType">
        /// The declaring type.
        /// </param>
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
            var pd = pi.Descriptor;
            var properties = pi.Properties;

            var tabName = this.DefaultTabName ?? instance.GetType().Name;
            var categoryName = this.DefaultCategoryName;

            // find the declaring type
            Type declaringType = pi.Descriptor.ComponentType;
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

            var ca = AttributeHelper.GetFirstAttribute<CategoryAttribute>(pd);
            if (ca != null)
            {
                this.CurrentCategory = ca.Category;
                this.CurrentCategoryDeclaringType = declaringType;
            }

            var category = this.CurrentCategory ?? (this.DefaultCategoryName ?? pd.Category);

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

            var displayName = this.GetDisplayName(pd, declaringType);
            var description = this.GetDescription(pd, declaringType);

            pi.DisplayName = this.GetLocalizedString(displayName, declaringType);
            pi.Description = this.GetLocalizedDescription(description, declaringType);

            pi.Category = this.GetLocalizedString(categoryName, this.CurrentCategoryDeclaringType);

            // pi.CategoryDescription = this.GetLocalizedDescription(categoryName, this.CurrentCategoryDeclaringType);
            pi.Tab = this.GetLocalizedString(tabName, this.CurrentCategoryDeclaringType);

            // pi.TabDescription = this.GetLocalizedDescription(tabName, this.CurrentCategoryDeclaringType);

            // Find descriptors by convention
            pi.IsEnabledDescriptor = pi.GetDescriptor(string.Format(this.EnabledPattern, pd.Name));
            pi.IsVisibleDescriptor = pi.GetDescriptor(string.Format(this.VisiblePattern, pd.Name));
            pi.OptionalDescriptor = pi.GetDescriptor(string.Format(this.OptionalPattern, pd.Name));

            var ssa = pi.GetAttribute<SelectorStyleAttribute>();
            if (ssa != null)
            {
                pi.SelectorStyle = ssa.SelectorStyle;
            }

            var fp = pi.GetAttribute<FontPreviewAttribute>();
            pi.PreviewFonts = fp != null;
            if (fp != null)
            {
                pi.FontSize = fp.Size;
                pi.FontWeight = fp.Weight;
                pi.FontFamilyPropertyDescriptor = pi.GetDescriptor(fp.FontFamilyPropertyName);
            }

            pi.IsFontFamilySelector = pi.GetAttribute<FontFamilySelectorAttribute>() != null;

            var ifpa = pi.GetAttribute<InputFilePathAttribute>();
            if (ifpa != null)
            {
                pi.IsFilePath = true;
                pi.IsFileOpenDialog = true;
                pi.FilePathFilter = ifpa.Filter;
                pi.FilePathDefaultExtension = ifpa.DefaultExtension;
            }

            var ofpa = pi.GetAttribute<OutputFilePathAttribute>();
            if (ofpa != null)
            {
                pi.IsFilePath = true;
                pi.IsFileOpenDialog = false;
                pi.FilePathFilter = ofpa.Filter;
                pi.FilePathDefaultExtension = ofpa.DefaultExtension;
            }

            var dpa = pi.GetAttribute<DirectoryPathAttribute>();
            pi.IsDirectoryPath = dpa != null;

            foreach (var da in pi.GetAttributes<DataTypeAttribute>())
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

            foreach (var da in pi.GetAttributes<ColumnAttribute>())
            {
                pi.Columns.Add(da);
            }

            var la = pi.GetAttribute<ListAttribute>();
            if (la != null)
            {
                pi.ListCanAdd = la.CanAdd;
                pi.ListCanRemove = la.CanRemove;
                pi.ListMaximumNumberOfItems = la.MaximumNumberOfItems;
            }

            var ida = pi.GetAttribute<InputDirectionAttribute>();
            if (ida != null)
            {
                pi.InputDirection = ida.InputDirection;
            }

            var eia = pi.GetAttribute<EasyInsertAttribute>();
            if (eia != null)
            {
                pi.EasyInsert = eia.EasyInsert;
            }

            var sia = pi.GetAttribute<SortIndexAttribute>();
            if (sia != null)
            {
                pi.SortIndex = sia.SortIndex;
            }

            var eba = pi.GetAttribute<EnableByAttribute>();
            if (eba != null)
            {
                pi.IsEnabledDescriptor = properties.Find(eba.PropertyName, false);
            }

            var vba = pi.GetAttribute<VisibleByAttribute>();
            if (vba != null)
            {
                pi.IsVisibleDescriptor = properties.Find(vba.PropertyName, false);
            }

            pi.IsOptional = pi.OptionalDescriptor != null;

            var oa = pi.GetAttribute<OptionalAttribute>();
            if (oa != null)
            {
                if (oa.PropertyName != null)
                {
                    pi.OptionalDescriptor = properties.Find(oa.PropertyName, false);
                }

                pi.IsOptional = true;
            }

            pi.IsComment = pi.GetAttribute<CommentAttribute>() != null;
            if (pi.IsComment)
            {
                pi.HeaderPlacement = HeaderPlacement.Hidden;
            }

            pi.IsEditable = pi.GetAttribute<IsEditableAttribute>() != null;

            pi.AutoUpdateText = pi.GetAttribute<AutoUpdateTextAttribute>() != null;

            var ispa = pi.GetAttribute<ItemsSourcePropertyAttribute>();
            if (ispa != null)
            {
                pi.ItemsSourceDescriptor = properties.Find(ispa.PropertyName, false);
            }

            var rpa = pi.GetAttribute<BasePathPropertyAttribute>();
            if (rpa != null)
            {
                pi.RelativePathDescriptor = properties.Find(rpa.BasePathPropertyName, false);
            }

            var fa = pi.GetAttribute<FilterPropertyAttribute>();
            if (fa != null)
            {
                pi.FilterDescriptor = properties.Find(fa.PropertyName, false);
            }

            var dea = pi.GetAttribute<DefaultExtensionPropertyAttribute>();
            if (dea != null)
            {
                pi.DefaultExtensionDescriptor = properties.Find(dea.PropertyName, false);
            }

            var fsa = pi.GetAttribute<FormatStringAttribute>();
            if (fsa != null)
            {
                pi.FormatString = fsa.FormatString;
            }

            var coa = pi.GetAttribute<ConverterAttribute>();
            if (coa != null)
            {
                pi.Converter = Activator.CreateInstance(coa.ConverterType) as IValueConverter;
            }

            var sa = pi.GetAttribute<SlidableAttribute>();
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

            var spa = pi.GetAttribute<SpinnableAttribute>();
            if (spa != null)
            {
                pi.IsSpinnable = true;
                pi.SpinMinimum = spa.Minimum;
                pi.SpinMaximum = spa.Maximum;
                pi.SpinSmallChange = spa.SmallChange;
                pi.SpinLargeChange = spa.LargeChange;
            }

            var wpa = pi.GetAttribute<WidePropertyAttribute>();
            if (wpa != null)
            {
                pi.HeaderPlacement = wpa.ShowHeader ? HeaderPlacement.Above : HeaderPlacement.Hidden;
            }

            var wia = pi.GetAttribute<WidthAttribute>();
            if (wia != null)
            {
                pi.Width = wia.Width;
            }

            var hpa = pi.GetAttribute<HeaderPlacementAttribute>();
            if (hpa != null)
            {
                pi.HeaderPlacement = hpa.HeaderPlacement;
            }

            var ha = pi.GetAttribute<HorizontalAlignmentAttribute>();
            if (ha != null)
            {
                pi.HorizontalAlignment = ha.HorizontalAlignment;
            }

            var hea = pi.GetAttribute<HeightAttribute>();
            if (hea != null)
            {
                pi.Height = hea.Height;
                pi.MinimumHeight = hea.MinimumHeight;
                pi.MaximumHeight = hea.MaximumHeight;
                pi.AcceptsReturn = true;
            }

            var fta = pi.GetAttribute<FillTabAttribute>();
            if (fta != null)
            {
                pi.FillTab = true;
                pi.AcceptsReturn = true;
            }

            if (pi.Descriptor.PropertyType == typeof(TimeSpan) && pi.Converter == null)
            {
                pi.Converter = new TimeSpanToStringConverter();
                pi.ConverterParameter = pi.FormatString;
            }
        }
    }

    /// <summary>
    /// Specifies options for the PropertyControl
    /// </summary>
    public interface IPropertyControlOptions
    {
        /// <summary>
        /// Gets or sets the required attribute.
        /// </summary>
        /// <value> The required attribute. </value>
        Type RequiredAttribute { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to show declared properties only.
        /// </summary>
        /// <value> <c>true</c> if only declared properties should be shown; otherwise, <c>false</c> . </value>
        bool ShowDeclaredOnly { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to show read only properties.
        /// </summary>
        /// <value> <c>true</c> if read only properties should be shown; otherwise, <c>false</c> . </value>
        bool ShowReadOnlyProperties { get; }
    }
}