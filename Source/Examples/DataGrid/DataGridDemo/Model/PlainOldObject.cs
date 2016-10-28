// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlainOldObject.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.Generic;
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
        public IEnumerable<string> Items => StandardCollections.Cities;
    }
}