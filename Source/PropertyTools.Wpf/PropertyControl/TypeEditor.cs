// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeEditor.cs" company="PropertyTools">
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
//   Define a data template for the specified type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;

    /// <summary>
    /// Define a data template for the specified type.
    /// </summary>
    public class TypeEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeEditor" /> class.
        /// </summary>
        public TypeEditor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeEditor"/> class.
        /// </summary>
        /// <param name="editedType">
        /// Type to edit.
        /// </param>
        /// <param name="editorTemplate">
        /// The data template.
        /// </param>
        public TypeEditor(Type editedType, DataTemplate editorTemplate)
        {
            this.EditedType = editedType;
            this.EditorTemplate = editorTemplate;
        }

        /// <summary>
        /// Gets or sets the type to edit.
        /// </summary>
        public Type EditedType { get; set; }

        /// <summary>
        /// Gets or sets template for this type.
        /// </summary>
        public DataTemplate EditorTemplate { get; set; }

        /// <summary>
        /// Determines whether the specified type is assignable to the EditedType.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified type is assignable; otherwise, <c>false</c> .
        /// </returns>
        public bool IsAssignable(Type type)
        {
            return this.EditedType.IsAssignableFrom(type);
        }
    }
}