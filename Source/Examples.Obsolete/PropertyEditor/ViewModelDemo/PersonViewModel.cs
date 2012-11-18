// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonViewModel.cs" company="PropertyTools">
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
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using PropertyTools.Wpf;

namespace ViewModelDemo
{
    using PropertyTools;
    using PropertyTools.DataAnnotations;

    public class PersonViewModel : INotifyPropertyChanged, IDataErrorInfo, IPropertyStateUpdater
    {
        private readonly Person model;

        public PersonViewModel(Person model)
        {
            this.model = model;
        }

        [Category("Personal data|General")]
        [DisplayName("Is anonymous"), Description("If this person is anonymous")]
        public bool Anonymous
        {
            get { return model.Anonymous; }
            set
            {
                if (model.Anonymous != value)
                {
                    model.Anonymous = value;
                    RaisePropertyChanged("Anonymous");
                }
            }
        }
        [DisplayName("Full name"), Description("The full name of the person")]
        public string Name
        {
            get { return model.Name; }
            set
            {
                if (model.Name != value)
                {
                    model.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        [DisplayName("Height (m)"), Description("The height of the person"),
        Slidable(0, 2, 0.01, 0.1), FormatString("0.00")]
        public double Height
        {
            get { return model.Height; }
            set
            {
                if (model.Height != value)
                {
                    model.Height = value;
                    RaisePropertyChanged("Height");
                }
            }
        }
        [DisplayName("Age (years)"), Description("The age of the person"), Slidable(0,100)]
        public double Age
        {
            get { return model.Age; }
            set
            {
                if (model.Age != value)
                {
                    model.Age = (int)value;
                    RaisePropertyChanged("Age");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        public void UpdatePropertyStates(PropertyStateBag stateBag)
        {
            stateBag.Enable("Age", !String.IsNullOrEmpty(Name));
            stateBag.Enable("Height", !Anonymous);
            stateBag.Enable("Name", !Anonymous);
        }
        [Browsable(false)]
        public string Error
        {
            get
            {
                if (Age < 0) return "ERROR: Negative age";
                if (String.IsNullOrEmpty(Name)) return "ERROR: Missing name";
                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Age":
                        if (Age < 0) return "Negative age";
                        break;
                    case "Name":
                        if (String.IsNullOrEmpty(Name)) return "Missing name";
                        break;
                }
                return null;
            }
        }

    }
}