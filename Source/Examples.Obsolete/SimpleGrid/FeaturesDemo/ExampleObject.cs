// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleObject.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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