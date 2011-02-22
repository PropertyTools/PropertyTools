using System;
using System.ComponentModel;
using System.Windows.Media;

namespace SimpleGridDemo
{
    public class ExampleObject : Observable
    {
        private bool boolean;
        public bool Boolean
        {
            get { return boolean; }
            set { boolean = value; RaisePropertyChanged("Boolean"); }
        }

        private Fruit _enum;
        public Fruit Enum
        {
            get { return _enum; }
            set { _enum = value; RaisePropertyChanged("Enum"); }
        }

        private double d;
        public double Double
        {
            get { return d; }
            set { d = value; RaisePropertyChanged("Double"); }
        }

        private int integer;
        public int Integer
        {
            get { return integer; }
            set { integer = value; RaisePropertyChanged("Integer"); }
        }

        private DateTime dateTime;
        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; RaisePropertyChanged("DateTime"); }
        }

        private string s;
        public string String
        {
            get { return s; }
            set { s = value; RaisePropertyChanged("String"); }
        }

        private Uri uri;
        [Browsable(false)]
        public Uri Uri
        {
            get { return uri; }
            set { uri = value; RaisePropertyChanged("Uri"); }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; RaisePropertyChanged("Color"); }
        }
    }
}
