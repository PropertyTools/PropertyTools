// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyDialogCancelEventArgs.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a custom CancelEventArgs for <see cref="PropertyDialog"/>'s Closing event
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
	using System.ComponentModel;

    /// <summary>
    /// Represents a custom CancelEventArgs for <see cref="PropertyDialog"/>'s Closing event
    /// </summary>
	public class PropertyDialogCancelEventArgs : CancelEventArgs
	{
		public bool? DialogResult { get; }

		/// <summary>
		/// Instance currently being editing in PropertyDialog.
		/// </summary>		
		public object EditingContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDialogCancelEventArgs" /> class.
        /// </summary>
        /// <param name="editingContext">
        /// The instance currently being editing in PropertyDialog.
        /// </param>
        public PropertyDialogCancelEventArgs(object editingContext, bool? dialogResult)
		{
			this.EditingContext = editingContext;
			this.DialogResult = dialogResult;
		}
	}
}