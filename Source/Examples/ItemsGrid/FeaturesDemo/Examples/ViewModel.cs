// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="PropertyTools">
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
// <summary>
//   The view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FeaturesDemo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// The view model.
    /// </summary>
    public class ViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            this.Items100 = new ObservableCollection<ExampleObject>();
            for (int i = 0; i < 50; i++)
            {
                this.Items100.Add(
                    new ExampleObject
                        {
                            Boolean = true,
                            DateTime = DateTime.Now,
                            Color = Colors.Blue,
                            Double = Math.PI,
                            Enum = Fruit.Apple,
                            Integer = 7,
                            Selector = null,
                            String = "Hello"
                        });
                this.Items100.Add(
                    new ExampleObject
                        {
                            Boolean = false,
                            DateTime = DateTime.Now.AddDays(-1),
                            Color = Colors.Gold,
                            Double = Math.E,
                            Enum = Fruit.Pear,
                            Integer = -1,
                            Selector = null,
                            String = "World"
                        });
            };

            this.Items200 = new List<PlainOldObject>
                {
                    new PlainOldObject
                        {
                            Boolean = true,
                            DateTime = DateTime.Now,
                            Color = Colors.Blue,
                            Double = Math.PI,
                            Enum = Fruit.Apple,
                            Integer = 7,
                            Selector = null,
                            String = "Hello"
                        },
                    new PlainOldObject
                        {
                            Boolean = true,
                            DateTime = DateTime.Now.AddDays(-1),
                            Color = Colors.Red,
                            Double = Math.PI * 2,
                            Enum = Fruit.Banana,
                            Integer = 7,
                            Selector = null,
                            String = "World"
                        }
                };

            this.Items301 = new List<int> { 3, 7, 9 };
            this.Items302 = new ObservableCollection<double> { 3, 7, 9 };
            this.Items303 = new List<Vector3D> { new Vector3D(1, 0, 0), new Vector3D(0, 1, 0), new Vector3D(0, 0, 1) };
            this.Items401 = new[] { 3.0, 7, 9 };
            this.Items402 = new[]
            {
                11.0, 12 , 13 ,
                21 , 22 , 23 ,
                31 , 32 , 33
            };
            this.Items403 = new[]
            {
                11.0 * Mass.Kilogram, 12 * Mass.Kilogram, 13 * Mass.Kilogram,
                21 * Mass.Kilogram, 22 * Mass.Kilogram, 23 * Mass.Kilogram,
                31 * Mass.Kilogram, 32 * Mass.Kilogram, 33 * Mass.Kilogram
            };
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public ObservableCollection<ExampleObject> Items100 { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<PlainOldObject> Items200 { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<int> Items301 { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<double> Items302 { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<Vector3D> Items303 { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public double[] Items401 { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public double[] Items402 { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public Mass[] Items403 { get; set; }
    }
}