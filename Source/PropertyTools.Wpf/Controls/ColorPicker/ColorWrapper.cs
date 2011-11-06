// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorWrapper.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Controls.ColorPicker
{
    using System.ComponentModel;
    using System.Windows.Media;

    /// <summary>
    /// Represents a color.
    /// </summary>
    /// <remarks>
    /// Wrapper class for colors - needed to get unique items in the persistent color list
    ///   since Color.XXX added in multiple positions results in multiple items being selected.
    ///   Also needed to implement the INotifyPropertyChanged for binding support.
    /// </remarks>
    public class ColorWrapper : INotifyPropertyChanged
    {
        #region Constants and Fields

        /// <summary>
        /// The color.
        /// </summary>
        private Color color;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorWrapper"/> class.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        public ColorWrapper(Color c)
        {
            this.Color = c;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Color.
        /// </summary>
        public Color Color
        {
            get
            {
                return this.color;
            }

            set
            {
                this.color = value;
                this.OnPropertyChanged("Color");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}