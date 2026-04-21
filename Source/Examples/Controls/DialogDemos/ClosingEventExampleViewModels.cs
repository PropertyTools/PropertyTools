using System.ComponentModel;

namespace DialogDemos
{
	/// <remarks>
	/// Properties are not needed for OnClosingEvent example #1
	/// </remarks>
	public class ClosingEventExampleViewModels : PropertyTools.Observable
	{ 
	}

	public class ClosingEventExample2ViewModel : PropertyTools.Observable
	{
		private bool _cancelOnClosingFlag;

		[Category("Flags|General")]
		[DisplayName("Cancel On Closing")]
		public bool CancelOnClosingFlag
		{
			get
			{
				return _cancelOnClosingFlag;
			}
			set
			{
				base.SetValue(ref _cancelOnClosingFlag, value);
			}
		}
	}


	public class ClosingEventExample3ViewModel : PropertyTools.Observable, IEditableObject
	{
		private string _username;

		[Category("General|Credentials")]
		[DisplayName("Username")]
		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				base.SetValue(ref _username, value);
			}
		}

		#region IEditableObject

		private ClosingEventExample3ViewModel _backup;
		private bool _isEditing;

		public void BeginEdit()
		{
			if (_isEditing) return;
			_isEditing = true;
			// Simple snapshot: copy current values
			_backup = new ClosingEventExample3ViewModel { Username = this.Username };
		}

		public void EndEdit()
		{
			if (!_isEditing) return;
			_isEditing = false;
			_backup = null; // Discard backup
		}

		public void CancelEdit()
		{
			if (!_isEditing) return;
			_isEditing = false;
			// Restore from backup
			this.Username = _backup.Username;
		}

		#endregion
	}
}