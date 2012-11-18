// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
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
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            this.Items100 = new ObservableCollection<ExampleObject> 
                {
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
                        }, 
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
                        }
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

        #endregion

        #region Public Properties

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
        #endregion
    }
}