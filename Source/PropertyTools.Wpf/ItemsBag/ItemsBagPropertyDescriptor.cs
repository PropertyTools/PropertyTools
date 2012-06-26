// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsBagPropertyDescriptor.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Provides a property descriptor for an object in the <see cref="ItemsBag"/>.
    /// </summary>
    public class ItemsBagPropertyDescriptor : PropertyDescriptor
    {
        #region Constants and Fields

        /// <summary>
        ///   The default descriptor.
        /// </summary>
        private readonly PropertyDescriptor defaultDescriptor;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsBagPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="defaultDescriptor">
        ///   The default descriptor.
        /// </param>
        /// <param name="biggestType"></param>
        public ItemsBagPropertyDescriptor(PropertyDescriptor defaultDescriptor, Type componentType)
            : base(defaultDescriptor)
        {
            this.defaultDescriptor = defaultDescriptor;
            this.componentType = componentType;
        }

        private Type componentType;

        #endregion

        #region Public Properties

        /// <summary>
        ///   When overridden in a derived class, gets the type of the component this property is bound to.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref = "T:System.Type" /> that represents the type of component this property is bound to. When the <see
        ///    cref = "M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)" /> or <see
        ///    cref = "M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)" /> methods are invoked, the object specified might be an instance of this type.</returns>
        public override Type ComponentType
        {
            get
            {
                return componentType;
            }
        }

        /// <summary>
        ///   When overridden in a derived class, gets a value indicating whether this property is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the property is read-only; otherwise, false.</returns>
        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///   When overridden in a derived class, gets the type of the property.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref = "T:System.Type" /> that represents the type of the property.</returns>
        public override Type PropertyType
        {
            get
            {
                if (this.defaultDescriptor.PropertyType.IsValueType)
                {
                    var nt = this.GetNullableType(this.defaultDescriptor.PropertyType);
                    return nt;
                }

                return this.defaultDescriptor.PropertyType;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// When overridden in a derived class, returns whether resetting an object changes its value.
        /// </summary>
        /// <param name="component">
        /// The component to test for reset capability.
        /// </param>
        /// <returns>
        /// true if resetting the component changes its value; otherwise, false.
        /// </returns>
        public override bool CanResetValue(object component)
        {
            return false;
        }

        /// <summary>
        /// When overridden in a derived class, gets the current value of the property on a component.
        /// </summary>
        /// <param name="component">
        /// The component with the property for which to retrieve the value.
        /// </param>
        /// <returns>
        /// The value of a property for a given component.
        /// </returns>
        public override object GetValue(object component)
        {
            var bag = (ItemsBag)component;
            object value = null;
            bool isFirst = true;
            foreach (var obj in bag.Objects)
            {
                var type = obj.GetType();
                var pi = type.GetProperty(this.Name);
                if (pi == null)
                {
                    continue;
                }

                var itemValue = pi.GetValue(obj, null);
                if (value != null && !value.Equals(itemValue))
                {
                    value = null;
                }

                if (isFirst)
                {
                    value = itemValue;
                    isFirst = false;
                }
            }

            return value;
        }

        /// <summary>
        /// When overridden in a derived class, resets the value for this property of the component to the default value.
        /// </summary>
        /// <param name="component">
        /// The component with the property value that is to be reset to the default value.
        /// </param>
        public override void ResetValue(object component)
        {
        }

        /// <summary>
        /// When overridden in a derived class, sets the value of the component to a different value.
        /// </summary>
        /// <param name="component">
        /// The component with the property value that is to be set.
        /// </param>
        /// <param name="value">
        /// The new value.
        /// </param>
        public override void SetValue(object component, object value)
        {
            var bag = (ItemsBag)component;
            bag.SuspendNotifications = true;
            foreach (var obj in bag.Objects)
            {
                var type = obj.GetType();
                var pi = type.GetProperty(this.Name);
                pi.SetValue(obj, value, null);
            }

            bag.RaisePropertyChanged(this.Name);
            bag.SuspendNotifications = false;
        }

        /// <summary>
        /// When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.
        /// </summary>
        /// <param name="component">
        /// The component with the property to be examined for persistence.
        /// </param>
        /// <returns>
        /// true if the property should be persisted; otherwise, false.
        /// </returns>
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the type of the nullable.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// </returns>
        private Type GetNullableType(Type type)
        {
            // http://stackoverflow.com/questions/108104/how-do-i-convert-a-system-type-to-its-nullable-version
            // Use Nullable.GetUnderlyingType() to remove the Nullable<T> wrapper if type is already nullable.
            // type = Nullable.GetUnderlyingType(type);
            // if (type.IsValueType)
            return typeof(Nullable<>).MakeGenericType(type);

            // else
            // return type;
        }

        #endregion
    }
}