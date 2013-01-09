// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabViewModel.cs" company="PropertyTools">
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
//   ViewModel for the tabs.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// ViewModel for the tabs.
    /// </summary>
    public class TabViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabViewModel"/> class.
        /// </summary>
        /// <param name="tabName">
        /// The tab name.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public TabViewModel(string tabName, PropertyEditor owner)
            : base(owner)
        {
            this.Name = this.Header = tabName;
            this.Categories = new List<CategoryViewModel>();
        }

        /// <summary>
        /// Gets Categories.
        /// </summary>
        public List<CategoryViewModel> Categories { get; private set; }

        /// <summary>
        /// Gets CategoryTemplateSelector.
        /// </summary>
        public CategoryTemplateSelector CategoryTemplateSelector
        {
            get
            {
                return this.Owner.CategoryTemplateSelector;
            }
        }

        /// <summary>
        /// Gets a value indicating whether HasErrors.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                foreach (var cat in this.Categories)
                {
                    foreach (var prop in cat.Properties)
                    {
                        if (prop.PropertyError != null)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether HasWarnings.
        /// </summary>
        public bool HasWarnings
        {
            get
            {
                foreach (var cat in this.Categories)
                {
                    foreach (var prop in cat.Properties)
                    {
                        if (prop.PropertyWarning != null)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets or sets Icon.
        /// </summary>
        public ImageSource Icon { get; set; }

        /// <summary>
        /// Gets IconVisibility.
        /// </summary>
        public Visibility IconVisibility
        {
            get
            {
                return this.Icon != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sort.
        /// </summary>
        public void Sort()
        {
            this.Categories = this.Categories.OrderBy(c => c.SortIndex).ToList();
        }

        /// <summary>
        /// The update error info.
        /// </summary>
        public void UpdateErrorInfo()
        {
            this.NotifyPropertyChanged("HasErrors");
            this.NotifyPropertyChanged("HasWarnings");
        }

    }
}