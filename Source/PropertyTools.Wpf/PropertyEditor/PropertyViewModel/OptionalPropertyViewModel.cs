// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionalPropertyViewModel.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Properties that are nullable or marked [Optional(...)] are enabled/disabled by a checkbox
    /// </summary>
    public class OptionalPropertyViewModel : PropertyViewModel
    {
        #region Constants and Fields

        /// <summary>
        ///   The optional descriptor.
        /// </summary>
        private readonly PropertyDescriptor optionalDescriptor;

        /// <summary>
        ///   The enabled but has no value.
        /// </summary>
        private bool enabledButHasNoValue;

        /// <summary>
        ///   The previous value.
        /// </summary>
        private object previousValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalPropertyViewModel"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="optionalPropertyName">
        /// The optional property name.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public OptionalPropertyViewModel(
            object instance, PropertyDescriptor descriptor, string optionalPropertyName, PropertyEditor owner)
            : this(instance, descriptor, GetDescriptor(instance, optionalPropertyName), owner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalPropertyViewModel"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="optionalDescriptor">
        /// The optional descriptor.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public OptionalPropertyViewModel(
            object instance, PropertyDescriptor descriptor, PropertyDescriptor optionalDescriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
            this.optionalDescriptor = optionalDescriptor;

            // http://msdn.microsoft.com/en-us/library/ms366789.aspx
            this.IsPropertyNullable = descriptor.PropertyType.IsGenericType
                                      && descriptor.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            this.IsPropertyNullable = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether IsOptionalChecked.
        /// </summary>
        public bool IsOptionalChecked
        {
            get
            {
                if (this.optionalDescriptor != null)
                {
                    return (bool)this.optionalDescriptor.GetValue(this.FirstInstance);
                }

                if (this.IsPropertyNullable)
                {
                    return this.enabledButHasNoValue || this.Value != null;
                }

                return true; // default value is true (enable editor)
            }

            set
            {
                if (this.optionalDescriptor != null)
                {
                    this.optionalDescriptor.SetValue(this.FirstInstance, value);
                    this.NotifyPropertyChanged("IsOptionalChecked");
                    return;
                }

                if (this.IsPropertyNullable)
                {
                    if (value)
                    {
                        this.Value = this.previousValue;
                        this.enabledButHasNoValue = true;
                    }
                    else
                    {
                        this.previousValue = this.Value;
                        this.Value = null;
                        this.enabledButHasNoValue = false;
                    }

                    this.NotifyPropertyChanged("IsOptionalChecked");
                    return;
                }
            }
        }

        /// <summary>
        ///   Gets a value indicating whether IsPropertyNullable.
        /// </summary>
        public bool IsPropertyNullable { get; private set; }

        /// <summary>
        ///   Gets OptionalPropertyName.
        /// </summary>
        public string OptionalPropertyName
        {
            get
            {
                if (this.optionalDescriptor != null)
                {
                    return this.optionalDescriptor.Name;
                }

                return null;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The subscribe value changed.
        /// </summary>
        public override void SubscribeValueChanged()
        {
            base.SubscribeValueChanged();
            if (this.optionalDescriptor != null)
            {
                this.SubscribeValueChanged(this.optionalDescriptor, this.IsOptionalChanged);
            }
        }

        /// <summary>
        /// The unsubscribe value changed.
        /// </summary>
        public override void UnsubscribeValueChanged()
        {
            base.UnsubscribeValueChanged();
            if (this.optionalDescriptor != null)
            {
                this.UnsubscribeValueChanged(this.optionalDescriptor, this.IsOptionalChanged);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get descriptor.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// </returns>
        private static PropertyDescriptor GetDescriptor(object instance, string propertyName)
        {
            if (instance == null || propertyName == null)
            {
                return null;
            }

            return TypeDescriptor.GetProperties(instance).Find(propertyName, false);
        }

        /// <summary>
        /// The is optional changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void IsOptionalChanged(object sender, EventArgs e)
        {
            this.NotifyPropertyChanged("IsOptionalChecked");
        }

        #endregion
    }
}