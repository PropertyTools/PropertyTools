// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadioButtonList.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a control that shows a list of radio buttons.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using PropertyTools.Wpf.Common;

    /// <summary>
    /// Represents a control that shows a list of radio buttons.
    /// </summary>
    [TemplatePart(Name = PartPanel, Type = typeof(StackPanel))]
    public class RadioButtonSelector : Control, ISelectorDefinition
    {
        /// <summary>
        /// Identifies the <see cref="ItemMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
            nameof(ItemMargin),
            typeof(Thickness),
            typeof(RadioButtonSelector),
            new UIPropertyMetadata(new Thickness(0, 4, 0, 4)));

        /// <summary>
        /// Identifies the <see cref="ItemPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(
            nameof(ItemPadding),
            typeof(Thickness),
            typeof(RadioButtonSelector),
            new UIPropertyMetadata(new Thickness(4, 0, 0, 0)));

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation),
            typeof(Orientation),
            typeof(RadioButtonSelector),
            new UIPropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(object),
            typeof(RadioButtonSelector),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChanged));

        /// <summary>
        /// The part panel.
        /// </summary>
        protected const string PartPanel = "PART_Panel";

        /// <summary>
        /// The panel.
        /// </summary>
        protected StackPanel panel;

        /// <summary>
        /// Initializes static members of the <see cref="RadioButtonSelector" /> class.
        /// </summary>
        static RadioButtonSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RadioButtonSelector), new FrameworkPropertyMetadata(typeof(RadioButtonSelector)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButtonSelector" /> class.
        /// </summary>
        public RadioButtonSelector()
        {
            this.DataContextChanged += this.HandleDataContextChanged;
        }


        /// <summary>
        /// Gets or sets the item margin.
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
        /// Gets or sets the item padding.
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
        /// Gets or sets the orientation.
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
        /// Gets or sets the value.
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

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see
        /// cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> .
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (this.panel == null)
            {
                this.panel = this.Template.FindName(PartPanel, this) as StackPanel;
            }

            this.UpdateContent();
        }

        /// <summary>
        /// Called when the <see cref="Value" /> has changed or the <see cref="EnumType" /> has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected static void ValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((RadioButtonSelector)sender).UpdateContent();
        }

        /// <summary>
        /// Handles data context changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.UpdateContent();
        }


        /// <summary>
        /// Updates the content.
        /// </summary>
        protected virtual void UpdateContent()
        {
            if (this.panel == null)
            {
                return;
            }

            this.panel.Children.Clear();

            IEnumerable itemValues = PopulateItems();
            if (itemValues == null)
            {
                return;
            }

            var converter = new SelectorItemToBooleanConverter() { SelectorDefinition = this };

            foreach (var itemValue in itemValues)
            {
                object content;                
                if (itemValue == null || !ReflectionExtensions.TryGetFieldOrPropertyValue(itemValue, this.DisplayMemberPath, out content))
                {
                    content = "-";
                }

                var rb = new RadioButton
                {
                    Content = content,
                    Padding = this.ItemPadding,
                };

                var isCheckedBinding = new Binding(nameof(this.Value))
                {
                    Converter = converter,
                    ConverterParameter = itemValue,
                    Source = this,
                    Mode = BindingMode.TwoWay
                };

                rb.SetBinding(ToggleButton.IsCheckedProperty, isCheckedBinding);

                rb.SetBinding(MarginProperty, new Binding(nameof(this.ItemMargin)) { Source = this });

                this.panel.Children.Add(rb);
            }
        }

        protected virtual IEnumerable PopulateItems()
        {
            IEnumerable itemValues = null;

            if (this.ItemsSource != null)
            {
                itemValues = this.ItemsSource;
            }
            else if (this.ItemsSourceProperty != null)
            {
                var instance = this.DataContext;
                if (instance != null)
                {
                    // use instance.GetType to be able to fetch static properties also
                    var p = instance.GetType().GetProperties().FirstOrDefault(x => x.Name == this.ItemsSourceProperty);
                    itemValues = p?.GetValue(instance) as IEnumerable;
                }
            }

            return itemValues;
        }

        #region ISelectorDefinition

        /// <inheritdoc/>        
        public string ItemsSourceProperty {get;set;}
        
        /// <inheritdoc/>
        public IEnumerable ItemsSource { get; set; }
        
        /// <inheritdoc/>
        public string SelectedValuePath { get; set; }
        
        /// <inheritdoc/>
        public string DisplayMemberPath { get; set; }
        
        /// <inheritdoc/>
        public bool DisplayTextForNullItem { get; set; }

        #endregion
    }
}
