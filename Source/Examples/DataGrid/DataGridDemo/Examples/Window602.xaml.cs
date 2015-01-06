// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window602.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window602.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Windows.Media;

namespace DataGridDemo
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Interaction logic for Window602
    /// </summary>
    public partial class Window602
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window602" /> class.
        /// </summary>
        public Window602()
        {
            this.InitializeComponent();
            this.DataContext = this;

            var lst = new List<DemoClass>()
            {
                new DemoClass()
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
            Grid1.ItemsSource = lst;
        }

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
