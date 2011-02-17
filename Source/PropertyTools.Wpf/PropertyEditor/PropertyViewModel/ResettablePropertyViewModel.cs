using System.ComponentModel;
using System.Windows.Input;

namespace PropertyTools.Wpf
{
    /// <summary>
    ///   Properties that are marked [resettable(...)] have a reset button
    /// </summary>
    public class ResettablePropertyViewModel : PropertyViewModel
    {
        private readonly object instance;
        private readonly PropertyDescriptor resettableDescriptor;

        public ResettablePropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
            this.instance = instance;
            resettableDescriptor = descriptor;
            ResetCommand = new DelegateCommand(ExecuteReset);

            var resettableAttr = AttributeHelper.GetFirstAttribute<ResettableAttribute>(resettableDescriptor);

            if (resettableAttr != null)
            {
                Label = (string) resettableAttr.ButtonLabel;
            }
        }

        public string ResettablePropertyName
        {
            get
            {
                if (resettableDescriptor != null)
                {
                    return resettableDescriptor.Name;
                }

                return null;
            }
        }

        public string Label { get; set; }

        /// <summary>
        ///   Gets or sets BrowseCommand.
        /// </summary>
        public ICommand ResetCommand { get; set; }

        public void ExecuteReset()
        {
            var reset = instance as IResettableProperties;

            if (reset != null)
            {
                Value = reset.GetResetValue(resettableDescriptor.Name);
            }
        }
    }
}