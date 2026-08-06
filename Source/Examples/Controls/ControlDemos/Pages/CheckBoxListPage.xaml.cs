// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckBoxListPage.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for CheckBoxListPage.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlDemos
{
    using System;
    using System.ComponentModel;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for CheckBoxListPage.xaml
    /// </summary>
    public partial class CheckBoxListPage : Page
    {
        private readonly CheckBoxListViewModel vm = new CheckBoxListViewModel();

        public CheckBoxListPage()
        {
            this.InitializeComponent();
            this.vm.SelectedPermissions = Permission.Read | Permission.Write;
            this.DataContext = this.vm;
        }
    }

    public class CheckBoxListViewModel : INotifyPropertyChanged
    {
        private Permission selectedPermissions;

        public event PropertyChangedEventHandler PropertyChanged;

        public Permission SelectedPermissions
        {
            get
            {
                return this.selectedPermissions;
            }

            set
            {
                this.selectedPermissions = value;
                this.RaisePropertyChanged(nameof(this.SelectedPermissions));
            }
        }

        protected void RaisePropertyChanged(string property)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    [Flags]
    public enum Permission
    {
        Read = 1,
        Write = 2,
        Execute = 4,
        All = Read | Write | Execute
    }
}
