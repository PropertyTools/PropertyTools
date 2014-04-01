// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleObject.cs" company="PropertyTools">
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

namespace DataGridDemo
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
            get
            {
                return boolean;
            }

            set
            {
                boolean = value;
                RaisePropertyChanged("Boolean");
                RaisePropertyChanged("ReadOnlyBoolean");
            }
        }

        public bool ReadOnlyBoolean
        {
            get { return boolean; }
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

        private int readOnlyInteger;
        [ReadOnly(true)]
        public int ReadOnlyInteger
        {
            get { return readOnlyInteger; }
            set { readOnlyInteger = value; RaisePropertyChanged("ReadOnlyInteger"); }
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
            set
            {
                s = value;
                RaisePropertyChanged("String");
                RaisePropertyChanged("ReadOnlyString");
            }
        }

        public string ReadOnlyString
        {
            get
            {
                return s != null ? "L=" + s.Length.ToString() : null;
            }
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