namespace DialogDemos
{
    using System.ComponentModel;

    /// <remarks>
    /// Properties are not needed for OnClosingEvent example #1
    /// </remarks>
    public class ClosingEventExampleViewModels : PropertyTools.Observable
	{ 
	}

	public class ClosingEventExample2ViewModel : PropertyTools.Observable
	{
		private bool cancelOnClosingFlag;

		[Category("Flags|General")]
		[DisplayName("Cancel On Closing")]
		public bool CancelOnClosingFlag
		{
			get
			{
				return this.cancelOnClosingFlag;
			}
			set
			{
				base.SetValue(ref this.cancelOnClosingFlag, value);
			}
		}
	}


	public class ClosingEventExample3ViewModel : PropertyTools.Observable, IEditableObject
	{
		private string username;

		[Category("General|Credentials")]
		[DisplayName("Username")]
		public string Username
		{
			get
			{
				return this.username;
			}
			set
			{
				base.SetValue(ref this.username, value);
			}
		}

		private ClosingEventExample3ViewModel backup;
		private bool isEditing;

		public void BeginEdit()
		{
			if (this.isEditing) return;
			this.isEditing = true;
			// Simple snapshot: copy current values
			this.backup = new ClosingEventExample3ViewModel { Username = this.Username };
		}

		public void EndEdit()
		{
			if (!isEditing) return;
			this.isEditing = false;
			this.backup = null; // Discard backup
		}

		public void CancelEdit()
		{
			if (!isEditing) return;
			this.isEditing = false;
			// Restore from backup
			this.Username = this.backup.Username;
		}
	}
}