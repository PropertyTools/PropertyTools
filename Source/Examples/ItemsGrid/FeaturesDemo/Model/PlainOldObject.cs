namespace FeaturesDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Media;

    using PropertyTools.DataAnnotations;

    public class PlainOldObject
    {
        public bool Boolean { get; set; }
        
        public Fruit Enum { get; set; }
        
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

        public PlainOldObject()
        {
            DateTime = DateTime.Now;
        }
    }
}
