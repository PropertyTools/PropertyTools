// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlagsEnumExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for FlagsEnumExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using System;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for FlagsEnumExample.
    /// </summary>
    public partial class FlagsEnumExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlagsEnumExample"/> class.
        /// </summary>
        public FlagsEnumExample()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    /// View model for the <see cref="FlagsEnumExample"/> window.
    /// </summary>
    public class FlagsEnumExampleViewModel : Observable
    {
        /// <summary>
        /// The shared model instance — static so all open windows bind to the same object.
        /// </summary>
        private static readonly FlagsEnumExampleModel SharedModel = new FlagsEnumExampleModel();

        /// <summary>Gets the selected object shown in the PropertyGrid (shared across all windows).</summary>
        public object SelectedObject => SharedModel;
    }

    /// <summary>File-system access permissions flags enum.</summary>
    [Flags]
    public enum FileAccess
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4,
        All = Read | Write | Execute,
    }

    /// <summary>Days of the week flags enum.</summary>
    [Flags]
    public enum WeekDays
    {
        None = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64,
        Weekend = Saturday | Sunday,
        All = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday,
    }

    /// <summary>
    /// Model used by <see cref="FlagsEnumExampleViewModel"/>. Demonstrates that the PropertyGrid
    /// automatically uses a <see cref="PropertyTools.Wpf.CheckBoxList"/> for <see cref="FlagsAttribute"/> enums.
    /// Composite values such as <c>All</c> and <c>Weekend</c> are skipped by the control automatically.
    /// </summary>
    public class FlagsEnumExampleModel : Observable
    {
        private FileAccess access = FileAccess.Read;
        private WeekDays workDays = WeekDays.Monday | WeekDays.Wednesday | WeekDays.Friday;

        [System.ComponentModel.Category("Flags Enum (auto CheckBoxList)")]
        [System.ComponentModel.Description("File access flags — shows Read, Write, Execute checkboxes.")]
        public FileAccess Access
        {
            get => this.access;
            set => this.SetValue(ref this.access, value);
        }

        [System.ComponentModel.Category("Flags Enum (auto CheckBoxList)")]
        [System.ComponentModel.Description("Work days — composite values (Weekend, All) are skipped; only atomic flags shown.")]
        public WeekDays WorkDays
        {
            get => this.workDays;
            set => this.SetValue(ref this.workDays, value);
        }
    }
}
