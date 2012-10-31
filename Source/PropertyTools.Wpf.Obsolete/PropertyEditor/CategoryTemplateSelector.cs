// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryTemplateSelector.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The CategoryTemplateSelector is used to select a DataTemplate for the categories
// </summary>
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

        /// <summary>
        /// Gets Owner.
        /// </summary>
        public PropertyEditor Owner { get; private set; }

        /// <summary>
        /// Gets or sets TemplateOwner.
        /// </summary>
        public FrameworkElement TemplateOwner { get; set; }

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

    }
}