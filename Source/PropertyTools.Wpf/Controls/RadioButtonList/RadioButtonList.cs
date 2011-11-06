// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadioButtonList.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    /// <summary>
    /// Represents a control that shows a list of radiobuttons.
    /// </summary>
    /// <remarks>
    /// See http://bea.stollnitz.com/blog/?p=28 and http://code.msdn.microsoft.com/wpfradiobuttonlist
    /// </remarks>
    [TemplatePart(Name = PartPanel, Type = typeof(StackPanel))]
    public class RadioButtonList : Control
    {
        #region Constants and Fields

        /// <summary>
        /// The enum type property.
        /// </summary>
        public static readonly DependencyProperty EnumTypeProperty = DependencyProperty.Register(
            "EnumType", typeof(Type), typeof(RadioButtonList), new UIPropertyMetadata(null, ValueChanged));

        /// <summary>
        /// The item margin property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
            "ItemMargin", typeof(Thickness), typeof(RadioButtonList), new UIPropertyMetadata(new Thickness(0, 4, 0, 4)));

        /// <summary>
        /// The item padding property.
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(
            "ItemPadding", typeof(Thickness), typeof(RadioButtonList), new UIPropertyMetadata(new Thickness(4, 0, 0, 0)));

        /// <summary>
        /// The orientation property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(RadioButtonList), new UIPropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// The value property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", 
            typeof(object), 
            typeof(RadioButtonList), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChanged));

        /// <summary>
        /// The part panel.
        /// </summary>
        private const string PartPanel = "PART_Panel";

        /// <summary>
        /// The panel.
        /// </summary>
        private StackPanel panel;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="RadioButtonList"/> class. 
        /// </summary>
        static RadioButtonList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RadioButtonList), new FrameworkPropertyMetadata(typeof(RadioButtonList)));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "RadioButtonList" /> class.
        /// </summary>
        public RadioButtonList()
        {
            this.DataContextChanged += this.RadioButtonListDataContextChanged;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the type of the enum.
        /// </summary>
        /// <value>The type of the enum.</value>
        public Type EnumType
        {
            get
            {
                return (Type)this.GetValue(EnumTypeProperty);
            }

            set
            {
                this.SetValue(EnumTypeProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the item margin.
        /// </summary>
        /// <value>The item margin.</value>
        public Thickness ItemMargin
        {
            get
            {
                return (Thickness)this.GetValue(ItemMarginProperty);
            }

            set
            {
                this.SetValue(ItemMarginProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the item padding.
        /// </summary>
        /// <value>The item padding.</value>
        public Thickness ItemPadding
        {
            get
            {
                return (Thickness)this.GetValue(ItemPaddingProperty);
            }

            set
            {
                this.SetValue(ItemPaddingProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get
            {
                return (Orientation)this.GetValue(OrientationProperty);
            }

            set
            {
                this.SetValue(OrientationProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get
            {
                return this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (this.panel == null)
            {
                this.panel = this.Template.FindName(PartPanel, this) as StackPanel;
            }

            this.UpdateContent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The value changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RadioButtonList)d).UpdateContent();
        }

        /// <summary>
        /// The radio button list data context changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RadioButtonListDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.UpdateContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        private void UpdateContent()
        {
            if (this.panel == null)
            {
                return;
            }

            this.panel.Children.Clear();

            var enumType = this.EnumType;
            if (enumType != null)
            {
                var ult = Nullable.GetUnderlyingType(enumType);
                if (ult != null)
                {
                    enumType = ult;
                }
            }

            if (this.Value != null)
            {
                enumType = this.Value.GetType();
            }

            if (enumType == null || !typeof(Enum).IsAssignableFrom(enumType))
            {
                return;
            }

            var enumValues = Enum.GetValues(enumType).FilterOnBrowsableAttribute();
            var converter = new EnumToBooleanConverter { EnumType = enumType };
            var descriptionConverter = new EnumDescriptionConverter();

            foreach (var itemValue in enumValues)
            {
                var rb = new RadioButton
                    {
                        Content =
                            descriptionConverter.Convert(itemValue, typeof(string), null, CultureInfo.CurrentCulture), 
                        Padding = this.ItemPadding
                    };

                var isCheckedBinding = new Binding("Value")
                    {
                       Converter = converter, ConverterParameter = itemValue, Source = this, Mode = BindingMode.TwoWay 
                    };
                rb.SetBinding(ToggleButton.IsCheckedProperty, isCheckedBinding);

                var itemMarginBinding = new Binding("ItemMargin") { Source = this };
                rb.SetBinding(MarginProperty, itemMarginBinding);

                this.panel.Children.Add(rb);
            }
        }

        #endregion
    }
}