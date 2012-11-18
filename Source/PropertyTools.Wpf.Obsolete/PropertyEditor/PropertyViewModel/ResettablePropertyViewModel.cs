// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResettablePropertyViewModel.cs" company="PropertyTools">
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
//   Properties that are marked [resettable(...)] have a reset button
// </summary>
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
        /// <summary>
        /// The instance.
        /// </summary>
        private readonly object instance;

        /// <summary>
        /// The resettable descriptor.
        /// </summary>
        private readonly PropertyDescriptor resettableDescriptor;

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

        /// <summary>
        /// Gets or sets Label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets BrowseCommand.
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

    }
}