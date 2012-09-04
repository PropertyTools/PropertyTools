using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlsDemo
{
    using System.ComponentModel;

    using PropertyTools.Wpf;

    /// <summary>
    /// Interaction logic for ColorPickerPage.xaml
    /// </summary>
    public partial class ColorPicker2Page : Page, INotifyPropertyChanged
    {
        private Color color1;

        public Color Color1
        {
            get
            {
                return this.color1;
            }
            set
            {
                this.color1 = value;
                RaisePropertyChanged("Color1");
            }
        }
        private Color color2;

        public Color Color2
        {
            get
            {
                return this.color2;
            }
            set
            {
                this.color2 = value;
                RaisePropertyChanged("Color2");
            }
        }
        private Color color3;

        public Color Color3
        {
            get
            {
                return this.color3;
            }
            set
            {
                this.color3 = value;
                RaisePropertyChanged("Color3");
            }
        }
        private Color? color4;

        public Color? Color4
        {
            get
            {
                return this.color4;
            }
            set
            {
                this.color4 = value;
                RaisePropertyChanged("Color4");
            }
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


        public SolidColorBrush Brush { get; set; }

        public ColorPicker2Page()
        {
            InitializeComponent();
            this.Color1 = Color.FromArgb(80, 255, 0, 0);
            this.Color2 = ColorHelper.UndefinedColor;
            this.Color3 = ColorHelper.Automatic;
            this.Color4 = null;
            this.Brush = Brushes.Blue;
            DataContext = this;
        }
    }
}
