// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeEditor.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines a data template for the specified type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;

    /// <summary>
    /// Defines a data template for the specified type.
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
        /// Initializes a new instance of the <see cref="TypeEditor" /> class.
        /// </summary>
        /// <param name="editedType">Type to edit.</param>
        /// <param name="editorTemplate">The data template.</param>
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
        /// <param name="type">The type.</param>
        /// <returns>
        /// <c>true</c> if the specified type is assignable; otherwise, <c>false</c> .
        /// </returns>
        public bool IsAssignable(Type type)
        {
            return this.EditedType.IsAssignableFrom(type);
        }
    }
}