// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyDialogCancelEventArgs.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a property editing dialog.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
	using System.ComponentModel;

	public class PropertyDialogCancelEventArgs : CancelEventArgs
	{
		public bool? DialogResult { get; }

		/// <summary>
		/// Instance currently being editing in PropertyDialog.
		/// </summary>		
		public object EditingContext { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="editingContext">
		/// Instance currently being editing in PropertyDialog.
		/// </param>
		public PropertyDialogCancelEventArgs(object editingContext, bool? dialogResult)
		{
			this.EditingContext = editingContext;
			this.DialogResult = dialogResult;
		}
	}
}