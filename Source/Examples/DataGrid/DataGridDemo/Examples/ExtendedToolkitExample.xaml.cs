// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendedToolkitExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ExtendedToolkitExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ExtendedToolkitExample
    /// </summary>
    public partial class ExtendedToolkitExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedToolkitExample" /> class.
        /// </summary>
        public ExtendedToolkitExample()
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.Items = new List<DemoClass>
            {
                new DemoClass
                {
                    DateTime = DateTime.Now,
                    TimeSpan = TimeSpan.FromSeconds(3500),
                    Brush = Brushes.Red,
                    Int = 33,
                    UInt = 44,
                    Guid = Guid.NewGuid(),
                    Char = 'h',
                    Decimal = 9,
                    Double = 3.6
                }
            };
        }

        public List<DemoClass> Items { get; set; }

        public class DemoClass
        {
            public DateTime DateTime { get; set; }
            public TimeSpan TimeSpan { get; set; }
            public Brush Brush { get; set; }
            public int Int { get; set; }
            public uint UInt { get; set; }
            public Guid Guid { get; set; }
            public char Char { get; set; }
            public decimal Decimal { get; set; }
            public Single Single { get; set; }
            public Double Double { get; set; }
        }
    }
}
