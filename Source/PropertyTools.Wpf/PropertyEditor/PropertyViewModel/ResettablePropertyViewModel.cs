// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResettablePropertyViewModel.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.Windows.Input;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Properties that are marked [resettable(...)] have a reset button
    /// </summary>
    public class ResettablePropertyViewModel : PropertyViewModel
    {
        #region Constants and Fields

        /// <summary>
        /// The instance.
        /// </summary>
        private readonly object instance;

        /// <summary>
        /// The resettable descriptor.
        /// </summary>
        private readonly PropertyDescriptor resettableDescriptor;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResettablePropertyViewModel"/> class.
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
        public ResettablePropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
            this.instance = instance;
            this.resettableDescriptor = descriptor;
            this.ResetCommand = new DelegateCommand(this.ExecuteReset);

            var resettableAttr = AttributeHelper.GetFirstAttribute<ResettableAttribute>(this.resettableDescriptor);

            if (resettableAttr != null)
            {
                this.Label = (string)resettableAttr.ButtonLabel;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///   Gets or sets BrowseCommand.
        /// </summary>
        public ICommand ResetCommand { get; set; }

        /// <summary>
        /// Gets ResettablePropertyName.
        /// </summary>
        public string ResettablePropertyName
        {
            get
            {
                if (this.resettableDescriptor != null)
                {
                    return this.resettableDescriptor.Name;
                }

                return null;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The execute reset.
        /// </summary>
        public void ExecuteReset()
        {
            var reset = this.instance as IResettableProperties;

            if (reset != null)
            {
                this.Value = reset.GetResetValue(this.resettableDescriptor.Name);
            }
        }

        #endregion
    }
}