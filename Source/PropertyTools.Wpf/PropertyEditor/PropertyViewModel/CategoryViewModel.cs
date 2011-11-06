// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryViewModel.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// ViewModel for categories.
    ///   The categories can be shown as GroupBox, Expander or by the Header.
    /// </summary>
    public class CategoryViewModel : ViewModelBase
    {
        #region Constants and Fields

        /// <summary>
        /// The is enabled.
        /// </summary>
        private bool isEnabled = true;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryViewModel"/> class.
        /// </summary>
        /// <param name="categoryName">
        /// The category name.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public CategoryViewModel(string categoryName, PropertyEditor owner)
            : base(owner)
        {
            this.Name = this.Header = categoryName;
            this.Properties = new List<PropertyViewModel>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether IsEnabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }

            set
            {
                this.isEnabled = value;
                this.NotifyPropertyChanged("IsEnabled");
            }
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets Properties.
        /// </summary>
        public List<PropertyViewModel> Properties { get; private set; }

        /// <summary>
        /// Gets Visibility.
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                return Visibility.Visible;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The sort.
        /// </summary>
        public void Sort()
        {
            this.Properties = this.Properties.OrderBy(p => p.SortIndex).ToList();
        }

        #endregion
    }
}