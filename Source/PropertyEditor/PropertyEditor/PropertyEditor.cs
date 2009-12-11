using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace OpenControls
{
    /// <summary>
    /// PropertyEditor control
    /// Set the SelectedObject control to define the contents
    /// TODO: support SelectedObjects - multiple objects
    /// </summary>
    public class PropertyEditor : Control
    {
        #region Custom control initialization

        private const string PartGrid = "PART_Grid";
        private const string PartPage = "PART_Page";
        private const string PartTabs = "PART_Tabs";

        private Grid _grid;
        private ContentControl _page;
        private TabControl _tabs;

        static PropertyEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (PropertyEditor),
                                                     new FrameworkPropertyMetadata(typeof (PropertyEditor)));
        }

        public override void OnApplyTemplate()
        {
            if (_tabs == null)
            {
                _tabs = Template.FindName(PartTabs, this) as TabControl;
            }
            if (_page == null)
            {
                _page = Template.FindName(PartPage, this) as ContentControl;
            }
            if (_grid == null)
            {
                _grid = Template.FindName(PartGrid, this) as Grid;
            }

            PropertyTemplateSelector = new PropertyTemplateSelector();
            PropertyTemplateSelector.TemplateOwner = _grid;

            // now update the control
            UpdateContent();
        }

        #endregion

        #region Properties

        #region CaptionWidth

        public static readonly DependencyProperty CaptionWidthProperty =
            DependencyProperty.Register("CaptionWidth", typeof (double), typeof (PropertyEditor),
                                        new UIPropertyMetadata(100.0));

        public double CaptionWidth
        {
            get { return (double) GetValue(CaptionWidthProperty); }
            set { SetValue(CaptionWidthProperty, value); }
        }

        #endregion

        #region ShowReadOnlyProperties

        public static readonly DependencyProperty ShowReadOnlyPropertiesProperty =
            DependencyProperty.Register("ShowReadOnlyProperties", typeof (bool), typeof (PropertyEditor),
                                        new UIPropertyMetadata(true, ShowReadOnlyPropertiesChanged));

        public bool ShowReadOnlyProperties
        {
            get { return (bool) GetValue(ShowReadOnlyPropertiesProperty); }
            set { SetValue(ShowReadOnlyPropertiesProperty, value); }
        }

        private static void ShowReadOnlyPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyEditor) d).UpdateContent();
        }

        #endregion

        #region UseTabs

        public static readonly DependencyProperty UseTabsProperty =
            DependencyProperty.Register("UseTabs", typeof (bool), typeof (PropertyEditor),
                                        new UIPropertyMetadata(true, UseTabsChanged));

        public bool UseTabs
        {
            get { return (bool) GetValue(UseTabsProperty); }
            set { SetValue(UseTabsProperty, value); }
        }

        private static void UseTabsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyEditor) d).UpdateContent();
        }

        #endregion

        #region DeclaredOnly

        public static readonly DependencyProperty DeclaredOnlyProperty =
            DependencyProperty.Register("DeclaredOnly", typeof (bool), typeof (PropertyEditor),
                                        new UIPropertyMetadata(false));

        public bool DeclaredOnly
        {
            get { return (bool) GetValue(DeclaredOnlyProperty); }
            set { SetValue(DeclaredOnlyProperty, value); }
        }

        #endregion

        #region SelectedObject

        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register("SelectedObject", typeof (object), typeof (PropertyEditor),
                                        new UIPropertyMetadata(null, SelectedObjectChanged));

        public object SelectedObject
        {
            get { return DataContext; }
            set { DataContext = value; }
        }

        private static void SelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pe = (PropertyEditor) d;
            pe.DataContext = e.NewValue;
        }

        #endregion



        public bool EnumAsComboBox
        {
            get { return (bool)GetValue(EnumAsComboBoxProperty); }
            set { SetValue(EnumAsComboBoxProperty, value); }
        }

        public static readonly DependencyProperty EnumAsComboBoxProperty =
            DependencyProperty.Register("ShowEnumAsComboBox", typeof(bool), typeof(PropertyEditor), new UIPropertyMetadata(true));



        #region CaptionAlignment

        public static readonly DependencyProperty CaptionAlignmentProperty =
            DependencyProperty.Register("CaptionAlignment", typeof (HorizontalAlignment), typeof (PropertyEditor),
                                        new UIPropertyMetadata(HorizontalAlignment.Left));

        public HorizontalAlignment CaptionAlignment
        {
            get { return (HorizontalAlignment) GetValue(CaptionAlignmentProperty); }
            set { SetValue(CaptionAlignmentProperty, value); }
        }

        #endregion

        #endregion

        #region Events

        #region GetPropertyDescription

        #region Delegates

        public delegate void GetPropertyDescriptionRoutedEventHandler(object sender, PropertyDescriptionArgs e);

        #endregion

        public static readonly RoutedEvent GetPropertyDescriptionEvent =
            EventManager.RegisterRoutedEvent("GetPropertyDescription", RoutingStrategy.Bubble,
                                             typeof (GetPropertyDescriptionRoutedEventHandler), typeof (PropertyEditor));

        /// <summary>
        /// The GetPropertyDescription can be used to localize the property description strings. 
        /// The event handler can e.g. use the ResourceManager to return the localized strings.
        /// </summary>
        public event GetPropertyDescriptionRoutedEventHandler GetPropertyDescription
        {
            add { AddHandler(GetPropertyDescriptionEvent, value); }
            remove { RemoveHandler(GetPropertyDescriptionEvent, value); }
        }

        protected virtual PropertyDescriptionArgs RaiseGetPropertyDescription(string propertyName)
        {
            var args = new PropertyDescriptionArgs {PropertyName = propertyName};
            args.RoutedEvent = GetPropertyDescriptionEvent;
            RaiseEvent(args);
            return args;
        }

        #endregion

        #region PropertyValueChanged event

        #region Delegates

        public delegate void PropertyValueChangedEventHandler(object sender, PropertyValueChangedEventArgs e);

        #endregion

        public static readonly RoutedEvent PropertyValueChangedEvent = EventManager.RegisterRoutedEvent(
            "PropertyValueChanged",
            RoutingStrategy.Bubble,
            typeof (PropertyValueChangedEventHandler),
            typeof (PropertyEditor));

        public event PropertyValueChangedEventHandler PropertyChanged
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

        #endregion

        public PropertyEditor()
        {
            DataContextChanged += OnDataContextChanged;
        }

        private PropertyTemplateSelector PropertyTemplateSelector { get; set; }

        #region Editors

        public Collection<TypeEditor> Editors
        {
            get { return PropertyTemplateSelector.Editors; }
        }

        #endregion

        private Dictionary<string, Property> propertyMap = null;

        public void UpdateContent()
        {
            if (_tabs==null || _page==null)
                return;

            var propertyTabs = GetProperties(DataContext);
            if (UseTabs)
            {
                _tabs.ItemsSource = propertyTabs;
                if (_tabs.Items.Count > 0)
                    _tabs.SelectedIndex = 0;
                _tabs.Visibility = Visibility.Visible;
                _page.Visibility = Visibility.Collapsed;
            }
            else
            {
                var tab = propertyTabs.Count > 0 ? propertyTabs[0] : null;
                _page.Content = tab;
                _tabs.Visibility = Visibility.Collapsed;
                _page.Visibility = Visibility.Visible;
            }
            UpdatePropertyStates();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateContent();
        }

        public virtual Collection<PropertyTab> GetProperties(object instance)
        {
            var tabs = new Collection<PropertyTab>();
            if (instance == null)
                return tabs;

            Type t = instance.GetType();

            // find the DeclaredOnly propertyTabs
            PropertyInfo[] declaredOnlyProperties =
                t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(instance);

            PropertyTab currentTab = null;
            PropertyCategory currentCategory = null;

            string category = null;

            // Use the type name of the instance as the default tab name
            string tab = instance.GetType().Name;
            
            PropertyTemplateSelector.ShowEnumAsComboBox = EnumAsComboBox;

            propertyMap=new Dictionary<string, Property>();

            foreach (PropertyDescriptor descriptor in properties)
            {
                if (DeclaredOnly)
                {
                    // check if the property is in the DeclaredOnlyList
                    bool ok = false;
                    foreach (PropertyInfo pi in declaredOnlyProperties)
                        if (pi.Name == descriptor.Name)
                        {
                            ok = true;
                            break;
                        }
                    if (!ok)
                        continue;
                }
                if (!descriptor.IsBrowsable)
                    continue;
                if (!ShowReadOnlyProperties && descriptor.IsReadOnly)
                    continue;

                ParseTabAndCategory(descriptor.Category, ref tab, ref category);
                if (category == "Misc") // "Misc" is returned when category is not defined...
                {
                    category = currentCategory != null ? currentCategory.Name : category;
                }

                if (currentTab == null || (currentTab.Name != tab && UseTabs))
                {
                    currentTab = new PropertyTab {Name = tab, Header = tab};
                    currentCategory = null;
                    UpdateProperty(currentTab, tab);
                    tabs.Add(currentTab);
                }
                if (currentCategory == null || currentCategory.Name != category)
                {
                    currentCategory = new PropertyCategory {Name = category, Header = category};
                    UpdateProperty(currentCategory, category);
                    currentTab.Categories.Add(currentCategory);
                }
                
                var property = new Property(instance, descriptor, PropertyTemplateSelector);               
                propertyMap.Add(descriptor.Name,property);

                property.PropertyChanged += OnPropertyChanged;
                UpdateProperty(property, descriptor.Name);
                currentCategory.Properties.Add(property);
            }
            return tabs;
        }

        private void UpdatePropertyStates()
        {
            UpdatePropertyStates(DataContext);    
        }

        private void UpdatePropertyStates(object instance)
        {
            var psi = instance as IPropertyState;
            if (psi==null)
                return;
            var ps = new PropertyState();
            psi.GetPropertyStates(ps);
            foreach (var ep in ps.EnabledProperties)
            {
                var p = propertyMap[ep.Key];
                if (p.IsEnabled!=ep.Value)
                    p.IsEnabled = ep.Value;
            }
            
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // todo
            Debug.IndentLevel++;
            Debug.WriteLine("PropertyEditor.OnPropertyChanged");
            var property = sender as Property;
            if (property != null)
                RaisePropertyChangedEvent(property.PropertyName, null, property.Value);
            Debug.IndentLevel--;
            if (e.PropertyName!="IsEnabled")
                UpdatePropertyStates();
        }

        private void UpdateProperty(PropertyBase property, string name)
        {
            PropertyDescriptionArgs a = RaiseGetPropertyDescription(name);
            if (a != null)
            {
                if (a.Header != null)
                    property.Header = a.Header;
                if (a.Tooltip != null)
                    property.ToolTip = a.Tooltip;
            }
            if (property.ToolTip is string)
            {
                var s = (string) property.ToolTip;
                s = s.Trim();
                if (s.Length == 0)
                    property.ToolTip = null;
            }
        }

        private void ParseTabAndCategory(string p, ref string tab, ref string category)
        {
            if (p == null)
                return;
            string[] items = p.Split('|');
            if (items.Length == 2)
            {
                tab = items[0];
                category = items[1];
            }
            if (items.Length == 1)
            {
                category = items[0];
            }
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

    /// <summary>
    /// Event args for the GetPropertyDescription event
    /// </summary>
    public class PropertyDescriptionArgs : RoutedEventArgs
    {
        public string PropertyName { get; set; }
        public string Header { get; set; }
        public object Tooltip { get; set; }
    }
}