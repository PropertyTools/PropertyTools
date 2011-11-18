// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyControl.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// The property control.
    /// </summary>
    [TemplatePart(Name = PartTabs, Type = typeof(TabControl))]
    [TemplatePart(Name = PartPanel, Type = typeof(StackPanel))]
    [TemplatePart(Name = PartScroller, Type = typeof(ScrollViewer))]
    public class PropertyControl : Control
    {
        #region Constants and Fields

        /// <summary>
        ///   The actual label width property.
        /// </summary>
        public static readonly DependencyProperty ActualLabelWidthProperty =
            DependencyProperty.Register(
                "ActualLabelWidth", typeof(double), typeof(PropertyControl), new UIPropertyMetadata(0.0));

        /// <summary>
        ///   The category control template property.
        /// </summary>
        public static readonly DependencyProperty CategoryControlTemplateProperty =
            DependencyProperty.Register(
                "CategoryControlTemplate",
                typeof(ControlTemplate),
                typeof(PropertyControl),
                new UIPropertyMetadata(null));

        /// <summary>
        ///   The category control type property.
        /// </summary>
        public static readonly DependencyProperty CategoryControlTypeProperty =
            DependencyProperty.Register(
                "CategoryControlType",
                typeof(CategoryControlType),
                typeof(PropertyControl),
                new UIPropertyMetadata(CategoryControlType.GroupBox, AppearanceChanged));

        /// <summary>
        ///   The category header template property.
        /// </summary>
        public static readonly DependencyProperty CategoryHeaderTemplateProperty =
            DependencyProperty.Register("CategoryHeaderTemplate", typeof(DataTemplate), typeof(PropertyControl));

        /// <summary>
        ///   The description icon alignment property.
        /// </summary>
        public static readonly DependencyProperty DescriptionIconAlignmentProperty =
            DependencyProperty.Register(
                "DescriptionIconAlignment",
                typeof(HorizontalAlignment),
                typeof(PropertyControl),
                new UIPropertyMetadata(HorizontalAlignment.Right, AppearanceChanged));

        /// <summary>
        ///   The description icon property.
        /// </summary>
        public static readonly DependencyProperty DescriptionIconProperty =
            DependencyProperty.Register("DescriptionIcon", typeof(ImageSource), typeof(PropertyControl));

        /// <summary>
        ///   The enum as radio buttons limit property.
        /// </summary>
        public static readonly DependencyProperty EnumAsRadioButtonsLimitProperty =
            DependencyProperty.Register(
                "EnumAsRadioButtonsLimit",
                typeof(int),
                typeof(PropertyControl),
                new UIPropertyMetadata(4, AppearanceChanged));

        /// <summary>
        ///   The minimum label width property.
        /// </summary>
        public static readonly DependencyProperty MinimumLabelWidthProperty =
            DependencyProperty.Register(
                "MinimumLabelWidth", typeof(double), typeof(PropertyControl), new UIPropertyMetadata(70.0));

        /// <summary>
        ///   The property control factory property.
        /// </summary>
        public static readonly DependencyProperty PropertyControlFactoryProperty =
            DependencyProperty.Register(
                "PropertyControlFactory",
                typeof(IPropertyControlFactory),
                typeof(PropertyControl),
                new UIPropertyMetadata(null));

        /// <summary>
        ///   The property item factory property.
        /// </summary>
        public static readonly DependencyProperty PropertyItemFactoryProperty =
            DependencyProperty.Register("PropertyItemFactory", typeof(IPropertyItemFactory), typeof(PropertyControl));

        /// <summary>
        ///   The required attribute property.
        /// </summary>
        public static readonly DependencyProperty RequiredAttributeProperty =
            DependencyProperty.Register(
                "RequiredAttribute",
                typeof(Type),
                typeof(PropertyControl),
                new UIPropertyMetadata(null, AppearanceChanged));

        /// <summary>
        ///   The selected object property.
        /// </summary>
        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
            "SelectedObject",
            typeof(object),
            typeof(PropertyControl),
            new UIPropertyMetadata(null, SelectedObjectChanged));

        /// <summary>
        ///   The show check box headers property.
        /// </summary>
        public static readonly DependencyProperty ShowCheckBoxHeadersProperty =
            DependencyProperty.Register(
                "ShowCheckBoxHeaders",
                typeof(bool),
                typeof(PropertyControl),
                new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        ///   The show declared only property.
        /// </summary>
        public static readonly DependencyProperty ShowDeclaredOnlyProperty =
            DependencyProperty.Register(
                "ShowDeclaredOnly",
                typeof(bool),
                typeof(PropertyControl),
                new UIPropertyMetadata(false, AppearanceChanged));

        /// <summary>
        ///   The show description icons property.
        /// </summary>
        public static readonly DependencyProperty ShowDescriptionIconsProperty =
            DependencyProperty.Register(
                "ShowDescriptionIcons",
                typeof(bool),
                typeof(PropertyControl),
                new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        ///   The show read only properties property.
        /// </summary>
        public static readonly DependencyProperty ShowReadOnlyPropertiesProperty =
            DependencyProperty.Register(
                "ShowReadOnlyProperties",
                typeof(bool),
                typeof(PropertyControl),
                new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        ///   The tab header template property.
        /// </summary>
        public static readonly DependencyProperty TabHeaderTemplateProperty =
            DependencyProperty.Register("TabHeaderTemplate", typeof(DataTemplate), typeof(PropertyControl));

        /// <summary>
        ///   The tab page header template property.
        /// </summary>
        public static readonly DependencyProperty TabPageHeaderTemplateProperty =
            DependencyProperty.Register(
                "TabPageHeaderTemplate", typeof(DataTemplate), typeof(PropertyControl), new UIPropertyMetadata(null));

        /// <summary>
        ///   The tab strip placement property.
        /// </summary>
        public static readonly DependencyProperty TabStripPlacementProperty =
            DependencyProperty.Register(
                "TabStripPlacement", typeof(Dock), typeof(PropertyControl), new UIPropertyMetadata(Dock.Top));

        /// <summary>
        ///   The tool tip template property.
        /// </summary>
        public static readonly DependencyProperty ToolTipTemplateProperty =
            DependencyProperty.Register(
                "ToolTipTemplate", typeof(DataTemplate), typeof(PropertyControl), new UIPropertyMetadata(null));

        /// <summary>
        ///   The use tabs property.
        /// </summary>
        public static readonly DependencyProperty UseTabsProperty = DependencyProperty.Register(
            "UseTabs", typeof(bool), typeof(PropertyControl), new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        ///   The validation error style property.
        /// </summary>
        public static readonly DependencyProperty ValidationErrorStyleProperty =
            DependencyProperty.Register(
                "ValidationErrorStyle", typeof(Style), typeof(PropertyControl), new UIPropertyMetadata(null));

        /// <summary>
        ///   The validation error template property.
        /// </summary>
        public static readonly DependencyProperty ValidationErrorTemplateProperty =
            DependencyProperty.Register(
                "ValidationErrorTemplate", typeof(DataTemplate), typeof(PropertyControl), new UIPropertyMetadata(null));

        /// <summary>
        ///   The validation template property.
        /// </summary>
        public static readonly DependencyProperty ValidationTemplateProperty =
            DependencyProperty.Register(
                "ValidationTemplate", typeof(ControlTemplate), typeof(PropertyControl), new UIPropertyMetadata(null));

        /// <summary>
        ///   The panel part constant.
        /// </summary>
        private const string PartPanel = "PART_Panel";

        /// <summary>
        ///   The scroller part constant.
        /// </summary>
        private const string PartScroller = "PART_Scroller";

        /// <summary>
        ///   The tabs part constant.
        /// </summary>
        private const string PartTabs = "PART_Tabs";

        /// <summary>
        ///   The bool to visibility converter.
        /// </summary>
        private static readonly BoolToVisibilityConverter BoolToVisibilityConverter = new BoolToVisibilityConverter();

        /// <summary>
        ///   The null to bool converter.
        /// </summary>
        private static readonly NullToBoolConverter NullToBoolConverter = new NullToBoolConverter { NullValue = false };

        /// <summary>
        ///   The zero to visibility converter.
        /// </summary>
        private static readonly ZeroToVisibilityConverter ZeroToVisibilityConverter = new ZeroToVisibilityConverter();

        /// <summary>
        ///   The panel control.
        /// </summary>
        private StackPanel panelControl;

        /// <summary>
        ///   The scroll viewer.
        /// </summary>
        private ScrollViewer scrollViewer;

        /// <summary>
        ///   The tab control.
        /// </summary>
        private TabControl tabControl;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "PropertyControl" /> class.
        /// </summary>
        static PropertyControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(PropertyControl), new FrameworkPropertyMetadata(typeof(PropertyControl)));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PropertyControl" /> class.
        /// </summary>
        public PropertyControl()
        {
            this.PropertyControlFactory = new DefaultPropertyControlFactory();
            this.PropertyItemFactory = new DefaultPropertyItemFactory();
            this.Factories = new List<IPropertyControlFactory>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the actual width of the property labels.
        /// </summary>
        /// <value>The actual width.</value>
        public double ActualLabelWidth
        {
            get
            {
                return (double)this.GetValue(ActualLabelWidthProperty);
            }

            set
            {
                this.SetValue(ActualLabelWidthProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the category control template.
        /// </summary>
        /// <value>The category control template.</value>
        public ControlTemplate CategoryControlTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(CategoryControlTemplateProperty);
            }

            set
            {
                this.SetValue(CategoryControlTemplateProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the type of the category control.
        /// </summary>
        /// <value>The type of the category control.</value>
        public CategoryControlType CategoryControlType
        {
            get
            {
                return (CategoryControlType)this.GetValue(CategoryControlTypeProperty);
            }

            set
            {
                this.SetValue(CategoryControlTypeProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the category header template.
        /// </summary>
        /// <value>The category header template.</value>
        public DataTemplate CategoryHeaderTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(CategoryHeaderTemplateProperty);
            }

            set
            {
                this.SetValue(CategoryHeaderTemplateProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the description icon.
        /// </summary>
        /// <value>The description icon.</value>
        public ImageSource DescriptionIcon
        {
            get
            {
                return (ImageSource)this.GetValue(DescriptionIconProperty);
            }

            set
            {
                this.SetValue(DescriptionIconProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the alignment for description icons.
        /// </summary>
        /// <value>The description icon alignment.</value>
        public HorizontalAlignment DescriptionIconAlignment
        {
            get
            {
                return (HorizontalAlignment)this.GetValue(DescriptionIconAlignmentProperty);
            }

            set
            {
                this.SetValue(DescriptionIconAlignmentProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the limiting number of enum values for radio buttons lists.
        /// </summary>
        /// <value>The limit.</value>
        public int EnumAsRadioButtonsLimit
        {
            get
            {
                return (int)this.GetValue(EnumAsRadioButtonsLimitProperty);
            }

            set
            {
                this.SetValue(EnumAsRadioButtonsLimitProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the property control factories.
        /// </summary>
        /// <value>The factories.</value>
        public List<IPropertyControlFactory> Factories { get; set; }

        /// <summary>
        ///   Gets or sets the minimum width of the property labels.
        /// </summary>
        /// <value>The minimum width.</value>
        public double MinimumLabelWidth
        {
            get
            {
                return (double)this.GetValue(MinimumLabelWidthProperty);
            }

            set
            {
                this.SetValue(MinimumLabelWidthProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets PropertyControlFactory.
        /// </summary>
        public IPropertyControlFactory PropertyControlFactory
        {
            get
            {
                return (IPropertyControlFactory)this.GetValue(PropertyControlFactoryProperty);
            }

            set
            {
                this.SetValue(PropertyControlFactoryProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the property item factory.
        /// </summary>
        /// <value>The property item factory.</value>
        public IPropertyItemFactory PropertyItemFactory
        {
            get
            {
                return (IPropertyItemFactory)this.GetValue(PropertyItemFactoryProperty);
            }

            set
            {
                this.SetValue(PropertyItemFactoryProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the required attribute type.
        /// </summary>
        /// <value>The required attribute type.</value>
        public Type RequiredAttribute
        {
            get
            {
                return (Type)this.GetValue(RequiredAttributeProperty);
            }

            set
            {
                this.SetValue(RequiredAttributeProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the selected object.
        /// </summary>
        /// <value>The selected object.</value>
        public object SelectedObject
        {
            get
            {
                return this.GetValue(SelectedObjectProperty);
            }

            set
            {
                this.SetValue(SelectedObjectProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether to show check box headers.
        /// </summary>
        /// <value>
        ///   <c>true</c> if check box headers should be shown; otherwise, <c>false</c>.
        /// </value>
        public bool ShowCheckBoxHeaders
        {
            get
            {
                return (bool)this.GetValue(ShowCheckBoxHeadersProperty);
            }

            set
            {
                this.SetValue(ShowCheckBoxHeadersProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether to show declared properties only.
        /// </summary>
        /// <value><c>true</c> if only declared properti should be shown; otherwise, <c>false</c>.</value>
        public bool ShowDeclaredOnly
        {
            get
            {
                return (bool)this.GetValue(ShowDeclaredOnlyProperty);
            }

            set
            {
                this.SetValue(ShowDeclaredOnlyProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether to show description icons.
        /// </summary>
        public bool ShowDescriptionIcons
        {
            get
            {
                return (bool)this.GetValue(ShowDescriptionIconsProperty);
            }

            set
            {
                this.SetValue(ShowDescriptionIconsProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether to show read only properties].
        /// </summary>
        /// <value>
        ///   <c>true</c> if read only properties should be shown; otherwise, <c>false</c>.
        /// </value>
        public bool ShowReadOnlyProperties
        {
            get
            {
                return (bool)this.GetValue(ShowReadOnlyPropertiesProperty);
            }

            set
            {
                this.SetValue(ShowReadOnlyPropertiesProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the tab header template.
        /// </summary>
        /// <value>The tab header template.</value>
        public DataTemplate TabHeaderTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(TabHeaderTemplateProperty);
            }

            set
            {
                this.SetValue(TabHeaderTemplateProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the tab page header template.
        /// </summary>
        /// <value>The tab page header template.</value>
        public DataTemplate TabPageHeaderTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(TabPageHeaderTemplateProperty);
            }

            set
            {
                this.SetValue(TabPageHeaderTemplateProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the tab strip placement.
        /// </summary>
        /// <value>The tab strip placement.</value>
        public Dock TabStripPlacement
        {
            get
            {
                return (Dock)this.GetValue(TabStripPlacementProperty);
            }

            set
            {
                this.SetValue(TabStripPlacementProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the tool tip template.
        /// </summary>
        /// <value>The tool tip template.</value>
        public DataTemplate ToolTipTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(ToolTipTemplateProperty);
            }

            set
            {
                this.SetValue(ToolTipTemplateProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether to use tabs.
        /// </summary>
        /// <value><c>true</c> if tabs should be used; otherwise, <c>false</c>.</value>
        public bool UseTabs
        {
            get
            {
                return (bool)this.GetValue(UseTabsProperty);
            }

            set
            {
                this.SetValue(UseTabsProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the validation error style.
        /// </summary>
        /// <value>The validation error style.</value>
        public Style ValidationErrorStyle
        {
            get
            {
                return (Style)this.GetValue(ValidationErrorStyleProperty);
            }

            set
            {
                this.SetValue(ValidationErrorStyleProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the validation error template.
        /// </summary>
        /// <value>
        ///   The validation error template.
        /// </value>
        public DataTemplate ValidationErrorTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(ValidationErrorTemplateProperty);
            }

            set
            {
                this.SetValue(ValidationErrorTemplateProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the validation template.
        /// </summary>
        /// <value>The validation template.</value>
        public ControlTemplate ValidationTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(ValidationTemplateProperty);
            }

            set
            {
                this.SetValue(ValidationTemplateProperty, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the controls.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="items">
        /// The items.
        /// </param>
        public virtual void CreateControls(object instance, IEnumerable<PropertyItem> items)
        {
            if (this.tabControl == null)
            {
                return;
            }

            this.tabControl.Items.Clear();
            this.panelControl.Children.Clear();
            if (this.UseTabs)
            {
                this.tabControl.DataContext = instance;
            }
            else
            {
                this.panelControl.DataContext = instance;
            }

            this.tabControl.Visibility = this.UseTabs ? Visibility.Visible : Visibility.Hidden;
            this.scrollViewer.Visibility = !this.UseTabs ? Visibility.Visible : Visibility.Hidden;

            if (items == null)
            {
                return;
            }

            HeaderedContentControl group = null;
            string currentTab = null;
            string currentCategory = null;

            StackPanel categoryItems = null;
            Panel tabItems = null;

            double maxLabelWidth = this.MinimumLabelWidth;

            foreach (var pi in items)
            {
                if (currentTab == null || !currentTab.Equals(pi.Tab))
                {
                    tabItems = new StackPanel();

                    if (this.UseTabs)
                    {
                        var scroller = new ScrollViewer
                            {
                                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                                Content = tabItems
                            };
                        var tab = new TabItem { Header = pi.Tab, Content = scroller };

                        if (this.TabHeaderTemplate != null)
                        {
                            tab.Header = new HeaderViewModel { Header = pi.Tab, Icon = pi.TabIcon };
                            tab.HeaderTemplate = this.TabHeaderTemplate;
                        }

                        this.tabControl.Items.Add(tab);
                    }
                    else
                    {
                        this.panelControl.Children.Add(tabItems);
                    }

                    if (this.TabPageHeaderTemplate != null)
                    {
                        var hc = new ContentControl
                            {
                                ContentTemplate = this.TabPageHeaderTemplate,
                                Content =
                                    new HeaderViewModel
                                        {
                                            Header = pi.Tab,
                                            Description = pi.TabDescription,
                                            Icon = pi.TabIcon
                                        }
                            };
                        tabItems.Children.Add(hc);
                    }

                    currentTab = pi.Tab;
                    currentCategory = null;
                }

                if (currentCategory == null || !currentCategory.Equals(pi.Category))
                {
                    categoryItems = new StackPanelEx();

                    switch (this.CategoryControlType)
                    {
                        case CategoryControlType.GroupBox:
                            group = new GroupBox { Margin = new Thickness(0, 4, 0, 4) };
                            break;
                        case CategoryControlType.Expander:
                            group = new Expander { IsExpanded = currentCategory == null };
                            break;
                        case CategoryControlType.Template:
                            group = new HeaderedContentControl { Template = this.CategoryControlTemplate };
                            break;
                    }

                    if (group != null)
                    {
                        if (this.CategoryHeaderTemplate != null)
                        {
                            group.HeaderTemplate = this.CategoryHeaderTemplate;
                            group.Header = new HeaderViewModel
                                {
                                    Header = pi.Category,
                                    Description = pi.CategoryDescription,
                                    Icon = pi.CategoryIcon
                                };
                        }
                        else
                        {
                            group.Header = pi.Category;
                        }

                        // Hide the group control if all child properties are invisible.
                        group.SetBinding(
                            VisibilityProperty,
                            new Binding("VisibleChildrenCount")
                                {
                                    Source = categoryItems,
                                    Converter = ZeroToVisibilityConverter
                                });

                        group.Content = categoryItems;
                        tabItems.Children.Add(group);
                    }

                    currentCategory = pi.Category;
                }

                // create the property panel (label, tooltip icon and property control)
                var propertyPanel = this.CreatePropertyPanel(pi, instance, ref maxLabelWidth);
                categoryItems.Children.Add(propertyPanel);
            }

            // set the label width to the calculated max label width
            this.ActualLabelWidth = maxLabelWidth;

            int index = this.tabControl.SelectedIndex;
            if (index >= this.tabControl.Items.Count || (uint)index == 0xffffffff)
            {
                index = 0;
            }

            if (this.tabControl.Items.Count > 0)
            {
                this.tabControl.SelectedItem = this.tabControl.Items[index];
            }
        }

        /// <summary>
        /// Creates the property item list.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="isEnumerable">
        /// if set to <c>true</c> [is enumerable].
        /// </param>
        /// <returns>
        /// A list of <see cref="PropertyItem"/>.
        /// </returns>
        public virtual IList<PropertyItem> CreateItems(object instance, bool isEnumerable)
        {
            if (instance == null)
            {
                return null;
            }

            var instanceType = instance.GetType();
            var properties = TypeDescriptor.GetProperties(instance);

            this.PropertyItemFactory.Reset();

            var items = new List<PropertyItem>();

            foreach (PropertyDescriptor pd in properties)
            {
                if (this.ShowDeclaredOnly && pd.ComponentType != instanceType)
                {
                    continue;
                }

                // Skip properties marked with [Browsable(false)]
                if (!pd.IsBrowsable)
                {
                    continue;
                }

                // Read-only properties
                if (!this.ShowReadOnlyProperties && pd.IsReadOnly)
                {
                    continue;
                }

                // If RequiredAttribute is set, skip properties that don't have the given attribute
                if (this.RequiredAttribute != null
                    && !AttributeHelper.ContainsAttributeOfType(pd.Attributes, this.RequiredAttribute))
                {
                    continue;
                }

                var pi = this.PropertyItemFactory.CreatePropertyItem(pd, instance);
                items.Add(pi);
            }

            return items.OrderBy(t => t.SortIndex).ToList();
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.tabControl = this.Template.FindName(PartTabs, this) as TabControl;
            this.panelControl = this.Template.FindName(PartPanel, this) as StackPanel;
            this.scrollViewer = this.Template.FindName(PartScroller, this) as ScrollViewer;
            this.UpdateControls();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a tool tip.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <returns>
        /// The tool tip.
        /// </returns>
        protected virtual object CreateToolTip(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            if (this.ToolTipTemplate == null)
            {
                return content;
            }

            return new ContentControl { ContentTemplate = this.ToolTipTemplate, Content = content };
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.KeyDown"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.KeyEventArgs"/> that contains the event data.
        /// </param>
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
                    if (bindingExpression != null && bindingExpression.Status == BindingStatus.Active)
                    {
                        bindingExpression.UpdateSource();

                        textBox.CaretIndex = textBox.Text.Length;
                        textBox.SelectAll();
                    }
                }

                var uie = e.OriginalSource as UIElement;
                if (uie != null)
                {
                    uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when the selected object is changed.
        /// </summary>
        protected virtual void OnSelectedObjectChanged()
        {
            this.UpdateControls();
        }

        /// <summary>
        /// The appearance changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyControl)d).UpdateControls();
        }

        /// <summary>
        /// The selected object changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void SelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyControl)d).OnSelectedObjectChanged();
        }

        /// <summary>
        /// Creates the label control.
        /// </summary>
        /// <param name="pi">
        /// The property item.
        /// </param>
        /// <returns>
        /// An element.
        /// </returns>
        private FrameworkElement CreateLabel(PropertyItem pi)
        {
            FrameworkElement propertyLabel;
            if (!pi.IsOptional)
            {
                propertyLabel = new Label
                    {
                        Content = pi.DisplayName,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(0, 4, 0, 0)
                    };
            }
            else
            {
                var cb = new CheckBox
                    {
                        Content = pi.DisplayName,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(5, 0, 0, 0)
                    };

                if (pi.OptionalDescriptor != null)
                {
                    cb.SetBinding(ToggleButton.IsCheckedProperty, new Binding(pi.OptionalDescriptor.Name));
                }
                else
                {
                    cb.SetBinding(
                        ToggleButton.IsCheckedProperty,
                        new Binding(pi.Descriptor.Name) { Converter = NullToBoolConverter });
                }

                var g = new Grid();
                g.Children.Add(cb);
                propertyLabel = g;
            }

            propertyLabel.Margin = new Thickness(0, 0, 4, 0);
            return propertyLabel;
        }

        /// <summary>
        /// Creates the property control.
        /// </summary>
        /// <param name="pi">
        /// The property item.
        /// </param>
        /// <returns>
        /// An element.
        /// </returns>
        private FrameworkElement CreatePropertyControl(PropertyItem pi)
        {
            var options = new PropertyControlFactoryOptions { EnumAsRadioButtonsLimit = this.EnumAsRadioButtonsLimit };
            return this.PropertyControlFactory.CreateControl(pi, options);
        }

        /// <summary>
        /// Creates the property panel.
        /// </summary>
        /// <param name="pi">
        /// The pi.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="maxLabelWidth">
        /// Width of the max label.
        /// </param>
        /// <returns>
        /// The property panel.
        /// </returns>
        private UIElement CreatePropertyPanel(PropertyItem pi, object instance, ref double maxLabelWidth)
        {
            var propertyPanel = new DockPanel { Margin = new Thickness(2) };
            var propertyLabel = this.CreateLabel(pi);
            var propertyControl = this.CreatePropertyControl(pi);
            if (propertyControl != null)
            {
                if (!double.IsNaN(pi.Height))
                {
                    propertyControl.Height = pi.Height;
                }

                if (!double.IsNaN(pi.MinimumHeight))
                {
                    propertyControl.MinHeight = pi.MinimumHeight;
                }

                if (!double.IsNaN(pi.MaximumHeight))
                {
                    propertyControl.MaxHeight = pi.MaximumHeight;
                }

                if (pi.IsOptional)
                {
                    if (pi.OptionalDescriptor != null)
                    {
                        propertyControl.SetBinding(IsEnabledProperty, new Binding(pi.OptionalDescriptor.Name));
                    }
                    else
                    {
                        propertyControl.SetBinding(
                            IsEnabledProperty, new Binding(pi.Descriptor.Name) { Converter = NullToBoolConverter });
                    }
                }

                if (instance is IDataErrorInfo)
                {
                    if (this.ValidationTemplate != null)
                    {
                        Validation.SetErrorTemplate(propertyControl, this.ValidationTemplate);
                    }

                    if (this.ValidationErrorStyle != null)
                    {
                        propertyControl.Style = this.ValidationErrorStyle;
                    }

                    var errorControl = new ContentControl { ContentTemplate = this.ValidationErrorTemplate };
                    errorControl.SetBinding(
                        VisibilityProperty,
                        new Binding("(Validation.HasError)")
                            {
                                Source = propertyControl,
                                Converter = BoolToVisibilityConverter
                            });
                    errorControl.SetBinding(
                        ContentControl.ContentProperty, new Binding("(Validation.Errors)") { Source = propertyControl });

                    // replace the property control by a stack panel containig the property control and the error control.
                    var sp = new StackPanel();
                    sp.VerticalAlignment = VerticalAlignment.Center;
                    sp.Children.Add(propertyControl);
                    sp.Children.Add(errorControl);
                    propertyControl = sp;
                }
            }

            var actualHeaderPlacement = pi.HeaderPlacement;

            if (!this.ShowCheckBoxHeaders && propertyControl is CheckBox)
            {
                actualHeaderPlacement = HeaderPlacement.Collapsed;
                var cb = propertyControl as CheckBox;
                cb.Content = propertyLabel;

                propertyLabel = null;
            }

            switch (actualHeaderPlacement)
            {
                case HeaderPlacement.Hidden:
                    {
                        var labelPanel = new DockPanel();
                        labelPanel.SetBinding(MinWidthProperty, new Binding("ActualLabelWidth") { Source = this });
                        propertyPanel.Children.Add(labelPanel);
                        break;
                    }

                case HeaderPlacement.Collapsed:
                    break;
                default:
                    {
                        // create the label panel 
                        var labelPanel = new DockPanel();
                        if (pi.HeaderPlacement == HeaderPlacement.Left)
                        {
                            DockPanel.SetDock(labelPanel, Dock.Left);
                            labelPanel.SetBinding(MinWidthProperty, new Binding("ActualLabelWidth") { Source = this });
                        }
                        else
                        {
                            DockPanel.SetDock(labelPanel, Dock.Top);
                        }

                        propertyPanel.Children.Add(labelPanel);

                        if (propertyLabel != null)
                        {
                            DockPanel.SetDock(propertyLabel, Dock.Left);
                            labelPanel.Children.Add(propertyLabel);
                        }

                        if (this.ShowDescriptionIcons && this.DescriptionIcon != null)
                        {
                            if (!string.IsNullOrWhiteSpace(pi.Description))
                            {
                                var descriptionIconImage = new Image
                                    {
                                        Source = this.DescriptionIcon,
                                        Stretch = Stretch.None,
                                        Margin = new Thickness(0, 4, 4, 4),
                                        VerticalAlignment = VerticalAlignment.Top
                                    };

                                // RenderOptions.SetBitmapScalingMode(descriptionIconImage, BitmapScalingMode.NearestNeighbor);
                                descriptionIconImage.HorizontalAlignment = this.DescriptionIconAlignment;
                                labelPanel.Children.Add(descriptionIconImage);
                                if (!string.IsNullOrWhiteSpace(pi.Description))
                                {
                                    descriptionIconImage.ToolTip = this.CreateToolTip(pi.Description);
                                }
                            }
                        }
                        else
                        {
                            labelPanel.ToolTip = this.CreateToolTip(pi.Description);
                        }

                        // measure the size of the label and tooltip icon
                        labelPanel.Measure(new Size(this.ActualWidth, this.ActualHeight));
                        maxLabelWidth = Math.Max(maxLabelWidth, labelPanel.DesiredSize.Width);
                    }

                    break;
            }

            // add the property control
            if (propertyControl != null)
            {
                propertyPanel.Children.Add(propertyControl);
            }

            if (pi.IsEnabledDescriptor != null)
            {
                propertyPanel.SetBinding(IsEnabledProperty, new Binding(pi.IsEnabledDescriptor.Name));
            }

            if (pi.IsVisibleDescriptor != null)
            {
                propertyPanel.SetBinding(
                    VisibilityProperty,
                    new Binding(pi.IsVisibleDescriptor.Name) { Converter = BoolToVisibilityConverter });
            }

            return propertyPanel;
        }

        /// <summary>
        /// Updates the controls.
        /// </summary>
        private void UpdateControls()
        {
            var items = this.CreateItems(this.SelectedObject, false);
            this.CreateControls(this.SelectedObject, items);
        }

        #endregion
    }
}