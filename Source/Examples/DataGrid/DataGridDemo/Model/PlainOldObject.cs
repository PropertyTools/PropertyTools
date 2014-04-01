// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlainOldObject.cs" company="PropertyTools">
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
    using System.Linq;
    using System.Windows.Media;

    using PropertyTools.DataAnnotations;

    public class PlainOldObject
    {
        public PlainOldObject()
        {
            this.DateTime = DateTime.Now;
            this.Selector = this.Items.First();
        }

        public bool Boolean { get; set; }

        public Fruit Fruit { get; set; }

        public double Double { get; set; }

        public int Integer { get; set; }

        public DateTime DateTime { get; set; }

        public string String { get; set; }

        public Color Color { get; set; }

        public Mass Mass { get; set; }

        [ItemsSourceProperty("Items")]
        public string Selector { get; set; }

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