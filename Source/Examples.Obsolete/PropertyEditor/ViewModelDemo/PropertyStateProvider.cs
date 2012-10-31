// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyStateProvider.cs" company="PropertyTools">
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
//   Providing default property states, using IDataErrorInfo for property error message
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel;
using PropertyTools.Wpf;

namespace ViewModelDemo
{
    /// <summary>
    /// Providing default property states, using IDataErrorInfo for property error message
    /// </summary>
    public class PropertyStateProvider : IPropertyStateProvider
    {
        public bool IsEnabled(object component, PropertyDescriptor descriptor)
        {
            return true;
        }

        public bool IsVisible(object component, PropertyDescriptor descriptor)
        {
            return true;
        }

        public string GetError(object component, PropertyDescriptor descriptor)
        {
            var dei = component as IDataErrorInfo;
            if (dei != null)
                return dei[descriptor.Name];
            return null;
        }

        public string GetWarning(object component, PropertyDescriptor descriptor)
        {
            return null; //
        }
    }
}