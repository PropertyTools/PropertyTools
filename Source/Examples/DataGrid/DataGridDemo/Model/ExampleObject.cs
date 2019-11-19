// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleObject.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                if (this.SetValue(ref this.boolean, value))
                {
                    this.RaisePropertyChanged("ReadOnlyBoolean");
                }
            }
        }

        public bool ReadOnlyBoolean => this.boolean;

        public Fruit Fruit
        {
            get
            {
                return this.fruit;
            }

            set
            {
                this.SetValue(ref this.fruit, value);
            }
        }

        public IEnumerable<Fruit> Fruits => Enum.GetValues(typeof(Fruit)).Cast<Fruit>();

        public double Number
        {
            get
            {
                return this.number;
            }

            set
            {
                this.SetValue(ref this.number, value);
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
                this.SetValue(ref this.integer, value);
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
                this.SetValue(ref this.readOnlyInteger, value);
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
                this.SetValue(ref this.dateTime, value);
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
                if (this.SetValue(ref this.s, value))
                {
                    this.RaisePropertyChanged(nameof(this.ReadOnlyString));
                }
            }
        }

        public string ReadOnlyString => this.s != null ? "L=" + this.s.Length : null;

        public Color Color
        {
            get
            {
                return this.color;
            }

            set
            {
                this.SetValue(ref this.color, value);
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
                this.SetValue(ref this.mass, value);
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
                this.SetValue(ref this.selector, value);
            }
        }

        [Browsable(false)]
        public IEnumerable<string> Items => StandardCollections.Cities;

        private static readonly Random r = new Random(0);

        public static ExampleObject CreateRandom()
        {
            return new ExampleObject
                       {
                           Boolean = r.Next(2) == 0,
                           DateTime = DateTime.Now.AddDays(r.Next(365)),
                           Color =
                               Color.FromArgb(
                                   (byte)r.Next(255),
                                   (byte)r.Next(255),
                                   (byte)r.Next(255),
                                   (byte)r.Next(255)),
                           Number = r.Next(),
                           Fruit = (Fruit)r.Next(5),
                           Integer = r.Next(1024),
                           Selector = null,
                           String = StandardCollections.GenerateName()
                       };
        }

        public override string ToString() => this.String;
    }
}