// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SlidablePropertyViewModel.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Properties marked [Slidable] are using a slider
    /// </summary>
    public class SlidablePropertyViewModel : PropertyViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidablePropertyViewModel"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public SlidablePropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets DoubleValue.
        /// </summary>
        public double DoubleValue
        {
            get
            {
                if (this.Value == null)
                {
                    return 0;
                }

                var t = this.Value.GetType();
                if (t == typeof(int))
                {
                    var i = (int)this.Value;
                    return i;
                }

                if (t == typeof(double))
                {
                    return (double)this.Value;
                }

                if (t == typeof(float))
                {
                    return (float)this.Value;
                }

                return 0;
            }

            set
            {
                this.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets SliderLargeChange.
        /// </summary>
        public double SliderLargeChange { get; set; }

        /// <summary>
        /// Gets or sets SliderMaximum.
        /// </summary>
        public double SliderMaximum { get; set; }

        /// <summary>
        /// Gets or sets SliderMinimum.
        /// </summary>
        public double SliderMinimum { get; set; }

        /// <summary>
        /// Gets or sets SliderSmallChange.
        /// </summary>
        public double SliderSmallChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SliderSnapToTicks.
        /// </summary>
        public bool SliderSnapToTicks { get; set; }

        /// <summary>
        /// Gets or sets SliderTickFrequency.
        /// </summary>
        public double SliderTickFrequency { get; set; }

        /// <summary>
        /// Gets or sets SliderTickPlacement.
        /// </summary>
        public TickPlacement SliderTickPlacement { get; set; }

        #endregion
    }
}