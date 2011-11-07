// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultPropertyItemFactory.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Provides a default property item factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Windows.Data;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Provides a default property item factory.
    /// </summary>
    public class DefaultPropertyItemFactory : IPropertyItemFactory
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DefaultPropertyItemFactory" /> class.
        /// </summary>
        public DefaultPropertyItemFactory()
        {
            this.EnabledPattern = "Is{0}Enabled";
            this.VisiblePattern = "Is{0}Visible";
            this.OptionalPattern = "Use{0}";
            this.NicifyDisplayNames = true;
            this.InheritCategories = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the default name of the category.
        /// </summary>
        /// <value>The default name of the category.</value>
        public string DefaultCategoryName { get; set; }

        /// <summary>
        ///   Gets or sets the default name of the tab.
        /// </summary>
        /// <value>The default name of the tab.</value>
        public string DefaultTabName { get; set; }

        /// <summary>
        ///   Gets or sets the enabled pattern.
        /// </summary>
        /// <value>The enabled pattern.</value>
        public string EnabledPattern { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether each property should inherit the category attribute from the property declared before.
        /// </summary>
        public bool InheritCategories { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to 'nicify' display names.
        /// </summary>
        /// <remarks>
        ///   The 'nicifiying' adds spaces at the camel bumps.
        /// </remarks>
        /// <value><c>true</c> if display names should be nicified; otherwise, <c>false</c>.</value>
        public bool NicifyDisplayNames { get; set; }

        /// <summary>
        ///   Gets or sets the optional pattern.
        /// </summary>
        /// <value>The optional pattern.</value>
        public string OptionalPattern { get; set; }

        /// <summary>
        ///   Gets or sets the visible pattern.
        /// </summary>
        /// <value>The visible pattern.</value>
        public string VisiblePattern { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the current category.
        /// </summary>
        /// <value>The current category.</value>
        private string CurrentCategory { get; set; }

        /// <summary>
        ///   Gets or sets the type of the current component.
        /// </summary>
        /// <remarks>
        ///   This is used to avoid that Category attributes are inherited from superclass to subclass.
        /// </remarks>
        /// <value>
        ///   The type of the current component.
        /// </value>
        private Type CurrentComponentType { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Variables the display name of the name to.
        /// </summary>
        /// <param name="variableName">
        /// Name of the variable.
        /// </param>
        /// <returns>
        /// The nicify string.
        /// </returns>
        public static string NicifyString(string variableName)
        {
            var sb = new StringBuilder();
            Func<char, bool> IsUpper = ch => ch.ToString() == ch.ToString().ToUpper();
            for (int i = 0; i < variableName.Length; i++)
            {
                if (i > 0 && IsUpper(variableName[i]) && !IsUpper(variableName[i - 1]))
                {
                    sb.Append(" ");
                    if (i == variableName.Length - 1 || IsUpper(variableName[i + 1]))
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
        /// Creates a property item.
        /// </summary>
        /// <param name="pd">
        /// The property descriptor.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// A property item.
        /// </returns>
        public PropertyItem CreatePropertyItem(
            PropertyDescriptor pd, PropertyDescriptorCollection properties, object instance)
        {
            var tabName = this.DefaultTabName ?? instance.GetType().Name;
            var categoryName = this.DefaultCategoryName ?? "Misc";

            if (pd.ComponentType != this.CurrentComponentType)
            {
                this.CurrentCategory = null;
            }

            if (!this.InheritCategories)
            {
                this.CurrentCategory = null;
            }

            this.CurrentComponentType = pd.ComponentType;

            var ca = AttributeHelper.GetFirstAttribute<CategoryAttribute>(pd);
            if (ca != null)
            {
                this.CurrentCategory = ca.Category;
            }

            var category = this.CurrentCategory ?? (this.DefaultCategoryName ?? pd.Category);

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

            var pi = new PropertyItem
                {
                    Descriptor = pd, 
                    Properties = properties, 
                    Instance = instance, 
                    DisplayName = this.GetDisplayName(pd, instance), 
                    ToolTip = this.GetToolTip(pd, instance), 
                    Category = this.GetLocalizedString(categoryName, instance), 
                    Tab = this.GetLocalizedString(tabName, instance), 
                };

            // Find descriptors by convention
            pi.IsEnabledDescriptor = pi.GetDescriptor(string.Format(this.EnabledPattern, pd.Name));
            pi.IsVisibleDescriptor = pi.GetDescriptor(string.Format(this.VisiblePattern, pd.Name));
            pi.OptionalDescriptor = pi.GetDescriptor(string.Format(this.OptionalPattern, pd.Name));

            pi.UseRadioButtons = pi.GetAttribute<RadioButtonsAttribute>() != null;
            var fp = pi.GetAttribute<FontPreviewAttribute>();
            pi.PreviewFonts = fp != null;
            if (fp != null)
            {
                pi.FontSize = fp.Size;
                pi.FontWeight = fp.Weight;
                pi.FontFamilyPropertyDescriptor = pi.GetDescriptor(fp.FontFamilyPropertyName);
            }

            var fpa = pi.GetAttribute<FilePathAttribute>();
            pi.IsFilePath = fpa != null;
            if (fpa != null)
            {
                pi.FilePathFilter = fpa.Filter;
                pi.FilePathDefaultExtension = fpa.DefaultExtension;
                pi.IsFileOpenDialog = fpa.UseOpenDialog;
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

            if (pi.Is(typeof(IList)))
            {
                pi.MinimumHeight = 100;
                pi.MaximumHeight = 240;
                pi.HeaderPlacement = HeaderPlacement.Above;
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
                pi.SliderSnapToTicks=sa.SnapToTicks;
                pi.SliderTickFrequency=sa.TickFrequency;
            }

            var wpa = pi.GetAttribute<WidePropertyAttribute>();
            if (wpa != null)
            {
                pi.HeaderPlacement = wpa.ShowHeader ? HeaderPlacement.Above : HeaderPlacement.Hidden;
            }

            var hpa = pi.GetAttribute<HeaderPlacementAttribute>();
            if (hpa != null)
            {
                pi.HeaderPlacement = hpa.HeaderPlacement;
            }

            var ha = pi.GetAttribute<HeightAttribute>();
            if (ha != null)
            {
                pi.Height = ha.Height;
                pi.MinimumHeight = ha.MinimumHeight;
                pi.MaximumHeight = ha.MaximumHeight;
                pi.AcceptsReturn = true;
            }

            return pi;
        }

        /// <summary>
        /// Gets the display name for the specified property.
        /// </summary>
        /// <param name="pd">
        /// The property descriptor.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// A display name string.
        /// </returns>
        public virtual string GetDisplayName(PropertyDescriptor pd, object instance)
        {
            var displayName = pd.DisplayName;
            if (this.NicifyDisplayNames && pd.DisplayName == pd.Name)
            {
                displayName = NicifyString(displayName);
            }

            return this.GetLocalizedString(displayName, instance);
        }

        /// <summary>
        /// Gets the localized string.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// The localized string.
        /// </returns>
        public virtual string GetLocalizedString(string key, object instance)
        {
            return key;
        }

        /// <summary>
        /// Gets the tool tip for the specified property.
        /// </summary>
        /// <param name="pd">
        /// The property descriptor.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// A tool tip.
        /// </returns>
        public virtual object GetToolTip(PropertyDescriptor pd, object instance)
        {
            if (string.IsNullOrEmpty(pd.Description))
            {
                return null;
            }

            return this.GetLocalizedString(pd.Description, instance);
        }

        /// <summary>
        /// Initializes this factory.
        /// </summary>
        public void Initialize()
        {
            this.CurrentCategory = null;
            this.CurrentComponentType = null;
        }

        #endregion
    }
}