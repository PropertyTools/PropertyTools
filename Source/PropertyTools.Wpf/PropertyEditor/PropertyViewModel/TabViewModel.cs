// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabViewModel.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
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
        #region Constructors and Destructors

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

        #endregion

        #region Public Properties

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

        #endregion

        #region Public Methods

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

        #endregion
    }
}