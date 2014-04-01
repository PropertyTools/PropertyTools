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

    using PropertyTools;
    using PropertyTools.DataAnnotations;

    public class ExampleObject : Observable
    {
        private bool boolean;
        private Fruit fruit;
        private double number;
        private string selector;
        private int integer;
        private int readOnlyInteger;
        private DateTime dateTime;
        private string s;
        private Color color;
        private Mass mass;

        public bool Boolean
        {
            get
            {
                return this.boolean;
            }

            set
            {
                if (this.SetValue(ref this.boolean, value, () => this.Boolean))
                {
                    this.RaisePropertyChanged("ReadOnlyBoolean");
                }
            }
        }

        public bool ReadOnlyBoolean
        {
            get
            {
                return this.boolean;
            }
        }

        public Fruit Fruit
        {
            get
            {
                return this.fruit;
            }

            set
            {
                this.SetValue(ref this.fruit, value, () => this.Fruit);
            }
        }

        public double Number
        {
            get
            {
                return this.number;
            }

            set
            {
                this.SetValue(ref this.number, value, () => this.Number);
            }
        }

        public int Integer
        {
            get
            {
                return this.integer;
            }

            set
            {
                this.SetValue(ref this.integer, value, () => this.Integer);
            }
        }

        [ReadOnly(true)]
        public int ReadOnlyInteger
        {
            get
            {
                return this.readOnlyInteger;
            }

            set
            {
                this.SetValue(ref this.readOnlyInteger, value, () => this.ReadOnlyInteger);
            }
        }

        public DateTime DateTime
        {
            get
            {
                return this.dateTime;
            }

            set
            {
                this.SetValue(ref this.dateTime, value, () => this.DateTime);
            }
        }

        public string String
        {
            get
            {
                return this.s;
            }

            set
            {
                if (this.SetValue(ref this.s, value, () => this.String))
                {
                    this.RaisePropertyChanged(() => this.ReadOnlyString);
                }
            }
        }

        public string ReadOnlyString
        {
            get
            {
                return s != null ? "L=" + this.s.Length : null;
            }
        }

        public Color Color
        {
            get
            {
                return this.color;
            }

            set
            {
                this.SetValue(ref this.color, value, () => this.Color);
            }
        }

        public Mass Mass
        {
            get
            {
                return this.mass;
            }

            set
            {
                this.SetValue(ref this.mass, value, () => this.Mass);
            }
        }

        [ItemsSourceProperty("Items")]
        public string Selector
        {
            get
            {
                return this.selector;
            }

            set
            {
                this.SetValue(ref this.selector, value, () => this.Selector);
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