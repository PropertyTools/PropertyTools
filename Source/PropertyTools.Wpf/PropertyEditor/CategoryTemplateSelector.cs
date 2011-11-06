// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryTemplateSelector.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The CategoryTemplateSelector is used to select a DataTemplate for the categories
    /// </summary>
    public class CategoryTemplateSelector : DataTemplateSelector
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryTemplateSelector"/> class.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public CategoryTemplateSelector(PropertyEditor owner)
        {
            this.Owner = owner;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Owner.
        /// </summary>
        public PropertyEditor Owner { get; private set; }

        /// <summary>
        /// Gets or sets TemplateOwner.
        /// </summary>
        public FrameworkElement TemplateOwner { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The select template.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var category = item as CategoryViewModel;
            if (category == null)
            {
                throw new ArgumentException("item must be of type CategoryViewModel");
            }

            var key = "CategoryGroupBoxTemplate";
            if (this.Owner.ShowCategoriesAs == ShowCategoriesAs.Expander)
            {
                key = "CategoryExpanderTemplate";
            }

            if (this.Owner.ShowCategoriesAs == ShowCategoriesAs.Header)
            {
                key = "CategoryHeaderTemplate";
            }

            var template = TryToFindDataTemplate(this.TemplateOwner, key);

            return template;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The try to find data template.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="dataTemplateKey">
        /// The data template key.
        /// </param>
        /// <returns>
        /// </returns>
        private static DataTemplate TryToFindDataTemplate(FrameworkElement element, object dataTemplateKey)
        {
            object dataTemplate = element.TryFindResource(dataTemplateKey);
            if (dataTemplate == null)
            {
                dataTemplateKey = new ComponentResourceKey(typeof(PropertyEditor), dataTemplateKey);
                dataTemplate = element.TryFindResource(dataTemplateKey);
            }

            return dataTemplate as DataTemplate;
        }

        #endregion
    }
}