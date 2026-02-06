// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SelectedObjectsDemo
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TestObject> TestCollection { get; set; }

        public MainWindowViewModel()
        {
            this.TestCollection = new ObservableCollection<TestObject>
            {
                new TestObject { Name = "Object 1", Description = "First test object", Value = 100 },
                new TestObject { Name = "Object 2", Description = "Second test object", Value = 200 }
            };
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
