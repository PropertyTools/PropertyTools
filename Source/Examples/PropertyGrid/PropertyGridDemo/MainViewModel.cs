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

namespace PropertyGridDemo
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using PropertyTools;

    using TestLibrary;

    public class MainViewModel : Observable
    {
        private object selectedItem;

        public MainViewModel()
        {
            this.Models = Examples.GetPropertyGridExamples().ToList();
            this.SelectedItem = this.Models.FirstOrDefault(o => o.GetType() == typeof(TestSimpleTypes));
            foreach (TestBase m in this.Models)
            {
                m.PropertyChanged += this.TestChanged;
            }
        }

        public List<object> Models { get; private set; }

        public object SelectedItem
        {
            get
            {
                return this.selectedItem;
            }

            set
            {
                if (this.SetValue(ref this.selectedItem, value, () => this.SelectedItem))
                {
                    this.RaisePropertyChanged(() => this.Output);
                }
            }
        }

        public string Output
        {
            get
            {
                return this.CreateOutput(this.SelectedItem);
            }
        }

        private string CreateOutput(object item)
        {
            if (item == null)
            {
                return null;
            }

            var sb = new StringBuilder();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(item))
            {
                var value = pd.GetValue(item);
                sb.AppendLine(pd.Name + " = " + this.ValueToString(value));
            }

            return sb.ToString();
        }

        private string ValueToString(object o)
        {
            var list = o as IList;
            if (list != null)
            {
                return string.Format("[{0}]", list.Count);
            }

            if (o == null)
            {
                return "null";
            }

            if (o is string)
            {
                return string.Format("\"{0}\"", o);
            }

            if (o is char)
            {
                return string.Format("'{0}'", o);
            }

            return string.Format("({0}) {1}", o.GetType().Name, o);
        }

        private void TestChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("Changed " + e.PropertyName);
            this.RaisePropertyChanged(() => this.Output);
        }
    }
}