using System.ComponentModel;
using System.Windows.Media;

namespace PropertyTools.Wpf.Controls.ColorPicker
{
    /// <summary>
    /// Wrapper class for colors - needed to get unique items in the persistent color list
    /// since Color.XXX added in multiple positions results in multiple items being selected.
    /// Also needed to implement the INotifyPropertyChanged for binding support.
    /// </summary>
    public class ColorWrapper : INotifyPropertyChanged
    {
        private Color color;


        public ColorWrapper(Color c)
        {
            Color = c;
        }

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}