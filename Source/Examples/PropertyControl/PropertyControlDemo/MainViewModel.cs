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

        #region PropertyChanged Block
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion
    }
}