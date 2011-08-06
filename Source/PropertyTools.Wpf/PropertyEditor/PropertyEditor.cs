using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PropertyTools.Wpf
{
    /// <summary>
    ///   The show categories as.
    /// </summary>
    public enum ShowCategoriesAs
    {
        /// <summary>
        ///   The group box.
        /// </summary>
        GroupBox,

        /// <summary>
        ///   The expander.
        /// </summary>
        Expander,

        /// <summary>
        ///   The header.
        /// </summary>
        Header
    }

    /// <summary>
    ///   PropertyEditor control.
    ///   Set the SelectedObject to define the contents of the control.
    /// </summary>
    public class PropertyEditor : Control
    {
        /// <summary>
        ///   The categor y_ appearance.
        /// </summary>
        private const string CATEGORY_APPEARANCE = "Appearance";

        /// <summary>
        ///   The par t_ grid.
        /// </summary>
        private const string PART_GRID = "PART_Grid";

        /// <summary>
        ///   The par t_ page.
        /// </summary>
        private const string PART_PAGE = "PART_Page";

        /// <summary>
        ///   The par t_ tabs.
        /// </summary>
        private const string PART_TABS = "PART_Tabs";

        /// <summary>
        ///   The property template selector property.
        /// </summary>
        public static readonly DependencyProperty PropertyTemplateSelectorProperty =
            DependencyProperty.Register("PropertyTemplateSelector", typeof(PropertyTemplateSelector),
                                        typeof(PropertyEditor), new UIPropertyMetadata(null));

        /// <summary>
        ///   The category template selector property.
        /// </summary>
        public static readonly DependencyProperty CategoryTemplateSelectorProperty =
            DependencyProperty.Register("CategoryTemplateSelector", typeof(CategoryTemplateSelector),
                                        typeof(PropertyEditor), new UIPropertyMetadata(null));

        /// <summary>
        ///   The label width property.
        /// </summary>
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(double), typeof(PropertyEditor),
                                        new UIPropertyMetadata(100.0));

        /// <summary>
        ///   The show read only properties property.
        /// </summary>
        public static readonly DependencyProperty ShowReadOnlyPropertiesProperty =
            DependencyProperty.Register("ShowReadOnlyProperties", typeof(bool), typeof(PropertyEditor),
                                        new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        ///   The show tabs property.
        /// </summary>
        public static readonly DependencyProperty ShowTabsProperty =
            DependencyProperty.Register("ShowTabs", typeof(bool), typeof(PropertyEditor),
                                        new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        ///   The declared only property.
        /// </summary>
        public static readonly DependencyProperty DeclaredOnlyProperty =
            DependencyProperty.Register("DeclaredOnly", typeof(bool), typeof(PropertyEditor),
                                        new UIPropertyMetadata(false, AppearanceChanged));

        /// <summary>
        ///   The selected object property.
        /// </summary>
        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register("SelectedObject", typeof(object), typeof(PropertyEditor),
                                        new UIPropertyMetadata(null, SelectedObjectChanged));

        /// <summary>
        ///   The selected objects property.
        /// </summary>
        public static readonly DependencyProperty SelectedObjectsProperty =
            DependencyProperty.Register("SelectedObjects", typeof(IEnumerable), typeof(PropertyEditor),
                                        new UIPropertyMetadata(null, SelectedObjectsChanged));

        /// <summary>
        ///   The show bool header property.
        /// </summary>
        public static readonly DependencyProperty ShowBoolHeaderProperty =
            DependencyProperty.Register("ShowBoolHeader", typeof(bool), typeof(PropertyEditor),
                                        new UIPropertyMetadata(true, AppearanceChanged));

        public static readonly DependencyProperty EnumAsRadioButtonsLimitProperty =
            DependencyProperty.Register("EnumAsRadioButtonsLimit", typeof(int), typeof(PropertyEditor),
                                        new UIPropertyMetadata(4, AppearanceChanged));

        /// <summary>
        /// Gets or sets a value indicating whether to use the default category for uncategorized properties.
        /// When this flag is false, the last defined category will be used.
        /// The default value is false.
        /// </summary>
        public bool UseDefaultCategoryNameForUncategorizedProperties
        {
            get { return (bool)GetValue(UseDefaultCategoryNameForUncategorizedPropertiesProperty); }
            set { SetValue(UseDefaultCategoryNameForUncategorizedPropertiesProperty, value); }
        }

        public static readonly DependencyProperty UseDefaultCategoryNameForUncategorizedPropertiesProperty =
            DependencyProperty.Register("UseDefaultCategoryNameForUncategorizedProperties", typeof(bool), typeof(PropertyEditor), new UIPropertyMetadata(false));


        
        /// <summary>
        ///   The show categories as property.
        /// </summary>
        public static readonly DependencyProperty ShowCategoriesAsProperty =
            DependencyProperty.Register("ShowCategoriesAs", typeof(ShowCategoriesAs), typeof(PropertyEditor),
                                        new UIPropertyMetadata(ShowCategoriesAs.GroupBox, AppearanceChanged));

        /// <summary>
        ///   The default category name property.
        /// </summary>
        public static readonly DependencyProperty DefaultCategoryNameProperty =
            DependencyProperty.Register("DefaultCategoryName", typeof(string), typeof(PropertyEditor),
                                        new UIPropertyMetadata("Properties", AppearanceChanged));

        /// <summary>
        ///   The label alignment property.
        /// </summary>
        public static readonly DependencyProperty LabelAlignmentProperty =
            DependencyProperty.Register("LabelAlignment", typeof(HorizontalAlignment), typeof(PropertyEditor),
                                        new UIPropertyMetadata(HorizontalAlignment.Left, AppearanceChanged));

        /// <summary>
        ///   The localization service property.
        /// </summary>
        public static readonly DependencyProperty LocalizationServiceProperty =
            DependencyProperty.Register("LocalizationService", typeof(ILocalizationService), typeof(PropertyEditor),
                                        new UIPropertyMetadata(null));

        /// <summary>
        ///   The image provider property.
        /// </summary>
        public static readonly DependencyProperty ImageProviderProperty =
            DependencyProperty.Register("ImageProvider", typeof(IImageProvider), typeof(PropertyEditor),
                                        new UIPropertyMetadata(null));

        /// <summary>
        ///   The required attribute property.
        /// </summary>
        public static readonly DependencyProperty RequiredAttributeProperty =
            DependencyProperty.Register("RequiredAttribute", typeof(Type), typeof(PropertyEditor),
                                        new UIPropertyMetadata(null));

        /// <summary>
        ///   The property value changed event.
        /// </summary>
        private static readonly RoutedEvent PropertyValueChangedEvent = EventManager.RegisterRoutedEvent(
            "PropertyValueChanged",
            RoutingStrategy.Bubble,
            typeof(EventHandler<PropertyValueChangedEventArgs>),
            typeof(PropertyEditor));

        /// <summary>
        ///   The default tab name property.
        /// </summary>
        public static readonly DependencyProperty DefaultTabNameProperty =
            DependencyProperty.Register("DefaultTabName", typeof(string), typeof(PropertyEditor),
                                        new UIPropertyMetadata(null, AppearanceChanged));

        /// <summary>
        ///   The error template property.
        /// </summary>
        public static readonly DependencyProperty ErrorTemplateProperty =
            DependencyProperty.Register("ErrorTemplate", typeof(DataTemplate), typeof(PropertyEditor),
                                        new UIPropertyMetadata(null));

        /// <summary>
        ///   The warning template property.
        /// </summary>
        public static readonly DependencyProperty WarningTemplateProperty =
            DependencyProperty.Register("WarningTemplate", typeof(DataTemplate), typeof(PropertyEditor),
                                        new UIPropertyMetadata(null));

        /// <summary>
        ///   The error border thickness property.
        /// </summary>
        public static readonly DependencyProperty ErrorBorderThicknessProperty =
            DependencyProperty.Register("ErrorBorderThickness", typeof(Thickness), typeof(PropertyEditor),
                                        new UIPropertyMetadata(new Thickness(4, 1, 4, 1)));


        /// <summary>
        ///   The property state provider property.
        /// </summary>
        public static readonly DependencyProperty PropertyStateProviderProperty =
            DependencyProperty.Register("PropertyStateProvider", typeof(IPropertyStateProvider),
                                        typeof(PropertyEditor), new UIPropertyMetadata(null));

        /// <summary>
        ///   The PropertyMap dictionary contains a map of all Properties of the current object being edited.
        /// </summary>
        private readonly Dictionary<string, PropertyViewModel> propertyMap;

        /// <summary>
        ///   The content control.
        /// </summary>
        private ContentControl contentControl;

        /// <summary>
        ///   The grid.
        /// </summary>
        private Grid grid;

        /// <summary>
        ///   The model.
        /// </summary>
        private IList<TabViewModel> model;

        /// <summary>
        ///   The property view model factory.
        /// </summary>
        private IPropertyViewModelFactory propertyViewModelFactory;

        /// <summary>
        ///   The tab control.
        /// </summary>
        private TabControl tabControl;

        /// <summary>
        ///   Initializes static members of the <see cref = "PropertyEditor" /> class.
        /// </summary>
        static PropertyEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyEditor),
                                                     new FrameworkPropertyMetadata(typeof(PropertyEditor)));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PropertyEditor" /> class.
        /// </summary>
        public PropertyEditor()
        {
            propertyMap = new Dictionary<string, PropertyViewModel>();

            PropertyTemplateSelector = new PropertyTemplateSelector(this);
            CategoryTemplateSelector = new CategoryTemplateSelector(this);
        }

        /// <summary>
        ///   Gets or sets the maximum number of enum values that can be shown using radio buttons.
        ///   If the value is 0, Enums will always be shown as ComboBoxes.
        ///   If the value is infinity, Enums will always be shown as Radiobuttons.
        /// </summary>
        /// <value>The limit.</value>
        [Category(CATEGORY_APPEARANCE)]
        public int EnumAsRadioButtonsLimit
        {
            get { return (int)GetValue(EnumAsRadioButtonsLimitProperty); }
            set { SetValue(EnumAsRadioButtonsLimitProperty, value); }
        }

        /// <summary>
        ///   Gets or sets PropertyStateProvider.
        /// </summary>
        public IPropertyStateProvider PropertyStateProvider
        {
            get { return (IPropertyStateProvider)GetValue(PropertyStateProviderProperty); }
            set { SetValue(PropertyStateProviderProperty, value); }
        }

        /// <summary>
        ///   Gets or sets ErrorBorderThickness.
        /// </summary>
        public Thickness ErrorBorderThickness
        {
            get { return (Thickness)GetValue(ErrorBorderThicknessProperty); }
            set { SetValue(ErrorBorderThicknessProperty, value); }
        }

        /// <summary>
        ///   Gets or sets SelectedObjects.
        /// </summary>
        public IEnumerable SelectedObjects
        {
            get { return (IEnumerable)GetValue(SelectedObjectsProperty); }
            set { SetValue(SelectedObjectsProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the property view model factory.
        ///   This factory is used to generate the view model based on the property descriptors.
        ///   You can override this factory to create the view model based on your own attributes.
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
        ///   Collection of custom editors
        /// </summary>
        [Browsable(false)]
        public Collection<TypeEditor> Editors
        {
            // the collection is stored in the propertyTemplateSelector
            get { return PropertyTemplateSelector.Editors; }
        }

        /// <summary>
        ///   The width of the property labels
        /// </summary>
        [Category(CATEGORY_APPEARANCE)]
        public double LabelWidth
        {
            get { return (double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

        /// <summary>
        ///   Show read-only properties.
        /// </summary>
        [Category(CATEGORY_APPEARANCE)]
        public bool ShowReadOnlyProperties
        {
            get { return (bool)GetValue(ShowReadOnlyPropertiesProperty); }
            set { SetValue(ShowReadOnlyPropertiesProperty, value); }
        }

        /// <summary>
        ///   Organize the properties in tabs.
        ///   You should use the [Category("Tabname|Groupname")] attribute to define the tabs.
        /// </summary>
        [Category(CATEGORY_APPEARANCE)]
        public bool ShowTabs
        {
            get { return (bool)GetValue(ShowTabsProperty); }
            set { SetValue(ShowTabsProperty, value); }
        }

        /// <summary>
        ///   Show only declared properties (not inherited properties).
        ///   Specifies that only members declared at the level of the supplied type's hierarchy 
        ///   should be considered. Inherited members are not considered.
        /// </summary>
        [Category(CATEGORY_APPEARANCE)]
        public bool DeclaredOnly
        {
            get { return (bool)GetValue(DeclaredOnlyProperty); }
            set { SetValue(DeclaredOnlyProperty, value); }
        }

        /// <summary>
        ///   Gets or sets SelectedObject.
        /// </summary>
        [Browsable(false)]
        public object SelectedObject
        {
            get { return GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        /// <summary>
        ///   Show enum properties as ComboBox or RadioButtonList.
        /// </summary>
        [Category(CATEGORY_APPEARANCE)]
        public bool ShowBoolHeader
        {
            get { return (bool)GetValue(ShowBoolHeaderProperty); }
            set { SetValue(ShowBoolHeaderProperty, value); }
        }

        /// <summary>
        ///   Gets or sets ShowCategoriesAs.
        /// </summary>
        [Category(CATEGORY_APPEARANCE)]
        public ShowCategoriesAs ShowCategoriesAs
        {
            get { return (ShowCategoriesAs)GetValue(ShowCategoriesAsProperty); }
            set { SetValue(ShowCategoriesAsProperty, value); }
        }

        /// <summary>
        ///   Gets or sets DefaultTabName.
        /// </summary>
        public string DefaultTabName
        {
            get { return (string)GetValue(DefaultTabNameProperty); }
            set { SetValue(DefaultTabNameProperty, value); }
        }


        /// <summary>
        ///   Gets or sets DefaultCategoryName.
        /// </summary>
        public string DefaultCategoryName
        {
            get { return (string)GetValue(DefaultCategoryNameProperty); }
            set { SetValue(DefaultCategoryNameProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the alignment of property labels.
        /// </summary>
        public HorizontalAlignment LabelAlignment
        {
            get { return (HorizontalAlignment)GetValue(LabelAlignmentProperty); }
            set { SetValue(LabelAlignmentProperty, value); }
        }

        /// <summary>
        ///   Implement the LocalizationService to translate the tab, category and property strings and tooltips
        /// </summary>
        [Browsable(false)]
        public ILocalizationService LocalizationService
        {
            get { return (ILocalizationService)GetValue(LocalizationServiceProperty); }
            set { SetValue(LocalizationServiceProperty, value); }
        }

        /// <summary>
        ///   The ImageProvider can be used to provide images to the Tab icons.
        /// </summary>
        [Browsable(false)]
        public IImageProvider ImageProvider
        {
            get { return (IImageProvider)GetValue(ImageProviderProperty); }
            set { SetValue(ImageProviderProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the required attribute type.
        ///   If the required attribute type is set, only properties where this attribute is set will be shown.
        /// </summary>
        [Browsable(false)]
        public Type RequiredAttribute
        {
            get { return (Type)GetValue(RequiredAttributeProperty); }
            set { SetValue(RequiredAttributeProperty, value); }
        }

        /// <summary>
        ///   The PropertyTemplateSelector is used to select the DataTemplate for each PropertyViewModel
        /// </summary>
        [Browsable(false)]
        public PropertyTemplateSelector PropertyTemplateSelector
        {
            get { return (PropertyTemplateSelector)GetValue(PropertyTemplateSelectorProperty); }
            set { SetValue(PropertyTemplateSelectorProperty, value); }
        }

        /// <summary>
        ///   The CategoryTemplateSelector is used to select the DataTemplate for the CategoryViewModels
        /// </summary>
        [Browsable(false)]
        public CategoryTemplateSelector CategoryTemplateSelector
        {
            get { return (CategoryTemplateSelector)GetValue(CategoryTemplateSelectorProperty); }
            set { SetValue(CategoryTemplateSelectorProperty, value); }
        }

        /// <summary>
        ///   Gets or sets ErrorTemplate.
        /// </summary>
        public DataTemplate ErrorTemplate
        {
            get { return (DataTemplate)GetValue(ErrorTemplateProperty); }
            set { SetValue(ErrorTemplateProperty, value); }
        }

        /// <summary>
        ///   Gets or sets WarningTemplate.
        /// </summary>
        public DataTemplate WarningTemplate
        {
            get { return (DataTemplate)GetValue(WarningTemplateProperty); }
            set { SetValue(WarningTemplateProperty, value); }
        }

        /// <summary>
        ///   Gets rortemplate.
        /// </summary>
        public static RoutedEvent rortemplate
        {
            get { return PropertyValueChangedEvent; }
        }

        public IFileDialogService FileDialogService { get; set; }

        public IFolderBrowserDialogService FolderBrowserDialogService { get; set; }

        /// <summary>
        ///   The on apply template.
        /// </summary>
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

        /// <summary>
        ///   The property editor_ loaded.
        /// </summary>
        /// <param name = "sender">
        ///   The sender.
        /// </param>
        /// <param name = "e">
        ///   The e.
        /// </param>
        private void PropertyEditor_Loaded(object sender, RoutedEventArgs e)
        {
            // Update the content of the control
            UpdateContent();
        }

        /// <summary>
        ///   The property editor_ unloaded.
        /// </summary>
        /// <param name = "sender">
        ///   The sender.
        /// </param>
        /// <param name = "e">
        ///   The e.
        /// </param>
        private void PropertyEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            // Unsubscribe all property change event handlers
            ClearModel();
        }

        /// <summary>
        ///   The selected object changed.
        /// </summary>
        /// <param name = "d">
        ///   The d.
        /// </param>
        /// <param name = "e">
        ///   The e.
        /// </param>
        private static void SelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pe = (PropertyEditor)d;
            pe.UpdateContent();
        }

        /// <summary>
        ///   The selected objects changed.
        /// </summary>
        /// <param name = "d">
        ///   The d.
        /// </param>
        /// <param name = "e">
        ///   The e.
        /// </param>
        private static void SelectedObjectsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pe = (PropertyEditor)d;
            pe.UpdateContent();
        }

        /// <summary>
        ///   The property changed.
        /// </summary>
        public event EventHandler<PropertyValueChangedEventArgs> PropertyChanged
        {
            add { AddHandler(PropertyValueChangedEvent, value); }
            remove { RemoveHandler(PropertyValueChangedEvent, value); }
        }

        /// <summary>
        ///   Invoke this method to raise a PropertyChanged event.
        ///   This event only makes sense when editing single objects (not IEnumerables).
        /// </summary>
        /// <param name = "propertyName">
        ///   The property Name.
        /// </param>
        /// <param name = "oldValue">
        ///   The old Value.
        /// </param>
        /// <param name = "newValue">
        ///   The new Value.
        /// </param>
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

        /// <summary>
        ///   The appearance changed.
        /// </summary>
        /// <param name = "d">
        ///   The d.
        /// </param>
        /// <param name = "e">
        ///   The e.
        /// </param>
        private static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyEditor)d).UpdateContent();
        }

        /// <summary>
        ///   Updates the content of the control
        ///   (after initialization and SelectedObject changes)
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
                var tab = model != null && model.Count > 0 ? model[0] : null;
                contentControl.Content = tab;
                tabControl.Visibility = Visibility.Collapsed;
                contentControl.Visibility = Visibility.Visible;
            }

            UpdatePropertyStates(SelectedObject);
            UpdateErrorInfo();
        }

        /// <summary>
        ///   The clear model.
        /// </summary>
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
        ///   This method takes an object Instance and creates the property model.
        ///   The properties are organized in a hierarchy
        ///   PropertyTab
        ///   PropertyCategory
        ///   Property|OptionalProperty|WideProperty|CheckBoxProperty
        /// </summary>
        /// <param name = "instance">
        /// </param>
        /// <param name = "isEnumerable">
        /// </param>
        /// <returns>
        ///   Collection of tab ViewModels
        /// </returns>
        public virtual IList<TabViewModel> CreatePropertyModel(object instance, bool isEnumerable)
        {
            if (instance == null)
            {
                return null;
            }

            // find the instance type
            var instanceType = isEnumerable
                                   ? TypeHelper.FindBiggestCommonType(instance as IEnumerable)
                                   : instance.GetType();

            if (instanceType == null)
            {
                return null;
            }

            // find all properties of the instance type
            var properties = isEnumerable
                                 ? TypeDescriptor.GetProperties(instanceType)
                                 : TypeDescriptor.GetProperties(instance);

            // The GetPropertyModel method does not return properties in a particular order, 
            // such as alphabetical or declaration order. Your code must not depend on the 
            // order in which properties are returned, because that order varies.
            TabViewModel currentTabViewModel = null;
            CategoryViewModel currentCategoryViewModel = null;
            Type currentComponentType = null;

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

                if (descriptor.ComponentType != currentComponentType)
                {
                    categoryName = DefaultCategoryName;
                    tabName = DefaultTabName ?? descriptor.ComponentType.Name;
                    currentComponentType = descriptor.ComponentType;
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

				// The default value for an Enum-property is the first enum in the enumeration.
				// If the first value happens to be filtered due to the attribute [Browsable(false)],
				// the WPF-binding system ends up in an infinite loop when updating the bound value
				// due to a PropertyChanged-call. We must therefore make sure that the initially selected
				// value is one of the allowed values from the filtered enumeration.
				if( descriptor.PropertyType.BaseType == typeof( Enum ) ) {
					List<object> validEnumValues = Enum.GetValues( descriptor.PropertyType ).FilterOnBrowsableAttribute();
					// Check if the enumeration that has all values hidden before accessing the first item.
					if( validEnumValues.Count > 0 && !validEnumValues.Contains( descriptor.GetValue( instance ) ) ) {
						descriptor.SetValue( instance, validEnumValues[0] );
					}
				}

                // Create Property ViewModel
                var propertyViewModel = PropertyViewModelFactory.CreateViewModel(instance, descriptor);
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

                bool categoryFound = ParseTabAndCategory(descriptor, ref tabName, ref categoryName);

                if (!categoryFound && UseDefaultCategoryNameForUncategorizedProperties)
                {
                    categoryName = DefaultCategoryName;
                    tabName = DefaultTabName ?? descriptor.ComponentType.Name;
                }

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

        /// <summary>
        ///   The check optional properties.
        /// </summary>
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

        /// <summary>
        ///   The sort property model.
        /// </summary>
        /// <param name = "result">
        ///   The result.
        /// </param>
        /// <returns>
        /// </returns>
        private List<TabViewModel> SortPropertyModel(List<TabViewModel> result)
        {
            // Use LINQ to stable sort tabs, categories and properties.
            // (important that it is a stable sort algorithm!)
            var sortedResult = result.OrderBy(t => t.SortOrder).ToList();
            foreach (TabViewModel tab in result)
            {
                tab.Sort();
                foreach (CategoryViewModel cat in tab.Categories)
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
        /// If the CategoryAttribute is
        /// [Category("GroupC")]
        /// the method will not change tabName, but set categoryName="GroupC"
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <param name="tabName">Name of the tab.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>true if the descriptor contained a CategoryAttribute</returns>
        private static bool ParseTabAndCategory(PropertyDescriptor descriptor, ref string tabName,
                                                ref string categoryName)
        {
            var ca = AttributeHelper.GetFirstAttribute<CategoryAttribute>(descriptor);

            if (ca == null || ca.Category == null || string.IsNullOrEmpty(ca.Category))
            {
                return false;
            }

            var items = ca.Category.Split('|');
            if (items.Length == 2)
            {
                tabName = items[0];
                categoryName = items[1];
            }

            if (items.Length == 1)
            {
                categoryName = items[0];
            }
            return true;
        }

        /// <summary>
        ///   The get or create category.
        /// </summary>
        /// <param name = "instanceType">
        ///   The instance type.
        /// </param>
        /// <param name = "categoryName">
        ///   The category name.
        /// </param>
        /// <param name = "sortOrder">
        ///   The sort order.
        /// </param>
        /// <param name = "currentTabViewModel">
        ///   The current tab view model.
        /// </param>
        /// <param name = "currentCategoryViewModel">
        ///   The current category view model.
        /// </param>
        private void GetOrCreateCategory(Type instanceType, string categoryName, int sortOrder,
                                         TabViewModel currentTabViewModel,
                                         ref CategoryViewModel currentCategoryViewModel)
        {
            if (currentCategoryViewModel == null || currentCategoryViewModel.Name != categoryName)
            {
                currentCategoryViewModel = currentTabViewModel.Categories.FirstOrDefault(c => c.Name == categoryName);
                if (currentCategoryViewModel == null)
                {
                    currentCategoryViewModel = new CategoryViewModel(categoryName, this) { SortOrder = sortOrder };
                    currentTabViewModel.Categories.Add(currentCategoryViewModel);
                    LocalizeCategoryHeader(instanceType, currentCategoryViewModel);
                }
            }
        }

        /// <summary>
        ///   The get or create tab.
        /// </summary>
        /// <param name = "instanceType">
        ///   The instance type.
        /// </param>
        /// <param name = "tabs">
        ///   The tabs.
        /// </param>
        /// <param name = "tabName">
        ///   The tab name.
        /// </param>
        /// <param name = "sortOrder">
        ///   The sort order.
        /// </param>
        /// <param name = "currentTabViewModel">
        ///   The current tab view model.
        /// </param>
        /// <param name = "currentCategoryViewModel">
        ///   The current category view model.
        /// </param>
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

        /// <summary>
        ///   The create tab.
        /// </summary>
        /// <param name = "tabName">
        ///   The tab name.
        /// </param>
        /// <returns>
        /// </returns>
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
        ///   Updates the property header and tooltip
        /// </summary>
        /// <param name = "instanceType">
        ///   Type of the object being edited
        /// </param>
        /// <param name = "propertyViewModel">
        ///   The property viewmodel
        /// </param>
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
        ///   Updates the category (expander/groupbox) header and tooltip
        /// </summary>
        /// <param name = "instanceType">
        /// </param>
        /// <param name = "categoryViewModel">
        /// </param>
        private void LocalizeCategoryHeader(Type instanceType, CategoryViewModel categoryViewModel)
        {
            categoryViewModel.Header = GetLocalizedString(instanceType, categoryViewModel.Name);
            categoryViewModel.ToolTip = GetLocalizedTooltip(instanceType, categoryViewModel.Name);
        }

        /// <summary>
        ///   Updates the tab header and tooltip
        /// </summary>
        /// <param name = "instanceType">
        /// </param>
        /// <param name = "tabViewModel">
        /// </param>
        private void LocalizeTabHeader(Type instanceType, TabViewModel tabViewModel)
        {
            tabViewModel.Header = GetLocalizedString(instanceType, tabViewModel.Name);
            tabViewModel.ToolTip = GetLocalizedTooltip(instanceType, tabViewModel.Name);
        }

        /// <summary>
        ///   The OnPropertyChanged handler.
        /// </summary>
        /// <param name = "sender">
        ///   The sender.
        /// </param>
        /// <param name = "e">
        ///   The event arguments.
        /// </param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = sender as PropertyViewModel;
            if (property != null && e.PropertyName == "Value")
            {
                RaisePropertyChangedEvent(property.PropertyName, property.OldValue, property.Value);
                UpdateOptionalProperties(property);
            }

            if (e.PropertyName != "IsEnabled" && e.PropertyName != "IsVisible"
                && e.PropertyName != "PropertyError" && e.PropertyName != "PropertyWarning")
            {
                UpdatePropertyStates(SelectedObject);
                UpdateErrorInfo();
            }
        }

        /// <summary>
        ///   The UpdatePropertyStates method updates the properties IsEnabled when the instance being edited implements IPropertyStateUpdater.
        /// </summary>
        /// <param name = "instance">
        ///   The instance.
        /// </param>
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

        /// <summary>
        ///   The update error info.
        /// </summary>
        private void UpdateErrorInfo()
        {
            foreach (PropertyViewModel p in propertyMap.Values)
            {
                p.UpdateErrorInfo();
            }

            if (model != null)
            {
                foreach (TabViewModel tab in model)
                {
                    tab.UpdateErrorInfo();
                }
            }
        }

        /// <summary>
        ///   Update IsEnabled on properties marked [Optional(..)]
        /// </summary>
        /// <param name = "propertyViewModel">
        /// </param>
        private void UpdateOptionalProperties(PropertyViewModel propertyViewModel)
        {
            foreach (PropertyViewModel prop in propertyMap.Values)
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
                        oprop.IsEnabled = (bool)propertyViewModel.Value;
                    }
                }
            }
        }

        /// <summary>
        ///   The get localized string.
        /// </summary>
        /// <param name = "instanceType">
        ///   The instance type.
        /// </param>
        /// <param name = "key">
        ///   The key.
        /// </param>
        /// <returns>
        ///   The get localized string.
        /// </returns>
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

        /// <summary>
        ///   The get localized tooltip.
        /// </summary>
        /// <param name = "instanceType">
        ///   The instance type.
        /// </param>
        /// <param name = "key">
        ///   The key.
        /// </param>
        /// <returns>
        ///   The get localized tooltip.
        /// </returns>
        private object GetLocalizedTooltip(Type instanceType, string key)
        {
            object tooltip = null;
            if (LocalizationService != null)
            {
                tooltip = LocalizationService.GetTooltip(instanceType, key);
            }

            if (tooltip is string)
            {
                var s = (string)tooltip;
                s = s.Trim();
                if (s.Length == 0)
                {
                    tooltip = null;
                }
            }

            return tooltip;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Enter)
            {
                var textBox = e.OriginalSource as TextBox;
                if (textBox != null)
                {
                    if (textBox.AcceptsReturn)
                    {
                        return;
                    }

                    var bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
                    if (bindingExpression != null && bindingExpression.Status == System.Windows.Data.BindingStatus.Active)
                    {
                        bindingExpression.UpdateSource();
                        
                        textBox.CaretIndex = textBox.Text.Length;
                        textBox.SelectAll();
                    }
                }
            }
        }

    }

    /// <summary>
    ///   Event args for the PropertyValueChanged event
    /// </summary>
    public class PropertyValueChangedEventArgs : RoutedEventArgs
    {
        /// <summary>
        ///   Gets or sets PropertyName.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        ///   Gets or sets OldValue.
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        ///   Gets or sets NewValue.
        /// </summary>
        public object NewValue { get; set; }
    }
}