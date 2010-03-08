using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace PropertyEditorLibrary
{
    public enum ShowCategoriesAs
    {
        GroupBox,
        Expander
    };

    /// <summary>
    /// PropertyEditor control.
    /// Set the DataContext to define the contents of the control.
    /// </summary>
    public class PropertyEditor : Control
    {
        #region Private fields

        /// <summary>
        /// The PropertyMap dictionary contains a map of all Properties of the current object being edited.
        /// </summary>
        private readonly Dictionary<string, Property> propertyMap;

        /// <summary>
        /// The PropertyTemplateSelector is used to select the DataTemplate for each property
        /// </summary>
        internal PropertyTemplateSelector PropertyTemplateSelector { get; set; }

        /// <summary>
        /// The CategoryTemplateSelector is used to select the DataTemplate for the categories
        /// </summary>
        internal CategoryTemplateSelector CategoryTemplateSelector { get; set; }

        private const string AppearanceCategory = "PropertyEditor Appearance";
        #endregion

        #region Custom control initialization

        private const string PartGrid = "PART_Grid";
        private const string PartPage = "PART_Page";
        private const string PartTabs = "PART_Tabs";

        private Grid grid;
        private ContentControl contentControl;
        private TabControl tabControl;

        static PropertyEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyEditor),
                                                     new FrameworkPropertyMetadata(typeof(PropertyEditor)));
        }

        public override void OnApplyTemplate()
        {
            if (tabControl == null)
            {
                tabControl = Template.FindName(PartTabs, this) as TabControl;
            }
            if (contentControl == null)
            {
                contentControl = Template.FindName(PartPage, this) as ContentControl;
            }
            if (grid == null)
            {
                grid = Template.FindName(PartGrid, this) as Grid;
            }

            PropertyTemplateSelector.TemplateOwner = grid;
            CategoryTemplateSelector.TemplateOwner = grid;

            // Update the content of the control
            UpdateContent();
        }

        #endregion

        #region Properties

        private static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyEditor)d).UpdateContent();
        }

        #region LabelWidth

        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(double), typeof(PropertyEditor),
                                        new UIPropertyMetadata(100.0));

        /// <summary>
        /// The width of the property labels
        /// </summary>
        [Category(AppearanceCategory)]
        public double LabelWidth
        {
            get { return (double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

        #endregion

        #region ShowReadOnlyProperties

        public static readonly DependencyProperty ShowReadOnlyPropertiesProperty =
            DependencyProperty.Register("ShowReadOnlyProperties", typeof(bool), typeof(PropertyEditor),
                                        new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Show read-only properties.
        /// </summary>
        [Category(AppearanceCategory)]
        public bool ShowReadOnlyProperties
        {
            get { return (bool)GetValue(ShowReadOnlyPropertiesProperty); }
            set { SetValue(ShowReadOnlyPropertiesProperty, value); }
        }

        #endregion

        #region ShowTabs

        public static readonly DependencyProperty ShowTabsProperty =
            DependencyProperty.Register("ShowTabs", typeof(bool), typeof(PropertyEditor),
                                        new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Organize the properties in tabs.
        /// You should use the [Category("Tabname|Groupname")] attribute to define the tabs.
        /// </summary>
        [Category(AppearanceCategory)]
        public bool ShowTabs
        {
            get { return (bool)GetValue(ShowTabsProperty); }
            set { SetValue(ShowTabsProperty, value); }
        }

        #endregion

        #region DeclaredOnly

        public static readonly DependencyProperty DeclaredOnlyProperty =
            DependencyProperty.Register("DeclaredOnly", typeof(bool), typeof(PropertyEditor),
                                        new UIPropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// Show only declared properties (not inherited properties).
        /// </summary>
        [Category(AppearanceCategory)]
        public bool DeclaredOnly
        {
            get { return (bool)GetValue(DeclaredOnlyProperty); }
            set { SetValue(DeclaredOnlyProperty, value); }
        }

        #endregion

        #region SelectedObject (obsolete)

        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register("SelectedObject", typeof(object), typeof(PropertyEditor),
                                        new UIPropertyMetadata(null, SelectedObjectChanged));

        [Obsolete("Using DataContext is preferred.")]
        [Browsable(false)]
        public object SelectedObject
        {
            get { return DataContext; }
            set { DataContext = value; }
        }

        private static void SelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pe = (PropertyEditor)d;
            pe.DataContext = e.NewValue;
        }

        #endregion

        #region ShowEnumAsComboBox
        /// <summary>
        /// Show enum properties as ComboBox or RadioButtonList.
        /// </summary>
        [Category(AppearanceCategory)]
        public bool ShowEnumAsComboBox
        {
            get { return (bool)GetValue(ShowEnumAsComboBoxProperty); }
            set { SetValue(ShowEnumAsComboBoxProperty, value); }
        }

        public static readonly DependencyProperty ShowEnumAsComboBoxProperty =
            DependencyProperty.Register("ShowEnumAsComboBox", typeof(bool), typeof(PropertyEditor),
            new UIPropertyMetadata(true, AppearanceChanged));

        #endregion

        #region ShowCategoriesAs
        [Category(AppearanceCategory)]
        public ShowCategoriesAs ShowCategoriesAs
        {
            get { return (ShowCategoriesAs)GetValue(ShowCategoriesAsProperty); }
            set { SetValue(ShowCategoriesAsProperty, value); }
        }

        public static readonly DependencyProperty ShowCategoriesAsProperty =
            DependencyProperty.Register("ShowCategoriesAs", typeof(ShowCategoriesAs), typeof(PropertyEditor),
            new UIPropertyMetadata(ShowCategoriesAs.GroupBox, AppearanceChanged));

        #endregion

        #region DefaultCategoryName Dependency Property

        public string DefaultCategoryName
        {
            get { return (string)GetValue(DefaultCategoryNameProperty); }
            set { SetValue(DefaultCategoryNameProperty, value); }
        }

        public static readonly DependencyProperty DefaultCategoryNameProperty =
            DependencyProperty.Register("DefaultCategoryName", typeof(string), typeof(PropertyEditor),
            new UIPropertyMetadata("Properties", AppearanceChanged));

        #endregion

        #region LabelAlignment

        public static readonly DependencyProperty LabelAlignmentProperty =
            DependencyProperty.Register("LabelAlignment", typeof(HorizontalAlignment), typeof(PropertyEditor),
                                        new UIPropertyMetadata(HorizontalAlignment.Left, AppearanceChanged));

        /// <summary>
        /// Gets or sets the alignment of property labels.
        /// </summary>
        public HorizontalAlignment LabelAlignment
        {
            get { return (HorizontalAlignment)GetValue(LabelAlignmentProperty); }
            set { SetValue(LabelAlignmentProperty, value); }
        }

        #endregion

        #region Localizer dependency property
        /// <summary>
        /// Implement the Localizer to translate the tab, category and property strings and tooltips
        /// </summary>
        [Browsable(false)]
        public ILocalizer Localizer
        {
            get { return (ILocalizer)GetValue(LocalizerProperty); }
            set { SetValue(LocalizerProperty, value); }
        }

        public static readonly DependencyProperty LocalizerProperty =
            DependencyProperty.Register("Localizer", typeof(ILocalizer), typeof(PropertyEditor),
            new UIPropertyMetadata(null));

        #endregion

        #region PropertySetter Dependency Property

        [Browsable(false)]
        public IPropertySetter PropertySetter
        {
            get { return (IPropertySetter)GetValue(PropertySetterProperty); }
            set { SetValue(PropertySetterProperty, value); }
        }

        public static readonly DependencyProperty PropertySetterProperty =
            DependencyProperty.Register("PropertySetter", typeof(IPropertySetter), typeof(PropertyEditor),
            new UIPropertyMetadata(null));

        #endregion

        #region ImageProvider Dependency Property

        /// <summary>
        /// The ImageProvider can be used to provide images to the Tab icons.
        /// </summary>
        [Browsable(false)]
        public IImageProvider ImageProvider
        {
            get { return (IImageProvider)GetValue(ImageProviderProperty); }
            set { SetValue(ImageProviderProperty, value); }
        }

        public static readonly DependencyProperty ImageProviderProperty =
            DependencyProperty.Register("ImageProvider", typeof(IImageProvider), typeof(PropertyEditor),
            new UIPropertyMetadata(null));

        #endregion

        #region PropertyAttributeProvider Dependency Property

        public IPropertyAttributeProvider PropertyAttributeProvider
        {
            get { return (IPropertyAttributeProvider)GetValue(PropertyAttributeProviderProperty); }
            set { SetValue(PropertyAttributeProviderProperty, value); }
        }

        public static readonly DependencyProperty PropertyAttributeProviderProperty =
            DependencyProperty.Register("PropertyAttributeProvider", typeof(IPropertyAttributeProvider), typeof(PropertyEditor),
            new UIPropertyMetadata(null));

        #endregion

        #region Custom Editors

        /// <summary>
        /// Collection of custom editors
        /// </summary>
        [Browsable(false)]
        public Collection<TypeEditor> Editors
        {
            // the collection is stored in the propertyTemplateSelector
            get { return PropertyTemplateSelector.Editors; }
        }

        #endregion

        #endregion

        #region PropertyValueChanged event

        public static readonly RoutedEvent PropertyValueChangedEvent = EventManager.RegisterRoutedEvent(
            "PropertyValueChanged",
            RoutingStrategy.Bubble,
            typeof(EventHandler<PropertyValueChangedEventArgs>),
            typeof(PropertyEditor));

        public event EventHandler<PropertyValueChangedEventArgs> PropertyChanged
        {
            add { AddHandler(PropertyValueChangedEvent, value); }
            remove { RemoveHandler(PropertyValueChangedEvent, value); }
        }

        /// <summary>
        /// Invoke this method to raise a PropertyChanged event
        /// </summary>
        private void RaisePropertyChangedEvent(string propertyName, object oldValue, object newValue)
        {
            var args = new PropertyValueChangedEventArgs();
            args.PropertyName = propertyName;
            args.OldValue = oldValue;
            args.NewValue = newValue;
            args.RoutedEvent = PropertyValueChangedEvent;
            RaiseEvent(args);
        }

        #endregion

        public PropertyEditor()
        {
            propertyMap = new Dictionary<string, Property>();

            PropertyTemplateSelector = new PropertyTemplateSelector();
            CategoryTemplateSelector = new CategoryTemplateSelector();
            DataContextChanged += OnDataContextChanged;
        }

        /// <summary>
        /// Updates the content of the control
        /// (after initialization and DataContext changes)
        /// </summary>
        public void UpdateContent()
        {
            if (tabControl == null || contentControl == null)
                return;

            // Get the property model (tabs, categories and properties)
            var propertyTabs = GetPropertyModel(DataContext);

            if (ShowTabs)
            {
                tabControl.ItemsSource = propertyTabs;
                if (tabControl.Items.Count > 0)
                    tabControl.SelectedIndex = 0;
                tabControl.Visibility = Visibility.Visible;
                contentControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                var tab = propertyTabs.Count > 0 ? propertyTabs[0] : null;
                contentControl.Content = tab;
                tabControl.Visibility = Visibility.Collapsed;
                contentControl.Visibility = Visibility.Visible;
            }

            UpdatePropertyStates(DataContext);
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateContent();
        }

        /// <summary>
        /// This method takes an object Instance and creates the property model.
        /// The properties are organized in a hierarchy
        /// PropertyTab
        ///   PropertyCategory
        ///     Property|OptionalProperty|WideProperty
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>Collection of PropertyTabs</returns>
        public virtual IList<PropertyTab> GetPropertyModel(object instance)
        {
            var result = new List<PropertyTab>();

            if (instance == null)
                return result;

            Type instanceType = instance.GetType();

            // The GetPropertyModel method does not return properties in a particular order, 
            // such as alphabetical or declaration order. Your code must not depend on the 
            // order in which properties are returned, because that order varies.

            // find the DeclaredOnly properties
            var declaredOnlyProperties =
                instanceType.GetProperties(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            // find all properties of this Instance
            var properties = TypeDescriptor.GetProperties(instance, true);

            PropertyTab currentTab = null;
            PropertyCategory currentCategory = null;

            string categoryName = DefaultCategoryName;

            // Use the type name of the Instance as the default tab name
            string tabName = instanceType.Name;

            // Initialize the template selectors
            PropertyTemplateSelector.ShowEnumAsComboBox = ShowEnumAsComboBox;
            CategoryTemplateSelector.ShowAs = ShowCategoriesAs;

            propertyMap.Clear();

            foreach (PropertyDescriptor descriptor in properties)
            {
                if (descriptor==null)
                    continue;
                PropertyDescriptor propertyDescriptor = descriptor;

                // Declared properties
                if (DeclaredOnly && declaredOnlyProperties.FirstOrDefault(p => p.Name == propertyDescriptor.Name) == null)
                    continue;

                // Skip properties marked with [Browsable(false)]
                if (!descriptor.IsBrowsable)
                    continue;

                // Read-only properties
                if (!ShowReadOnlyProperties && descriptor.IsReadOnly)
                    continue;

                ParseTabAndCategory(descriptor, ref tabName, ref categoryName);

                // Debug.WriteLine(String.Format("Adding property {0}.{1}.{2}", tabName, categoryName, descriptor.Name));

                GetOrCreateTab(instanceType, result, tabName, ref currentTab, ref currentCategory);
                GetOrCreateCategory(instanceType, categoryName, currentTab, ref currentCategory);

                var property = CreateProperty(descriptor, instance);
                propertyMap.Add(descriptor.Name, property);

                property.PropertyChanged += OnPropertyChanged;
                UpdatePropertyHeader(property);
                currentCategory.Properties.Add(property);
            }

            foreach (Property prop in propertyMap.Values)
            {
                var oprop = prop as OptionalProperty;
                if (oprop == null)
                    continue;

                if (!string.IsNullOrEmpty(oprop.OptionalPropertyName))
                {
                    if (propertyMap.ContainsKey(oprop.OptionalPropertyName))
                    {
                        Debug.WriteLine(String.Format("Optional properties ({0}) should not be [Browsable].", oprop.OptionalPropertyName));
                        // todo remove OptionalPropertyName from the property bag...
                        // prop.IsOptional = false;
                    }
                }
            }

            // todo: use sort algorithm that does not change order when sort order keys are equal
            SortPropertyModel(result);
            return result;
        }

        void SortPropertyModel(List<PropertyTab> result)
        {
            // SortPropertyModel tabs
            // result.Sort(); 
            result.OrderBy(t => t.SortOrder);
            foreach (var tab in result)
            {
                // SortPropertyModel category
                // tab.Categories.Sort();
                tab.Categories.OrderBy(c => c.SortOrder);
                foreach (var cat in tab.Categories)
                {
                    // SortPropertyModel properties
                    // cat.Properties.Sort();
                    cat.Properties.OrderBy(p => p.SortOrder);
                }
            }
        }

        /// <summary>
        /// If a CategoryAttributes is given as
        /// [Category("TabA|GroupB")]
        /// this will be parsed into tabName="TabA" and categoryName="GroupB"
        /// 
        /// If the CategoryAttribute is
        /// [Category("GroupC")]
        /// the method will not change tabName, but set categoryName="GroupC"
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="tabName"></param>
        /// <param name="categoryName"></param>
        private void ParseTabAndCategory(PropertyDescriptor descriptor, ref string tabName, ref string categoryName)
        {
            var ca = PropertyHelper.GetAttribute<CategoryAttribute>(descriptor);
            if (ca == null || ca.Category == null || string.IsNullOrEmpty(ca.Category))
                return;

            string[] items = ca.Category.Split('|');
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

        /// <summary>
        /// Create a property
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        private Property CreateProperty(PropertyDescriptor descriptor, object instance)
        {
            // If no provider is define, use the default provider
            if (PropertyAttributeProvider == null)
                PropertyAttributeProvider = new DefaultPropertyAttributeProvider();

            // Get the attributes from the provider
            var pa = PropertyAttributeProvider.GetAttributes(descriptor);

            Property property = null;

            // Optional
            if (pa.OptionalProperty != null)
                property = new OptionalProperty(instance, descriptor, pa.OptionalProperty, this);

            // Wide
            if (pa.IsWide)
                property = new WideProperty(instance, descriptor, pa.NoHeader, this);

            // Slidable
            if (pa.IsSlidable)
                property = new SlidableProperty(instance, descriptor, this)
                           {
                               SliderMinimum = pa.SliderMinimum,
                               SliderMaximum = pa.SliderMaximum,
                               SliderLargeChange = pa.SliderLargeChange,
                               SliderSmallChange = pa.SliderSmallChange
                           };

            // Normal property
            if (property == null)
                property = new Property(instance, descriptor, this);

            property.SortOrder = pa.SortOrder;
            property.Height = pa.Height;
            property.AcceptsReturn = property.Height > 0;
            property.FormatString = pa.FormatString;

            return property;
        }

        private void GetOrCreateCategory(Type instanceType, string categoryName, PropertyTab currentTab, ref PropertyCategory currentCategory)
        {
            if (currentCategory == null || currentCategory.Name != categoryName)
            {
                currentCategory = currentTab.Categories.FirstOrDefault(c => c.Name == categoryName);
                if (currentCategory == null)
                {
                    currentCategory = new PropertyCategory(categoryName, this);
                    currentTab.Categories.Add(currentCategory);
                    UpdateCategoryHeader(instanceType, currentCategory);
                }
            }
        }

        private void GetOrCreateTab(Type instanceType, ICollection<PropertyTab> tabs, string tabName, ref PropertyTab currentTab, ref PropertyCategory currentCategory)
        {
            if (currentTab == null || (currentTab.Name != tabName && ShowTabs))
            {
                currentTab = tabs.FirstOrDefault(t => t.Name == tabName);

                if (currentTab == null)
                {
                    // force to find/create a new category as well
                    currentCategory = null;
                    currentTab = CreateTab(tabName);
                    tabs.Add(currentTab);
                    UpdateTabHeader(instanceType, currentTab);
                }
            }
        }

        PropertyTab CreateTab(string tabName)
        {
            var tab = new PropertyTab(tabName, this);
            if (ImageProvider != null)
                tab.Icon = ImageProvider.GetImage(DataContext.GetType(), Name);
            return tab;
        }

        /// <summary>
        /// Updates the property header and tooltip
        /// </summary>
        /// <param name="property"></param>
        private void UpdatePropertyHeader(Property property)
        {
            var type = property.Descriptor.PropertyType;
            var key = property.Descriptor.Name;
            property.Header = GetLocalizedString(type, key);
            property.ToolTip = GetLocalizedTooltip(type, key);

            // [DisplayName(..)] and [Description(...)] attributes overrides the localized strings
            var dna = PropertyHelper.GetAttribute<DisplayNameAttribute>(property.Descriptor);
            if (dna != null)
                property.Header = dna.DisplayName;
            var da = PropertyHelper.GetAttribute<DescriptionAttribute>(property.Descriptor);
            if (da != null)
                property.ToolTip = da.Description;
        }

        /// <summary>
        /// Updates the category (expander/groupbox) header and tooltip
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="category"></param>
        private void UpdateCategoryHeader(Type instanceType, PropertyCategory category)
        {
            category.Header = GetLocalizedString(instanceType, category.Name);
            category.ToolTip = GetLocalizedTooltip(instanceType, category.Name);
        }

        /// <summary>
        /// Updates the tab header and tooltip
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="tab"></param>
        private void UpdateTabHeader(Type instanceType, PropertyTab tab)
        {
            tab.Header = GetLocalizedString(instanceType, tab.Name);
            tab.ToolTip = GetLocalizedTooltip(instanceType, tab.Name);
        }

        #region Property changes

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = sender as Property;
            if (property != null)
            {
                RaisePropertyChangedEvent(property.PropertyName, null, property.Value);
                UpdateOptionalProperties(property);
            }

            if (e.PropertyName != "IsEnabled" && e.PropertyName != "IsVisible")
                UpdatePropertyStates(DataContext);
        }

        private void UpdatePropertyStates(object instance)
        {
            var psi = instance as IPropertyState;
            if (psi == null)
                return;
            var ps = new PropertyStates();
            psi.UpdatePropertyStates(ps);
            foreach (var ep in ps.EnabledProperties)
            {
                var p = propertyMap[ep.Key];
                if (p.IsEnabled != ep.Value)
                    p.IsEnabled = ep.Value;
            }
        }

        /// <summary>
        /// Update IsEnabled on properties marked [Optional(..)]
        /// </summary>
        /// <param name="property"></param>
        private void UpdateOptionalProperties(Property property)
        {
            foreach (Property prop in propertyMap.Values)
            {
                var oprop = prop as OptionalProperty;
                if (oprop == null) continue;
                if (oprop.OptionalPropertyName == property.PropertyName)
                {
                    if (property.Value is bool)
                    {
                        oprop.IsEnabled = (bool)property.Value;
                    }
                }
            }
        }
        #endregion

        #region Localization

        private string GetLocalizedString(Type instanceType, string key)
        {
            string result = key;
            if (Localizer != null)
            {
                result = Localizer.GetString(instanceType, key);
            }
            if (String.IsNullOrEmpty(result))
                result = key;
            return result;
        }

        private object GetLocalizedTooltip(Type instanceType, string key)
        {
            object tooltip = null;
            if (Localizer != null)
            {
                tooltip = Localizer.GetTooltip(instanceType, key);
            }

            if (tooltip is string)
            {
                var s = (string)tooltip;
                s = s.Trim();
                if (s.Length == 0)
                    tooltip = null;
            }
            return tooltip;
        }
        #endregion

    }

    /// <summary>
    /// Event args for the PropertyValueChanged event
    /// </summary>
    public class PropertyValueChangedEventArgs : RoutedEventArgs
    {
        public string PropertyName { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }

}