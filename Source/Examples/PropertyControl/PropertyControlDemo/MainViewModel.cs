// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
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
namespace PropertyControlDemo
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text;

    using TestLibrary;

    public class MainViewModel : INotifyPropertyChanged
    {
        public List<object> Models { get; set; }

        private object selectedItem;

        public object SelectedItem
        {
            get
            {
                return this.selectedItem;
            }
            set
            {
                this.selectedItem = value;
                RaisePropertyChanged("SelectedItem");
                RaisePropertyChanged("Output");
            }
        }

        public string Output
        {
            get
            {
                return CreateOutput(SelectedItem);
            }
        }

        private string CreateOutput(object item)
        {
            if (item == null)
                return null;
            var sb = new StringBuilder();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(item))
            {
                var value = pd.GetValue(item);
                sb.AppendLine(pd.Name + " = " + ValueToString(value));
            }
            return sb.ToString();
        }

        string ValueToString(object o)
        {
            if (o is IList) return "[" + ((IList)o).Count.ToString() + "]";
            if (o == null) return "null";
            if (o is string) return string.Format("\"{0}\"", o);
            if (o is char) return string.Format("'{0}'", o);
            return string.Format("({0}) {1}", o.GetType().Name, o.ToString());
        }

        public MainViewModel()
        {
            Models = Tests.Get();
            foreach (TestBase m in Models)
            {
                m.PropertyChanged += TestChanged;
            }
        }

        private void TestChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("Changed " + e.PropertyName);
            RaisePropertyChanged("Output");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}