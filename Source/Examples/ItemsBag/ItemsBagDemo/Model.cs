namespace ItemsBagDemo
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;

    public enum Colors { Red, Green, Blue }

    public class Model : INotifyPropertyChanged
    {

        private bool isChecked;

        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                this.isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }

        private string name;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value; RaisePropertyChanged("Name");
            }
        }

        private int? value;

        [Required]
        public int? Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value; RaisePropertyChanged("Value");
            }
        }

        private Colors color;

        public Colors Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value; RaisePropertyChanged("Color");
            }
        }

        #region PropertyChanged Block
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            Debug.WriteLine("Model.RaisePropertyChanged on " + property);

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion


    }
}