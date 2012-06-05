using System.ComponentModel;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Properties marked [Optional(...)] are enabled/disabled by a checkbox
    /// </summary>
    public class OptionalProperty : Property
    {
        public OptionalProperty(object instance, PropertyDescriptor descriptor, string optionalPropertyName, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
            OptionalPropertyName = optionalPropertyName;
        }

        #region Optional

        private string optionalPropertyName;
        public string OptionalPropertyName
        {
            get
            {
                return optionalPropertyName;
            }
            set
            {
                if (optionalPropertyName != value)
                {
                    optionalPropertyName = value;
                    NotifyPropertyChanged("OptionalPropertyName");
                }
            }
        }

        public bool IsOptionalChecked
        {
            get
            {
                if (!string.IsNullOrEmpty(OptionalPropertyName))
                    return (bool)PropertyHelper.GetProperty(Instance, OptionalPropertyName);
                return true; // default must be true (enable editor)
            }
            set
            {
                if (!string.IsNullOrEmpty(OptionalPropertyName))
                {
                    PropertyHelper.SetProperty(Instance, OptionalPropertyName, value);
                    NotifyPropertyChanged("IsOptionalChecked");
                }
            }
        }

        #endregion


    }
}