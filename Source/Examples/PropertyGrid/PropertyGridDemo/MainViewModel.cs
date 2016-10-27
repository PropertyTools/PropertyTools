// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
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

    using ExampleLibrary;

    using PropertyTools;

    public class MainViewModel : Observable
    {
        private object selectedItem;

        public MainViewModel()
        {
            this.Models = Examples.GetPropertyGridExamples().ToList();
            this.SelectedItem = this.Models.FirstOrDefault(o => o.GetType() == typeof(SimpleTypesExample));
            foreach (Example m in this.Models)
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