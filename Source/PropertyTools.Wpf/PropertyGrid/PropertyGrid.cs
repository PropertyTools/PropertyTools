// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGrid.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies how the label widths are shared.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    using PropertyTools.DataAnnotations;

    using HorizontalAlignment = System.Windows.HorizontalAlignment;

    /// <summary>
    /// Specifies how the label widths are shared.
    /// </summary>
    public enum LabelWidthSharing
    {
        /// <summary>
        /// The shared in tab.
        /// </summary>
        SharedInTab,

        /// <summary>
        /// The shared in group.
        /// </summary>
        SharedInGroup,

        /// <summary>
        /// The not shared.
        /// </summary>
        NotShared
    }

    /// <summary>
    /// Specifies the layout for checkboxes.
    /// </summary>
    public enum CheckBoxLayout
    {
        /// <summary>
        /// Show the header, then the check box without content
        /// </summary>
        Header,

        /// <summary>
        /// Hide the header, show the check box with the display name as content
        /// </summary>
        HideHeader,

        /// <summary>
        /// Collapse the header, show the check box with the display name as content
        /// </summary>
        CollapseHeader
    }

    /// <summary>
    /// Specifies the visibility of the tab strip.
    /// </summary>
    public enum TabVisibility
    {
        /// <summary>
        /// The tabs are visible.
        /// </summary>
        Visible,

        /// <summary>
        /// The tabs are visible if there is more than one tab.
        /// </summary>
        VisibleIfMoreThanOne,

        /// <summary>
        /// The tab strip is collapsed. The contents of the tab pages will be stacked vertically in the control. 
        /// </summary>
        Collapsed
    }

    /// <summary>
    /// The property control.
    /// </summary>
    [TemplatePart(Name = PartTabs, Type = typeof(TabControl))]
    [TemplatePart(Name = PartPanel, Type = typeof(StackPanel))]
    [TemplatePart(Name = PartScrollViewer, Type = typeof(ScrollViewer))]
    public class PropertyGrid : Control, IPropertyGridOptions
    {
        /// <summary>
        /// Identifies the <see cref="CategoryControlTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CategoryControlTemplateProperty = DependencyProperty.Register(
            nameof(CategoryControlTemplate),
            typeof(ControlTemplate),
            typeof(PropertyGrid),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="CategoryControlType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CategoryControlTypeProperty = DependencyProperty.Register(
            nameof(CategoryControlType),
            typeof(CategoryControlType),
            typeof(PropertyGrid),
            new UIPropertyMetadata(CategoryControlType.GroupBox, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="CategoryHeaderTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CategoryHeaderTemplateProperty = DependencyProperty.Register(
            nameof(CategoryHeaderTemplate),
            typeof(DataTemplate),
            typeof(PropertyGrid));

        /// <summary>
        /// Identifies the <see cref="DescriptionIconAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionIconAlignmentProperty = DependencyProperty.Register(
            nameof(DescriptionIconAlignment),
            typeof(HorizontalAlignment),
            typeof(PropertyGrid),
            new UIPropertyMetadata(HorizontalAlignment.Right, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="DescriptionIcon"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionIconProperty = DependencyProperty.Register(
            nameof(DescriptionIcon), typeof(ImageSource), typeof(PropertyGrid));

        /// <summary>
        /// Identifies the <see cref="EnableLabelWidthResizing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EnableLabelWidthResizingProperty = DependencyProperty.Register(
            nameof(EnableLabelWidthResizing),
            typeof(bool),
            typeof(PropertyGrid),
            new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="EnumAsRadioButtonsLimit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EnumAsRadioButtonsLimitProperty = DependencyProperty.Register(
            nameof(EnumAsRadioButtonsLimit),
            typeof(int),
            typeof(PropertyGrid),
            new UIPropertyMetadata(4, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Indentation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IndentationProperty = DependencyProperty.Register(
            nameof(Indentation),
            typeof(double),
            typeof(PropertyGrid),
            new UIPropertyMetadata(16d));

        /// <summary>
        /// Identifies the <see cref="LabelWidthSharing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelWidthSharingProperty = DependencyProperty.Register(
            nameof(LabelWidthSharing),
            typeof(LabelWidthSharing),
            typeof(PropertyGrid),
            new UIPropertyMetadata(LabelWidthSharing.SharedInTab, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MaximumLabelWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumLabelWidthProperty = DependencyProperty.Register(
            nameof(MaximumLabelWidth),
            typeof(double),
            typeof(PropertyGrid),
            new UIPropertyMetadata(double.MaxValue));

        /// <summary>
        /// Identifies the <see cref="MinimumLabelWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumLabelWidthProperty = DependencyProperty.Register(
            nameof(MinimumLabelWidth),
            typeof(double),
            typeof(PropertyGrid),
            new UIPropertyMetadata(70.0));

        /// <summary>
        /// Identifies the <see cref="MoveFocusOnEnter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MoveFocusOnEnterProperty = DependencyProperty.Register(
            nameof(MoveFocusOnEnter),
            typeof(bool),
            typeof(PropertyGrid),
            new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="ControlFactory"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ControlFactoryProperty = DependencyProperty.Register(
            nameof(ControlFactory),
            typeof(IPropertyGridControlFactory),
            typeof(PropertyGrid),
            new UIPropertyMetadata(new PropertyGridControlFactory()));

        /// <summary>
        /// Identifies the <see cref="PropertyItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OperatorProperty = DependencyProperty.Register(
            nameof(Operator),
            typeof(IPropertyGridOperator),
            typeof(PropertyGrid),
            new UIPropertyMetadata(new PropertyGridOperator()));

        /// <summary>
        /// Identifies the <see cref="RequiredAttribute"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RequiredAttributeProperty = DependencyProperty.Register(
            nameof(RequiredAttribute),
            typeof(Type),
            typeof(PropertyGrid),
            new UIPropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="SelectedObject"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
            nameof(SelectedObject),
            typeof(object),
            typeof(PropertyGrid),
            new UIPropertyMetadata(null, (s, e) => ((PropertyGrid)s).OnSelectedObjectChanged(e)));

        /// <summary>
        /// Identifies the <see cref="SelectedObjects"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedObjectsProperty = DependencyProperty.Register(
            nameof(SelectedObjects),
            typeof(IEnumerable),
            typeof(PropertyGrid),
            new UIPropertyMetadata(null, (s, e) => ((PropertyGrid)s).SelectedObjectsChanged(e)));

        /// <summary>
        /// Identifies the <see cref="SelectedTabIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedTabIndexProperty = DependencyProperty.Register(
            nameof(SelectedTabIndex),
            typeof(int),
            typeof(PropertyGrid),
            new FrameworkPropertyMetadata(0, (s, e) => ((PropertyGrid)s).SelectedTabIndexChanged(e)));

        /// <summary>
        /// The selected tab id property.
        /// </summary>
        public static readonly DependencyProperty SelectedTabIdProperty = DependencyProperty.Register(
            nameof(SelectedTabId),
            typeof(string),
            typeof(PropertyGrid),
            new UIPropertyMetadata(null, (s, e) => ((PropertyGrid)s).SelectedTabChanged(e)));

        /// <summary>
        /// Identifies the <see cref="CheckBoxLayout"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CheckBoxLayoutProperty = DependencyProperty.Register(
            nameof(CheckBoxLayout),
            typeof(CheckBoxLayout),
            typeof(PropertyGrid),
            new UIPropertyMetadata(CheckBoxLayout.Header, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ShowDeclaredOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowDeclaredOnlyProperty = DependencyProperty.Register(
            nameof(ShowDeclaredOnly),
            typeof(bool),
            typeof(PropertyGrid),
            new UIPropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ShowDescriptionIcons"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowDescriptionIconsProperty = DependencyProperty.Register(
            nameof(ShowDescriptionIcons),
            typeof(bool),
            typeof(PropertyGrid),
            new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ShowReadOnlyProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowReadOnlyPropertiesProperty = DependencyProperty.Register(
            nameof(ShowReadOnlyProperties),
            typeof(bool),
            typeof(PropertyGrid),
            new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TabHeaderTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TabHeaderTemplateProperty = DependencyProperty.Register(
            nameof(TabHeaderTemplate),
            typeof(DataTemplate),
            typeof(PropertyGrid));

        /// <summary>
        /// Identifies the <see cref="TabPageHeaderTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TabPageHeaderTemplateProperty = DependencyProperty.Register(
            nameof(TabPageHeaderTemplate),
            typeof(DataTemplate),
            typeof(PropertyGrid),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="TabStripPlacement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TabStripPlacementProperty = DependencyProperty.Register(
            nameof(TabStripPlacement),
            typeof(Dock),
            typeof(PropertyGrid),
            new UIPropertyMetadata(Dock.Top));

        /// <summary>
        /// Identifies the <see cref="ToolTipTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register(
            nameof(ToolTipTemplate),
            typeof(DataTemplate),
            typeof(PropertyGrid),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="TabVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TabVisibilityProperty = DependencyProperty.Register(
            nameof(TabVisibility),
            typeof(TabVisibility),
            typeof(PropertyGrid),
            new UIPropertyMetadata(TabVisibility.Visible, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ValidationErrorStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidationErrorStyleProperty = DependencyProperty.Register(
            nameof(ValidationErrorStyle),
            typeof(Style),
            typeof(PropertyGrid),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ValidationErrorTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidationErrorTemplateProperty = DependencyProperty.Register(
            nameof(ValidationErrorTemplate),
            typeof(DataTemplate),
            typeof(PropertyGrid),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ValidationTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidationTemplateProperty = DependencyProperty.Register(
            nameof(ValidationTemplate),
            typeof(ControlTemplate),
            typeof(PropertyGrid),
            new UIPropertyMetadata(null));

        /// <summary>
        /// The panel part name.
        /// </summary>
        private const string PartPanel = "PART_Panel";

        /// <summary>
        /// The scroll control part name.
        /// </summary>
        private const string PartScrollViewer = "PART_ScrollViewer";

        /// <summary>
        /// The tab control part name.
        /// </summary>
        private const string PartTabs = "PART_Tabs";

        /// <summary>
        /// The value to visibility converter.
        /// </summary>
        private static readonly ValueToVisibilityConverter ValueToVisibilityConverter = new ValueToVisibilityConverter();

        /// <summary>
        /// The value to boolean converter.
        /// </summary>
        private static readonly ValueToBooleanConverter ValueToBooleanConverter = new ValueToBooleanConverter();

        /// <summary>
        /// Converts a list of values to a boolean value. Returns <c>true</c> if all values equal the converter parameter.
        /// </summary>
        private static readonly AllMultiValueConverter AllMultiValueConverter = new AllMultiValueConverter();

        /// <summary>
        /// The <c>null</c> to boolean converter.
        /// </summary>
        private static readonly NullToBoolConverter NullToBoolConverter = new NullToBoolConverter { NullValue = false };

        /// <summary>
        /// The zero to visibility converter.
        /// </summary>
        private static readonly ZeroToVisibilityConverter ZeroToVisibilityConverter = new ZeroToVisibilityConverter();

        /// <summary>
        /// The current selected object type.
        /// </summary>
        private Type currentSelectedObjectType;

        /// <summary>
        /// The panel control.
        /// </summary>
        private StackPanel panelControl;

        /// <summary>
        /// The scroll viewer.
        /// </summary>
        private ScrollViewer scrollViewer;

        /// <summary>
        /// The tab control.
        /// </summary>
        private TabControl tabControl;

        /// <summary>
        /// Initializes static members of the <see cref="PropertyGrid" /> class.
        /// </summary>
        static PropertyGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(PropertyGrid), new FrameworkPropertyMetadata(typeof(PropertyGrid)));
        }

        /// <summary>
        /// Gets or sets the category control template.
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
        /// Gets or sets the type of the category control.
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
        /// Gets or sets the category header template.
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
        /// Gets or sets CurrentObject.
        /// </summary>
        public object CurrentObject { get; set; }

        /// <summary>
        /// Gets or sets the description icon.
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
        /// Gets or sets the alignment for description icons.
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
        /// Gets or sets a value indicating whether label column resizing is enabled.
        /// </summary>
        public bool EnableLabelWidthResizing
        {
            get
            {
                return (bool)this.GetValue(EnableLabelWidthResizingProperty);
            }

            set
            {
                this.SetValue(EnableLabelWidthResizingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of values to show for radio buttons lists.
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
        /// Gets or sets the indentation.
        /// </summary>
        /// <value>
        /// The indentation.
        /// </value>
        public double Indentation
        {
            get
            {
                return (double)this.GetValue(IndentationProperty);
            }

            set
            {
                this.SetValue(IndentationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the type of label width sharing.
        /// </summary>
        /// <value>The label width sharing.</value>
        public LabelWidthSharing LabelWidthSharing
        {
            get
            {
                return (LabelWidthSharing)this.GetValue(LabelWidthSharingProperty);
            }

            set
            {
                this.SetValue(LabelWidthSharingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum width of the label.
        /// </summary>
        /// <value>The maximum width of the label.</value>
        public double MaximumLabelWidth
        {
            get
            {
                return (double)this.GetValue(MaximumLabelWidthProperty);
            }

            set
            {
                this.SetValue(MaximumLabelWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the minimum width of the property labels.
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
        /// Gets or sets a value indicating whether to move focus on unhandled Enter key down events.
        /// </summary>
        /// <value><c>true</c> if move focus on enter; otherwise, <c>false</c> .</value>
        public bool MoveFocusOnEnter
        {
            get
            {
                return (bool)this.GetValue(MoveFocusOnEnterProperty);
            }

            set
            {
                this.SetValue(MoveFocusOnEnterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the control factory.
        /// </summary>
        public IPropertyGridControlFactory ControlFactory
        {
            get
            {
                return (IPropertyGridControlFactory)this.GetValue(ControlFactoryProperty);
            }

            set
            {
                this.SetValue(ControlFactoryProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>The operator.</value>
        public IPropertyGridOperator Operator
        {
            get
            {
                return (IPropertyGridOperator)this.GetValue(OperatorProperty);
            }

            set
            {
                this.SetValue(OperatorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected object.
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
        /// Gets or sets the selected objects.
        /// </summary>
        /// <value>The selected objects.</value>
        public IEnumerable SelectedObjects
        {
            get
            {
                return (IEnumerable)this.GetValue(SelectedObjectsProperty);
            }

            set
            {
                this.SetValue(SelectedObjectsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected tab.
        /// </summary>
        /// <value>The index of the selected tab.</value>
        public int SelectedTabIndex
        {
            get
            {
                return (int)this.GetValue(SelectedTabIndexProperty);
            }

            set
            {
                this.SetValue(SelectedTabIndexProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected tab id.
        /// </summary>
        public string SelectedTabId
        {
            get
            {
                return (string)this.GetValue(SelectedTabIdProperty);
            }

            set
            {
                this.SetValue(SelectedTabIdProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the check box layout.
        /// </summary>
        /// <value>The check box layout.</value>
        public CheckBoxLayout CheckBoxLayout
        {
            get
            {
                return (CheckBoxLayout)this.GetValue(CheckBoxLayoutProperty);
            }

            set
            {
                this.SetValue(CheckBoxLayoutProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show description icons.
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
        /// Gets or sets the tab header template.
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
        /// Gets or sets the tab page header template.
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
        /// Gets or sets the tab strip placement.
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
        /// Gets or sets the tool tip template.
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
        /// Gets or sets a value indicating the tab visibility state.
        /// </summary>
        /// <value>The tab visibility state.</value>
        public TabVisibility TabVisibility
        {
            get
            {
                return (TabVisibility)this.GetValue(TabVisibilityProperty);
            }

            set
            {
                this.SetValue(TabVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the validation error style.
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
        /// Gets or sets the validation error template.
        /// </summary>
        /// <value>The validation error template.</value>
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
        /// Gets or sets the validation template.
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

        /// <summary>
        /// Gets or sets the required attribute type.
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
        /// Gets or sets a value indicating whether to show declared properties only.
        /// </summary>
        /// <value><c>true</c> if only declared properties should be shown; otherwise, <c>false</c> .</value>
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
        /// Gets or sets a value indicating whether to show read only properties].
        /// </summary>
        /// <value><c>true</c> if read only properties should be shown; otherwise, <c>false</c> .</value>
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
        /// Creates the controls.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="tabs">The tabs.</param>
        public virtual void CreateControls(object instance, IEnumerable<Tab> tabs)
        {
            if (this.tabControl == null)
            {
                return;
            }

            this.tabControl.Items.Clear();
            this.panelControl.Children.Clear();

            this.tabControl.DataContext = instance;

            // Set padding to zero - control margin on the tab items instead
            this.tabControl.Padding = new Thickness(0);

            this.tabControl.Visibility = Visibility.Visible;
            this.scrollViewer.Visibility = Visibility.Hidden;

            if (tabs == null)
            {
                return;
            }

            foreach (var tab in tabs)
            {
                bool fillTab = tab.Groups.Count == 1 && tab.Groups[0].Properties.Count == 1
                               && tab.Groups[0].Properties[0].FillTab;

                // Create the panel for the tab content
                var tabPanel = new Grid();

                if (this.LabelWidthSharing == LabelWidthSharing.SharedInTab)
                {
                    Grid.SetIsSharedSizeScope(tabPanel, true);
                }

                var tabItem = new TabItem { Header = tab, Padding = new Thickness(4), Name = tab.Id ?? string.Empty };

                if (instance is IDataErrorInfo dataErrorInfoInstance)
                {
                    tab.UpdateHasErrors(dataErrorInfoInstance);
                }
                else if (instance is INotifyDataErrorInfo notifyDataErrorInfoInstance)
                {
                    tab.UpdateHasErrors(notifyDataErrorInfoInstance);
                }

                if (fillTab)
                {
                    tabItem.Content = tabPanel;
                }
                else
                {
                    tabItem.Content = new ScrollViewer
                    {
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                        Content = tabPanel,
                        Focusable = false
                    };
                }

                this.tabControl.Items.Add(tabItem);

                // set no margin if 'fill tab' and no tab page header
                tabPanel.Margin = new Thickness(fillTab && this.TabPageHeaderTemplate == null ? 0 : 4);

                if (this.TabHeaderTemplate != null)
                {
                    tabItem.Header = tab;
                    tabItem.HeaderTemplate = this.TabHeaderTemplate;
                }

                this.AddTabPageHeader(tab, tabPanel);

                int i = 0;
                foreach (var g in tab.Groups)
                {
                    var groupPanel = this.CreatePropertyPanel(g, tabPanel, i++, fillTab);
                    foreach (var pi in g.Properties)
                    {
                        // create and add the property panel (label, tooltip icon and property control)
                        this.AddPropertyPanel(groupPanel, pi, instance, tab);
                    }
                }
            }

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
        /// Creates the controls (not using tab control).
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="tabs">The tab collection.</param>
        public virtual void CreateControlsTabless(object instance, IEnumerable<Tab> tabs)
        {
            if (this.tabControl == null)
            {
                return;
            }

            this.tabControl.Items.Clear();
            this.panelControl.Children.Clear();
            this.tabControl.Visibility = Visibility.Hidden;
            this.scrollViewer.Visibility = Visibility.Visible;
            if (tabs == null)
            {
                return;
            }

            this.panelControl.DataContext = instance;

            foreach (var tab in tabs)
            {
                // Create the panel for the properties
                var panel = new Grid();

                if (this.LabelWidthSharing == LabelWidthSharing.SharedInTab)
                {
                    Grid.SetIsSharedSizeScope(panel, true);
                }

                this.panelControl.Children.Add(panel);

                this.AddTabPageHeader(tab, panel);

                // Add the groups
                int i = 0;
                foreach (var g in tab.Groups)
                {
                    var propertyPanel = this.CreatePropertyPanel(g, panel, i++, false);

                    foreach (var pi in g.Properties)
                    {
                        // create and add the property panel (label, tooltip icon and property control)
                        this.AddPropertyPanel(propertyPanel, pi, instance, tab);
                    }
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see
        /// cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> .
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.tabControl = this.Template.FindName(PartTabs, this) as TabControl;
            this.panelControl = this.Template.FindName(PartPanel, this) as StackPanel;
            this.scrollViewer = this.Template.FindName(PartScrollViewer, this) as ScrollViewer;
            this.UpdateControls();
        }

        /// <summary>
        /// Creates a tool tip.
        /// </summary>
        /// <param name="content">The content.</param>
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
        /// Invoked when an unhandled KeyDown attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (this.MoveFocusOnEnter && e.Key == Key.Enter)
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
        /// <param name="e">The e.</param>
        protected virtual void OnSelectedObjectChanged(DependencyPropertyChangedEventArgs e)
        {
            this.CurrentObject = this.SelectedObject;
            this.UpdateControls();
        }

        /// <summary>
        /// The appearance changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyGrid)d).UpdateControls();
        }

        /// <summary>
        /// Creates the content control and property panel.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="tabItems">The tab items.</param>
        /// <param name="index">The index.</param>
        /// <param name="fillTab">Stretch the panel if set to <c>true</c>.</param>
        /// <returns>
        /// The property panel.
        /// </returns>
        private Panel CreatePropertyPanel(Group g, Grid tabItems, int index, bool fillTab)
        {
            if (fillTab)
            {
                var p = new Grid();
                tabItems.Children.Add(p);
                Grid.SetRow(p, tabItems.RowDefinitions.Count);
                tabItems.RowDefinitions.Add(
                    new System.Windows.Controls.RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                return p;
            }

            var propertyPanel = new StackPanelEx();

            HeaderedContentControl groupContentControl = null;
            switch (this.CategoryControlType)
            {
                case CategoryControlType.GroupBox:
                    groupContentControl = new GroupBox { Margin = new Thickness(0, 4, 0, 4) };
                    break;
                case CategoryControlType.Expander:
                    groupContentControl = new Expander { IsExpanded = index == 0 };
                    break;
                case CategoryControlType.Template:
                    groupContentControl = new HeaderedContentControl
                    {
                        Template = this.CategoryControlTemplate,
                        Focusable = false
                    };
                    break;
            }

            if (groupContentControl != null)
            {
                if (this.CategoryHeaderTemplate != null)
                {
                    groupContentControl.HeaderTemplate = this.CategoryHeaderTemplate;
                    groupContentControl.Header = g;
                }
                else
                {
                    groupContentControl.Header = g.Header;
                }

                // Hide the group control if all child properties are invisible.
                groupContentControl.SetBinding(
                    UIElement.VisibilityProperty,
                    new Binding("VisibleChildrenCount")
                    {
                        Source = propertyPanel,
                        Converter = ZeroToVisibilityConverter
                    });

                if (this.LabelWidthSharing == LabelWidthSharing.SharedInGroup)
                {
                    Grid.SetIsSharedSizeScope(propertyPanel, true);
                }

                groupContentControl.Content = propertyPanel;
                tabItems.Children.Add(groupContentControl);
                Grid.SetRow(groupContentControl, tabItems.RowDefinitions.Count);

                tabItems.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = GridLength.Auto });
            }

            return propertyPanel;
        }

        /// <summary>
        /// Adds the tab page header if TabPageHeaderTemplate is specified.
        /// </summary>
        /// <param name="tab">The tab.</param>
        /// <param name="panel">The tab panel.</param>
        private void AddTabPageHeader(Tab tab, Grid panel)
        {
            if (this.TabPageHeaderTemplate == null)
            {
                return;
            }

            panel.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = GridLength.Auto });
            var hc = new ContentControl
            {
                Focusable = false,
                ContentTemplate = this.TabPageHeaderTemplate,
                Content = tab
            };
            panel.Children.Add(hc);
        }

        /// <summary>
        /// Creates and adds the property panel.
        /// </summary>
        /// <param name="panel">The panel where the property panel should be added.</param>
        /// <param name="pi">The property.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="tab">The tab.</param>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private void AddPropertyPanel(Panel panel, PropertyItem pi, object instance, Tab tab)
        {
            // TODO: refactor this method - too long and complex...
            var propertyPanel = new Grid();
            if (!pi.FillTab)
            {
                propertyPanel.Margin = new Thickness(2);
            }

            var labelColumn = new System.Windows.Controls.ColumnDefinition
            {
                Width = GridLength.Auto,
                MinWidth = this.MinimumLabelWidth,
                MaxWidth = this.MaximumLabelWidth,
                SharedSizeGroup =
                                          this.LabelWidthSharing
                                          != LabelWidthSharing.NotShared
                                              ? "labelColumn"
                                              : null
            };

            propertyPanel.ColumnDefinitions.Add(labelColumn);
            propertyPanel.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            var rd = new System.Windows.Controls.RowDefinition
            {
                Height =
                                 pi.FillTab
                                     ? new GridLength(1, GridUnitType.Star)
                                     : GridLength.Auto
            };
            propertyPanel.RowDefinitions.Add(rd);

            var propertyLabel = this.CreateLabel(pi);
            var propertyControl = this.CreatePropertyControl(pi);
            ContentControl errorControl = null;
            if (propertyControl != null)
            {
                if (!double.IsNaN(pi.Width))
                {
                    propertyControl.Width = pi.Width;
                    propertyControl.HorizontalAlignment = HorizontalAlignment.Left;
                }

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
                    propertyControl.SetBinding(
                        IsEnabledProperty,
                        pi.OptionalDescriptor != null ? new Binding(pi.OptionalDescriptor.Name) : new Binding(pi.Descriptor.Name) { Converter = NullToBoolConverter });
                }

                if (pi.IsEnabledByRadioButton)
                {
                    propertyControl.SetBinding(
                        IsEnabledProperty,
                        new Binding(pi.RadioDescriptor.Name) { Converter = new EnumToBooleanConverter() { EnumType = pi.RadioDescriptor.PropertyType }, ConverterParameter = pi.RadioValue });
                }

                var dataErrorInfoInstance = instance as IDataErrorInfo;
                var notifyDataErrorInfoInstance = instance as INotifyDataErrorInfo;
                if (dataErrorInfoInstance != null || notifyDataErrorInfoInstance != null)
                {
                    if (this.ValidationTemplate != null)
                    {
                        Validation.SetErrorTemplate(propertyControl, this.ValidationTemplate);
                    }

                    if (this.ValidationErrorStyle != null)
                    {
                        propertyControl.Style = this.ValidationErrorStyle;
                    }

                    errorControl = new ContentControl
                    {
                        ContentTemplate = this.ValidationErrorTemplate,
                        Focusable = false
                    };
                    IValueConverter errorConverter = dataErrorInfoInstance != null ?
                        (IValueConverter)new DataErrorInfoConverter(dataErrorInfoInstance, pi.PropertyName) :
                        new NotifyDataErrorInfoConverter(notifyDataErrorInfoInstance, pi.PropertyName);
                    var visibilityBinding = new Binding(pi.PropertyName)
                    {
                        Converter = errorConverter,
                        NotifyOnTargetUpdated = true,
#if !NET40
                        ValidatesOnNotifyDataErrors = false,
#endif
                        // ValidatesOnDataErrors = false,
                        // ValidatesOnExceptions = false
                    };
                    errorControl.SetBinding(VisibilityProperty, visibilityBinding);

                    // When the visibility of the error control is changed, updated the HasErrors of the tab
                    errorControl.TargetUpdated += (s, e) =>
                    {
                        if (dataErrorInfoInstance != null)
                            tab.UpdateHasErrors(dataErrorInfoInstance);
                        else
                            tab.UpdateHasErrors(notifyDataErrorInfoInstance);
                    };

                    var contentBinding = new Binding(pi.PropertyName)
                    {
                        Converter = errorConverter,
#if !NET40
                        ValidatesOnNotifyDataErrors = false,
#endif
                        // ValidatesOnDataErrors = false,
                        // ValidatesOnExceptions = false
                    };
                    errorControl.SetBinding(ContentControl.ContentProperty, contentBinding);

                    // Add a row to the panel
                    propertyPanel.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = GridLength.Auto });
                    propertyPanel.Children.Add(errorControl);
                    Grid.SetRow(errorControl, 1);
                    Grid.SetColumn(errorControl, 1);
                }

                Grid.SetColumn(propertyControl, 1);
            }

            var actualHeaderPlacement = pi.HeaderPlacement;

            var checkBoxPropertyControl = propertyControl as CheckBox;

            if (checkBoxPropertyControl != null)
            {
                if (this.CheckBoxLayout != CheckBoxLayout.Header)
                {
                    checkBoxPropertyControl.Content = propertyLabel;
                    propertyLabel = null;
                }

                if (this.CheckBoxLayout == CheckBoxLayout.CollapseHeader)
                {
                    actualHeaderPlacement = HeaderPlacement.Collapsed;
                }
            }

            switch (actualHeaderPlacement)
            {
                case HeaderPlacement.Hidden:
                    break;

                case HeaderPlacement.Collapsed:
                    {
                        if (propertyControl != null)
                        {
                            Grid.SetColumn(propertyControl, 0);
                            Grid.SetColumnSpan(propertyControl, 2);
                        }

                        break;
                    }

                default:
                    {
                        // create the label panel
                        var labelPanel = new DockPanel();
                        if (pi.HeaderPlacement == HeaderPlacement.Left)
                        {
                            DockPanel.SetDock(labelPanel, Dock.Left);
                        }
                        else
                        {
                            // Above
                            if (propertyControl != null)
                            {
                                propertyPanel.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                                Grid.SetColumnSpan(labelPanel, 2);
                                Grid.SetRow(propertyControl, 1);
                                Grid.SetColumn(propertyControl, 0);
                                Grid.SetColumnSpan(propertyControl, 2);
                                if (errorControl != null)
                                {
                                    Grid.SetRow(errorControl, 2);
                                    Grid.SetColumn(errorControl, 0);
                                    Grid.SetColumnSpan(errorControl, 2);
                                }
                            }
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
                                    VerticalAlignment = VerticalAlignment.Top,
                                    HorizontalAlignment =
                                                                       this.DescriptionIconAlignment
                                };

                                // RenderOptions.SetBitmapScalingMode(descriptionIconImage, BitmapScalingMode.NearestNeighbor);
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
                    }

                    break;
            }

            // add the property control
            if (propertyControl != null)
            {
                propertyPanel.Children.Add(propertyControl);
            }

            // Set the IsEnabled binding of the label
            if (pi.IsEnabledDescriptor != null && propertyLabel != null)
            {
                var isEnabledBinding = new Binding(pi.IsEnabledDescriptor.Name);
                if (pi.IsEnabledValue != null)
                {
                    isEnabledBinding.ConverterParameter = pi.IsEnabledValue;
                    isEnabledBinding.Converter = ValueToBooleanConverter;
                }

                propertyLabel.SetBinding(IsEnabledProperty, isEnabledBinding);
            }

            // Set the IsEnabled binding of the property control
            if (pi.IsEnabledDescriptor != null && propertyControl != null)
            {
                var isEnabledBinding = new Binding(pi.IsEnabledDescriptor.Name);
                if (pi.IsEnabledValue != null)
                {
                    isEnabledBinding.ConverterParameter = pi.IsEnabledValue;
                    isEnabledBinding.Converter = ValueToBooleanConverter;
                }

                var currentBindingExpression = propertyControl.GetBindingExpression(IsEnabledProperty);
                if (currentBindingExpression != null)
                {
                    var multiBinding = new MultiBinding();
                    multiBinding.Bindings.Add(isEnabledBinding);
                    multiBinding.Bindings.Add(currentBindingExpression.ParentBinding);
                    multiBinding.Converter = AllMultiValueConverter;
                    multiBinding.ConverterParameter = true;
                    propertyControl.SetBinding(IsEnabledProperty, multiBinding);
                }
                else
                {
                    propertyControl.SetBinding(IsEnabledProperty, isEnabledBinding);
                }
            }

            if (pi.IsVisibleDescriptor != null)
            {
                var isVisibleBinding = new Binding(pi.IsVisibleDescriptor.Name);
                isVisibleBinding.ConverterParameter = pi.IsVisibleValue;
                isVisibleBinding.Converter = ValueToVisibilityConverter;
                propertyPanel.SetBinding(VisibilityProperty, isVisibleBinding);
            }

            if (this.EnableLabelWidthResizing && pi.HeaderPlacement == HeaderPlacement.Left)
            {
                propertyPanel.Children.Add(
                    new GridSplitter
                    {
                        Width = 4,
                        Background = Brushes.Transparent,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Focusable = false
                    });
            }

            panel.Children.Add(propertyPanel);
        }

        /// <summary>
        /// Creates the label control.
        /// </summary>
        /// <param name="pi">The property item.</param>
        /// <returns>
        /// An element.
        /// </returns>
        private FrameworkElement CreateLabel(PropertyItem pi)
        {
            var indentation = pi.IndentationLevel * this.Indentation;

            FrameworkElement propertyLabel = null;
            if (pi.IsOptional)
            {
                var cb = new CheckBox
                {
                    Content = pi.DisplayName,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5 + indentation, 0, 4, 0)
                };

                cb.SetBinding(
                    ToggleButton.IsCheckedProperty,
                    pi.OptionalDescriptor != null ? new Binding(pi.OptionalDescriptor.Name) : new Binding(pi.Descriptor.Name) { Converter = NullToBoolConverter });

                var g = new Grid();
                g.Children.Add(cb);
                propertyLabel = g;
            }

            if (pi.IsEnabledByRadioButton)
            {
                var rb = new RadioButton
                {
                    Content = pi.DisplayName,
                    GroupName = pi.RadioDescriptor.Name,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5 + indentation, 0, 4, 0)
                };

                var converter = new EnumToBooleanConverter { EnumType = pi.RadioDescriptor.PropertyType };
                rb.SetBinding(
                    ToggleButton.IsCheckedProperty,
                    new Binding(pi.RadioDescriptor.Name) { Converter = converter, ConverterParameter = pi.RadioValue });

                var g = new Grid();
                g.Children.Add(rb);
                propertyLabel = g;
            }

            if (propertyLabel == null)
            {
                propertyLabel = new Label
                {
                    Content = pi.DisplayName,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(indentation, 0, 4, 0)
                };
            }

            return propertyLabel;
        }

        /// <summary>
        /// Creates the property control.
        /// </summary>
        /// <param name="pi">The property item.</param>
        /// <returns>
        /// An element.
        /// </returns>
        private FrameworkElement CreatePropertyControl(PropertyItem pi)
        {
            var options = new PropertyControlFactoryOptions { EnumAsRadioButtonsLimit = this.EnumAsRadioButtonsLimit };
            var control = this.ControlFactory.CreateControl(pi, options);
            if (control != null)
            {
                control.SetValue(AutomationProperties.AutomationIdProperty, pi.PropertyName);
            }

            return control;
        }

        /// <summary>
        /// Handles changes in SelectedObjects.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private void SelectedObjectsChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                if (e.OldValue is INotifyCollectionChanged notifyCollectionChanged)
                {
                    CollectionChangedEventManager.RemoveHandler(notifyCollectionChanged, this.OnSelectedObjectsCollectionChanged);
                }
            }

            if (e.NewValue != null)
            {
                if (e.NewValue is INotifyCollectionChanged notifyCollectionChanged)
                {
                    CollectionChangedEventManager.AddHandler(notifyCollectionChanged, this.OnSelectedObjectsCollectionChanged);
                }
                else if (e.NewValue is IEnumerable enumerable)
                {
                    this.SetCurrentObjectFromSelectedObjects(enumerable);
                }
            }
            else
            {
                this.CurrentObject = null;
                this.UpdateControls();
            }
        }

        /// <summary>
        /// Called when the selected objects collection is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnSelectedObjectsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is IEnumerable enumerable)
            {
                this.SetCurrentObjectFromSelectedObjects(enumerable);
            }
        }

        /// <summary>
        /// Set CurrentObject when SelectedObjects is changed.
        /// </summary>
        /// <param name="enumerable">SelectedObjects.</param>
        private void SetCurrentObjectFromSelectedObjects(IEnumerable enumerable)
        {
            var list = enumerable.Cast<object>().ToList();
            if (list.Count == 0)
            {
                this.CurrentObject = null;
            }
            else if (list.Count == 1)
            {
                this.CurrentObject = list[0];
            }
            else
            {
                this.CurrentObject = new ItemsBag(list);
            }

            this.UpdateControls();
        }

        /// <summary>
        /// Updates the controls.
        /// </summary>
        private void UpdateControls()
        {
            if (this.Operator == null)
            {
                return;
            }

            var oldIndex = this.tabControl != null ? this.tabControl.SelectedIndex : -1;

            var tabs = this.Operator.CreateModel(this.CurrentObject, false, this);
            var tabsArray = tabs != null ? tabs.ToArray() : null;
            var areTabsVisible = this.TabVisibility == TabVisibility.Visible
                                 || (this.TabVisibility == TabVisibility.VisibleIfMoreThanOne && tabsArray != null && tabsArray.Length > 1);
            if (areTabsVisible)
            {
                this.CreateControls(this.CurrentObject, tabsArray);
            }
            else
            {
                this.CreateControlsTabless(this.CurrentObject, tabsArray);
            }

            var newSelectedObjectType = this.CurrentObject != null ? this.CurrentObject.GetType() : null;
            var currentObject = this.CurrentObject as ItemsBag;
            if (currentObject != null)
            {
                newSelectedObjectType = currentObject.BiggestType;
            }

            if (newSelectedObjectType == this.currentSelectedObjectType && this.tabControl != null)
            {
                this.tabControl.SelectedIndex = oldIndex;
            }

            if (this.tabControl != null && this.tabControl.SelectedItem is TabItem)
            {
                this.SelectedTabId = (this.tabControl.SelectedItem as TabItem).Name;
            }

            this.currentSelectedObjectType = newSelectedObjectType;
        }

        /// <summary>
        /// Handles changes of the selected tab.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        private void SelectedTabChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.tabControl == null)
            {
                return;
            }

            var tabId = e.NewValue as string;
            if (tabId == null)
            {
                this.tabControl.SelectedIndex = 0;
                return;
            }

            var tab = this.tabControl.Items.Cast<TabItem>().FirstOrDefault(t => t.Name == tabId);
            if (tab != null)
            {
                this.tabControl.SelectedItem = tab;
            }
        }

        /// <summary>
        /// Handles changes of the selected tab index.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        private void SelectedTabIndexChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.tabControl == null)
            {
                return;
            }

            int tabIndex;
            int.TryParse(e.NewValue.ToString(), out tabIndex);
            if (tabIndex >= 0)
            {
                var tabItems = this.tabControl.Items.Cast<TabItem>().ToArray();
                if (tabIndex < tabItems.Length)
                {
                    this.SelectedTabId = tabItems[tabIndex].Name;
                }
            }
        }
    }
}
