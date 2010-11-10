using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PropertyEditorLibrary
{
    public enum ShowCategoriesAs
    {
        GroupBox,
        Expander,
        Header
    } ;

    /// <summary>
    /// PropertyEditor control.
    /// Set the SelectedObject to define the contents of the control.
    /// </summary>
    public class PropertyEditor : Control
    {
        private const string CATEGORY_APPEARANCE = "Appearance";
        private const string PART_GRID = "PART_Grid";
        private const string PART_PAGE = "PART_Page";
        private const string PART_TABS = "PART_Tabs";

        public static readonly DependencyProperty PropertyTemplateSelectorProperty =
            DependencyProperty.Register("PropertyTemplateSelector", typeof (PropertyTemplateSelector),
                                        typeof (PropertyEditor), new UIPropertyMetadata(null));

        public static readonly DependencyProperty CategoryTemplateSelectorProperty =
            DependencyProperty.Register("CategoryTemplateSelector", typeof (CategoryTemplateSelector),
                                        typeof (PropertyEditor), new UIPropertyMetadata(null));

        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof (double), typeof (PropertyEditor),
                                        new UIPropertyMetadata(100.0));

        public static readonly DependencyProperty ShowReadOnlyPropertiesProperty =
            DependencyProperty.Register("ShowReadOnlyProperties", typeof (bool), typeof (PropertyEditor),
                                        new UIPropertyMetadata(true, AppearanceChanged));

        public static readonly DependencyProperty ShowTabsProperty =
            DependencyProperty.Register("ShowTabs", typeof (bool), typeof (PropertyEditor),
                                        new UIPropertyMetadata(true, AppearanceChanged));

        public static readonly DependencyProperty DeclaredOnlyProperty =
            DependencyProperty.Register("DeclaredOnly", typeof (bool), typeof (PropertyEditor),
                                        new UIPropertyMetadata(false, AppearanceChanged));

        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register("SelectedObject", typeof (object), typeof (PropertyEditor),
                                        new UIPropertyMetadata(null, SelectedObjectChanged));

        public static readonly DependencyProperty SelectedObjectsProperty =
            DependencyProperty.Register("SelectedObjects", typeof (IEnumerable), typeof (PropertyEditor),
                                        new UIPropertyMetadata(null, SelectedObjectsChanged));


        public static readonly DependencyProperty ShowBoolHeaderProperty =
            DependencyProperty.Register("ShowBoolHeader", typeof (bool), typeof (PropertyEditor),
                                        new UIPropertyMetadata(true, AppearanceChanged));

        public static readonly DependencyProperty ShowEnumAsComboBoxProperty =
            DependencyProperty.Register("ShowEnumAsComboBox", typeof (bool), typeof (PropertyEditor),
                                        new UIPropertyMetadata(true, AppearanceChanged));

        public static readonly DependencyProperty ShowCategoriesAsProperty =
            DependencyProperty.Register("ShowCategoriesAs", typeof (ShowCategoriesAs), typeof (PropertyEditor),
                                        new UIPropertyMetadata(ShowCategoriesAs.GroupBox, AppearanceChanged));

        public static readonly DependencyProperty DefaultCategoryNameProperty =
            DependencyProperty.Register("DefaultCategoryName", typeof (string), typeof (PropertyEditor),
                                        new UIPropertyMetadata("Properties", AppearanceChanged));

        public static readonly DependencyProperty LabelAlignmentProperty =
            DependencyProperty.Register("LabelAlignment", typeof (HorizontalAlignment), typeof (PropertyEditor),
                                        new UIPropertyMetadata(HorizontalAlignment.Left, AppearanceChanged));

        public static readonly DependencyProperty LocalizationServiceProperty =
            DependencyProperty.Register("LocalizationService", typeof (ILocalizationService), typeof (PropertyEditor),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty ImageProviderProperty =
            DependencyProperty.Register("ImageProvider", typeof (IImageProvider), typeof (PropertyEditor),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty RequiredAttributeProperty =
            DependencyProperty.Register("RequiredAttribute", typeof (Type), typeof (PropertyEditor),
                                        new UIPropertyMetadata(null));

        private static readonly RoutedEvent PropertyValueChangedEvent = EventManager.RegisterRoutedEvent(
            "PropertyValueChanged",
            RoutingStrategy.Bubble,
            typeof (EventHandler<PropertyValueChangedEventArgs>),
            typeof (PropertyEditor));

        public static readonly DependencyProperty DefaultTabNameProperty =
            DependencyProperty.Register("DefaultTabName", typeof (string), typeof (PropertyEditor),
                                        new UIPropertyMetadata(null, AppearanceChanged));

        public static readonly DependencyProperty ErrorTemplateProperty =
            DependencyProperty.Register("ErrorTemplate", typeof (DataTemplate), typeof (PropertyEditor),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty WarningTemplateProperty =
            DependencyProperty.Register("WarningTemplate", typeof (DataTemplate), typeof (PropertyEditor),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty ErrorBorderThicknessProperty =
            DependencyProperty.Register("ErrorBorderThickness", typeof (Thickness), typeof (PropertyEditor),
                                        new UIPropertyMetadata(new Thickness(4, 1, 4, 1)));


        public static readonly DependencyProperty PropertyStateProviderProperty =
            DependencyProperty.Register("PropertyStateProvider", typeof (IPropertyStateProvider),
                                        typeof (PropertyEditor), new UIPropertyMetadata(null));


        /// <summary>
        /// The PropertyMap dictionary contains a map of all Properties of the current object being edited.
        /// </summary>
        private readonly Dictionary<string, PropertyViewModel> propertyMap;

        private ContentControl contentControl;
        private Grid grid;
        private IList<TabViewModel> model;
        private IPropertyViewModelFactory propertyViewModelFactory;
        private TabControl tabControl;

        static PropertyEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (PropertyEditor),
                                                     new FrameworkPropertyMetadata(typeof (PropertyEditor)));
        }

        public PropertyEditor()
        {
            propertyMap = new Dictionary<string, PropertyViewModel>();

            PropertyTemplateSelector = new PropertyTemplateSelector(this);
            CategoryTemplateSelector = new CategoryTemplateSelector(this);
        }

        public IPropertyStateProvider PropertyStateProvider
        {
            get { return (IPropertyStateProvider) GetValue(PropertyStateProviderProperty); }
            set { SetValue(PropertyStateProviderProperty, value); }
        }

        public Thickness ErrorBorderThickness
        {
            get { return (Thickness) GetValue(ErrorBorderThicknessProperty); }
            set { SetValue(ErrorBorderThicknessProperty, value); }
        }

        public IEnumerable SelectedObjects
        {
            get { return (IEnumerable) GetValue(SelectedObjectsProperty); }
            set { SetValue(SelectedObjectsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the property view model factory.
        /// This factory is used to generate the view model based on the property descriptors.
        /// You can override this factory to create the view model based on your own attributes.
        /// </summary>
        /// <value>The property view model factory.</value>
        [Browsable(false)]
        public IPropertyViewModelFactory PropertyViewModelFactory
        {
            get
            {
                if (propertyViewModelFactory == null)
                {
                    propertyViewModelFactory = new DefaultPropertyViewModelFactory(this);
                }
                return propertyViewModelFactory;
            }
            set { propertyViewModelFactory = value; }
        }

        /// <summary>
        /// Collection of custom editors
        /// </summary>
        [Browsable(false)]
        public Collection<TypeEditor> Editors
        {
            // the collection is stored in the propertyTemplateSelector
            get { return PropertyTemplateSelector.Editors; }
        }

        /// <summary>
        /// The width of the property labels
        /// </summary>
        [Category(CATEGORY_APPEARANCE)]
        public double LabelWidth
        {
            get { return (double) GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

        /// <summary>
        /// Show read-only properties.
        /// </summary>
        [Category(CATEGORY_APPEARANCE)]
        public bool ShowReadOnlyProperties
        {
            get { return (bool) GetValue(ShowReadOnlyPropertiesProperty); }
            set { SetValue(ShowReadOnlyPropertiesProperty, value); }
        }

        /// <summary>
        /// Organize the properties in tabs.
        /// You should use the [Category("Tabname|Groupname")] attribute to define the tabs.
        /// </summary>
        [Category(CATEGORY_APPEARANCE)]
        public bool ShowTabs
        {
            get { return (bool) GetValue(ShowTabsProperty); }
            set { SetValue(ShowTabsProperty, value); }
        }

        /// <summary>
        /// Show only declared properties (not inherited properties).
        /// Specifies that only members declared at the level of the supplied type's hierarchy 
        /// should be considered. Inherited members are not considered.
        /// </summary>
        [Category(CATEGORY_APPEARANCE)]
        public bool DeclaredOnly
        {
            get { return (bool) GetValue(DeclaredOnlyProperty); }
            set { SetValue(DeclaredOnlyProperty, value); }
        }

        [Browsable(false)]
        public object SelectedObject
        {
            get { return GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        /// <summary>
        /// Show enum properties as ComboBox or RadioButtonList.
        /// </summary>
        [Category(CATEGORY_APPEARANCE)]
        public bool ShowBoolHeader
        {
            get { return (bool) GetValue(ShowBoolHeaderProperty); }
            set { SetValue(ShowBoolHeaderProperty, value); }
        }

        /// <summary>
        /// Show enum properties as ComboBox or RadioButtonList.
        /// </summary>
        [Category(CATEGORY_APPEARANCE)]
        public bool ShowEnumAsComboBox
        {
            get { return (bool) GetValue(ShowEnumAsComboBoxProperty); }
            set { SetValue(ShowEnumAsComboBoxProperty, value); }
        }

        [Category(CATEGORY_APPEARANCE)]
        public ShowCategoriesAs ShowCategoriesAs
        {
            get { return (ShowCategoriesAs) GetValue(ShowCategoriesAsProperty); }
            set { SetValue(ShowCategoriesAsProperty, value); }
        }

        public string DefaultTabName
        {
            get { return (string) GetValue(DefaultTabNameProperty); }
            set { SetValue(DefaultTabNameProperty, value); }
        }


        public string DefaultCategoryName
        {
            get { return (string) GetValue(DefaultCategoryNameProperty); }
            set { SetValue(DefaultCategoryNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the alignment of property labels.
        /// </summary>
        public HorizontalAlignment LabelAlignment
        {
            get { return (HorizontalAlignment) GetValue(LabelAlignmentProperty); }
            set { SetValue(LabelAlignmentProperty, value); }
        }

        /// <summary>
        /// Implement the LocalizationService to translate the tab, category and property strings and tooltips
        /// </summary>
        [Browsable(false)]
        public ILocalizationService LocalizationService
        {
            get { return (ILocalizationService) GetValue(LocalizationServiceProperty); }
            set { SetValue(LocalizationServiceProperty, value); }
        }

        /// <summary>
        /// The ImageProvider can be used to provide images to the Tab icons.
        /// </summary>
        [Browsable(false)]
        public IImageProvider ImageProvider
        {
            get { return (IImageProvider) GetValue(ImageProviderProperty); }
            set { SetValue(ImageProviderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the required attribute type.
        /// If the required attribute type is set, only properties where this attribute is set will be shown.
        /// </summary>
        [Browsable(false)]
        public Type RequiredAttribute
        {
            get { return (Type) GetValue(RequiredAttributeProperty); }
            set { SetValue(RequiredAttributeProperty, value); }
        }

        /// <summary>
        /// The PropertyTemplateSelector is used to select the DataTemplate for each PropertyViewModel
        /// </summary>
        [Browsable(false)]
        public PropertyTemplateSelector PropertyTemplateSelector
        {
            get { return (PropertyTemplateSelector) GetValue(PropertyTemplateSelectorProperty); }
            set { SetValue(PropertyTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// The CategoryTemplateSelector is used to select the DataTemplate for the CategoryViewModels
        /// </summary>
        [Browsable(false)]
        public CategoryTemplateSelector CategoryTemplateSelector
        {
            get { return (CategoryTemplateSelector) GetValue(CategoryTemplateSelectorProperty); }
            set { SetValue(CategoryTemplateSelectorProperty, value); }
        }

        public DataTemplate ErrorTemplate
        {
            get { return (DataTemplate) GetValue(ErrorTemplateProperty); }
            set { SetValue(ErrorTemplateProperty, value); }
        }

        public DataTemplate WarningTemplate
        {
            get { return (DataTemplate) GetValue(WarningTemplateProperty); }
            set { SetValue(WarningTemplateProperty, value); }
        }

        public static RoutedEvent rortemplate
        {
            get { return PropertyValueChangedEvent; }
        }

        public override void OnApplyTemplate()
        {
            if (tabControl == null)
            {
                tabControl = Template.FindName(PART_TABS, this) as TabControl;
            }
            if (contentControl == null)
            {
                contentControl = Template.FindName(PART_PAGE, this) as ContentControl;
            }
            if (grid == null)
            {
                grid = Template.FindName(PART_GRID, this) as Grid;
            }

            PropertyTemplateSelector.TemplateOwner = grid;
            CategoryTemplateSelector.TemplateOwner = grid;

            Loaded += PropertyEditor_Loaded;
            Unloaded += PropertyEditor_Unloaded;
        }

        private void PropertyEditor_Loaded(object sender, RoutedEventArgs e)
        {
            // Update the content of the control
            UpdateContent();
        }

        private void PropertyEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            // Unsubscribe all property change event handlers
            ClearModel();
        }

        private static void SelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pe = (PropertyEditor) d;
            pe.UpdateContent();
        }

        private static void SelectedObjectsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pe = (PropertyEditor) d;
            pe.UpdateContent();
        }

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
            var args = new PropertyValueChangedEventArgs
                           {
                               PropertyName = propertyName,
                               OldValue = oldValue,
                               NewValue = newValue,
                               RoutedEvent = PropertyValueChangedEvent
                           };
            RaiseEvent(args);
        }

        private static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyEditor) d).UpdateContent();
        }

        /// <summary>
        /// Updates the content of the control
        /// (after initialization and SelectedObject changes)
        /// </summary>
        public void UpdateContent()
        {
            if (!IsLoaded)
            {
                return;
            }

            if (tabControl == null)
            {
                throw new InvalidOperationException(PART_TABS + " cannot be found in the PropertyEditor template.");
            }
            if (contentControl == null)
            {
                throw new InvalidOperationException(PART_PAGE + " cannot be found in the PropertyEditor template.");
            }

            ClearModel();

            // Get the property model (tabs, categories and properties)
            if (SelectedObjects != null)
            {
                model = CreatePropertyModel(SelectedObjects, true);
            }
            else
            {
                model = CreatePropertyModel(SelectedObject, false);
            }

            if (ShowTabs)
            {
                tabControl.ItemsSource = model;
                if (tabControl.Items.Count > 0)
                {
                    tabControl.SelectedIndex = 0;
                }
                tabControl.Visibility = Visibility.Visible;
                contentControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                var tab = model.Count > 0 ? model[0] : null;
                contentControl.Content = tab;
                tabControl.Visibility = Visibility.Collapsed;
                contentControl.Visibility = Visibility.Visible;
            }

            UpdatePropertyStates(SelectedObject);
            UpdateErrorInfo();
        }

        private void ClearModel()
        {
            // Unsubscribe all property value changed events
            if (model != null)
            {
                foreach (TabViewModel tab in model)
                {
                    foreach (CategoryViewModel cat in tab.Categories)
                    {
                        foreach (PropertyViewModel prop in cat.Properties)
                        {
                            prop.UnsubscribeValueChanged();
                        }
                    }
                }
            }
            model = null;
        }


        /// <summary>
        /// This method takes an object Instance and creates the property model.
        /// The properties are organized in a hierarchy
        /// PropertyTab
        ///   PropertyCategory
        ///     Property|OptionalProperty|WideProperty|CheckBoxProperty
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="isEnumerable"></param>
        /// <returns>Collection of tab ViewModels</returns>
        public virtual IList<TabViewModel> CreatePropertyModel(object instance, bool isEnumerable)
        {
            if (instance == null)
            {
                return null;
            }

            Type instanceType;

            // find all properties of the instance type
            PropertyDescriptorCollection properties;
            if (isEnumerable)
            {
                instanceType = TypeHelper.FindBiggestCommonType(instance as IEnumerable);
                properties = TypeDescriptor.GetProperties(instanceType);
            }
            else
            {
                instanceType = instance.GetType();
                properties = TypeDescriptor.GetProperties(instance);
            }

            if (instanceType == null)
            {
                return null;
            }

            // The GetPropertyModel method does not return properties in a particular order, 
            // such as alphabetical or declaration order. Your code must not depend on the 
            // order in which properties are returned, because that order varies.

            TabViewModel currentTabViewModel = null;
            CategoryViewModel currentCategoryViewModel = null;

            // Setting the default tab name
            // Use the type name of the Instance as the default tab name
            string tabName = DefaultTabName ?? instanceType.Name;

            // Setting the default category name
            string categoryName = DefaultCategoryName;

            propertyMap.Clear();
            int sortOrder = 0;

            var result = new List<TabViewModel>();

            foreach (PropertyDescriptor descriptor in properties)
            {
                if (descriptor == null)
                {
                    continue;
                }

                // TODO: should not show attached dependency properties?
                if (DeclaredOnly && descriptor.ComponentType != instanceType)
                {
                    continue;
                }

                // Skip properties marked with [Browsable(false)]
                if (!descriptor.IsBrowsable)
                {
                    continue;
                }

                // Read-only properties
                if (!ShowReadOnlyProperties && descriptor.IsReadOnly)
                {
                    continue;
                }

                // If RequiredAttribute is set, skip properties that don't have the given attribute
                if (RequiredAttribute != null &&
                    !AttributeHelper.ContainsAttributeOfType(descriptor.Attributes, RequiredAttribute))
                {
                    continue;
                }

                // Create Property ViewModel
                PropertyViewModel propertyViewModel = PropertyViewModelFactory.CreateViewModel(instance, descriptor);
                propertyViewModel.IsEnumerable = isEnumerable;
                propertyViewModel.SubscribeValueChanged();

                LocalizePropertyHeader(instanceType, propertyViewModel);
                propertyMap.Add(propertyViewModel.Name, propertyViewModel);
                propertyViewModel.PropertyChanged += OnPropertyChanged;

                if (propertyViewModel.SortOrder == int.MinValue)
                {
                    propertyViewModel.SortOrder = sortOrder;
                }
                sortOrder = propertyViewModel.SortOrder;

                ParseTabAndCategory(descriptor, ref tabName, ref categoryName);

                GetOrCreateTab(instanceType, result, tabName, sortOrder, ref currentTabViewModel,
                               ref currentCategoryViewModel);
                GetOrCreateCategory(instanceType, categoryName, sortOrder, currentTabViewModel,
                                    ref currentCategoryViewModel);

                currentCategoryViewModel.Properties.Add(propertyViewModel);
            }

            // Check that properties used as optional properties are not Browsable
            CheckOptionalProperties();

            // Sort the model using a stable sort algorithm 
            return SortPropertyModel(result);
        }

        private void CheckOptionalProperties()
        {
            foreach (PropertyViewModel prop in propertyMap.Values)
            {
                var oprop = prop as OptionalPropertyViewModel;
                if (oprop == null)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(oprop.OptionalPropertyName))
                {
                    if (propertyMap.ContainsKey(oprop.OptionalPropertyName))
                    {
                        Debug.WriteLine(String.Format("Optional properties ({0}) should not be [Browsable].",
                                                      oprop.OptionalPropertyName));
                        // remove OptionalPropertyName from the property bag...
                        // prop.IsOptional = false;
                    }
                }
            }
        }

        private List<TabViewModel> SortPropertyModel(List<TabViewModel> result)
        {
            // Use LINQ to stable sort tabs, categories and properties.
            // (important that it is a stable sort algorithm!)
            var sortedResult = result.OrderBy(t => t.SortOrder).ToList();
            foreach (var tab in result)
            {
                tab.Sort();
                foreach (var cat in tab.Categories)
                {
                    cat.Sort();
                }
            }
            return sortedResult;
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
        private static void ParseTabAndCategory(PropertyDescriptor descriptor, ref string tabName,
                                                ref string categoryName)
        {
            var ca = AttributeHelper.GetFirstAttribute<CategoryAttribute>(descriptor);
            if (ca == null || ca.Category == null || string.IsNullOrEmpty(ca.Category))
            {
                return;
            }

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

        private void GetOrCreateCategory(Type instanceType, string categoryName, int sortOrder,
                                         TabViewModel currentTabViewModel,
                                         ref CategoryViewModel currentCategoryViewModel)
        {
            if (currentCategoryViewModel == null || currentCategoryViewModel.Name != categoryName)
            {
                currentCategoryViewModel = currentTabViewModel.Categories.FirstOrDefault(c => c.Name == categoryName);
                if (currentCategoryViewModel == null)
                {
                    currentCategoryViewModel = new CategoryViewModel(categoryName, this) {SortOrder = sortOrder};
                    currentTabViewModel.Categories.Add(currentCategoryViewModel);
                    LocalizeCategoryHeader(instanceType, currentCategoryViewModel);
                }
            }
        }

        private void GetOrCreateTab(Type instanceType, ICollection<TabViewModel> tabs, string tabName, int sortOrder,
                                    ref TabViewModel currentTabViewModel, ref CategoryViewModel currentCategoryViewModel)
        {
            if (currentTabViewModel == null || (currentTabViewModel.Name != tabName && ShowTabs))
            {
                currentTabViewModel = tabs.FirstOrDefault(t => t.Name == tabName);

                if (currentTabViewModel == null)
                {
                    // force to find/create a new category as well
                    currentCategoryViewModel = null;
                    currentTabViewModel = CreateTab(tabName);
                    currentTabViewModel.SortOrder = sortOrder;
                    tabs.Add(currentTabViewModel);
                    LocalizeTabHeader(instanceType, currentTabViewModel);
                }
            }
        }

        private TabViewModel CreateTab(string tabName)
        {
            var tab = new TabViewModel(tabName, this);
            if (ImageProvider != null)
            {
                tab.Icon = ImageProvider.GetImage(SelectedObject.GetType(), Name);
            }
            return tab;
        }

        /// <summary>
        /// Updates the property header and tooltip
        /// </summary>
        /// <param name="instanceType">Type of the object being edited</param>
        /// <param name="propertyViewModel">The property viewmodel</param>
        private void LocalizePropertyHeader(Type instanceType, PropertyViewModel propertyViewModel)
        {
            propertyViewModel.Header = GetLocalizedString(instanceType, propertyViewModel.Name);
            propertyViewModel.ToolTip = GetLocalizedTooltip(instanceType, propertyViewModel.Name);

            // [DisplayName(..)] and [Description(...)] attributes overrides the localized strings
            var dna = AttributeHelper.GetFirstAttribute<DisplayNameAttribute>(propertyViewModel.Descriptor);
            var da = AttributeHelper.GetFirstAttribute<DescriptionAttribute>(propertyViewModel.Descriptor);

            if (dna != null)
            {
                propertyViewModel.Header = dna.DisplayName;
            }
            if (da != null)
            {
                propertyViewModel.ToolTip = da.Description;
            }
        }

        /// <summary>
        /// Updates the category (expander/groupbox) header and tooltip
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="categoryViewModel"></param>
        private void LocalizeCategoryHeader(Type instanceType, CategoryViewModel categoryViewModel)
        {
            categoryViewModel.Header = GetLocalizedString(instanceType, categoryViewModel.Name);
            categoryViewModel.ToolTip = GetLocalizedTooltip(instanceType, categoryViewModel.Name);
        }

        /// <summary>
        /// Updates the tab header and tooltip
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="tabViewModel"></param>
        private void LocalizeTabHeader(Type instanceType, TabViewModel tabViewModel)
        {
            tabViewModel.Header = GetLocalizedString(instanceType, tabViewModel.Name);
            tabViewModel.ToolTip = GetLocalizedTooltip(instanceType, tabViewModel.Name);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = sender as PropertyViewModel;
            if (property != null)
            {
                RaisePropertyChangedEvent(property.PropertyName, null, property.Value);
                UpdateOptionalProperties(property);
            }

            if (e.PropertyName != "IsEnabled" && e.PropertyName != "IsVisible" 
                && e.PropertyName != "PropertyError" && e.PropertyName != "PropertyWarning")
            {
                UpdatePropertyStates(SelectedObject);
                UpdateErrorInfo();
            }
        }

        private void UpdatePropertyStates(object instance)
        {
            var psi = instance as IPropertyStateUpdater;
            if (psi == null)
            {
                return;
            }
            var ps = new PropertyStateBag();
            psi.UpdatePropertyStates(ps);
            foreach (var ep in ps.EnabledProperties)
            {
                var p = propertyMap[ep.Key];
                if (p != null && p.IsEnabled != ep.Value)
                {
                    p.IsEnabled = ep.Value;
                }
            }
        }

        private void UpdateErrorInfo()
        {
            foreach (var p in propertyMap.Values)
                p.UpdateErrorInfo();
            if (model!=null)
                foreach (var tab in model)
                    tab.UpdateErrorInfo();
        }

        /// <summary>
        /// Update IsEnabled on properties marked [Optional(..)]
        /// </summary>
        /// <param name="propertyViewModel"></param>
        private void UpdateOptionalProperties(PropertyViewModel propertyViewModel)
        {
            foreach (var prop in propertyMap.Values)
            {
                var oprop = prop as OptionalPropertyViewModel;
                if (oprop == null)
                {
                    continue;
                }
                if (oprop.OptionalPropertyName == propertyViewModel.PropertyName)
                {
                    if (propertyViewModel.Value is bool)
                    {
                        oprop.IsEnabled = (bool) propertyViewModel.Value;
                    }
                }
            }
        }

        private string GetLocalizedString(Type instanceType, string key)
        {
            string result = key;
            if (LocalizationService != null)
            {
                result = LocalizationService.GetString(instanceType, key);
            }
            if (String.IsNullOrEmpty(result))
            {
                result = key;
            }
            return result;
        }

        private object GetLocalizedTooltip(Type instanceType, string key)
        {
            object tooltip = null;
            if (LocalizationService != null)
            {
                tooltip = LocalizationService.GetTooltip(instanceType, key);
            }

            if (tooltip is string)
            {
                var s = (string) tooltip;
                s = s.Trim();
                if (s.Length == 0)
                {
                    tooltip = null;
                }
            }
            return tooltip;
        }
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