namespace FeaturesDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Media;

    using PropertyTools.DataAnnotations;

    public enum Fruit { Apple, Pear, Banana, Orange }

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

        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; RaisePropertyChanged("Color"); }
        }

        private Mass mass;
        public Mass Mass
        {
            get { return mass; }
            set { mass = value; RaisePropertyChanged("Mass"); }
        }

        private string selector;

        [ItemsSourceProperty("Items")]
        public string Selector
        {
            get
            {
                return this.selector;
            }
            set
            {
                this.selector = value; RaisePropertyChanged("Selector");
            }
        }

        [Browsable(false)]
        public IEnumerable<string> Items
        {
            get
            {
                return new[] { "Oslo", "Stockholm", "Copenhagen" };
            }
        }
    }
}
