// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionalProperty.cs" company="PropertyTools">
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
//   Properties marked [Optional(...)] are enabled/disabled by a checkbox
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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

    }
}