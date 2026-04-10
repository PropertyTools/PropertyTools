// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FillTabAboveExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.ObjectModel;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Demonstrates that a property with <see cref="FillTabAttribute"/> and
    /// <see cref="HeaderPlacementAttribute"/> set to <see cref="HeaderPlacement.Above"/>
    /// correctly fills the remaining tab height — including when the model implements
    /// <see cref="System.ComponentModel.IDataErrorInfo"/>.
    /// </summary>
    [PropertyGridExample]
    public class FillTabAboveExample : Example, System.ComponentModel.IDataErrorInfo
    {
        /// <summary>
        /// Gets or sets the collection of points. The DataGrid should fill the tab vertically.
        /// </summary>
        [Category("FillTab + HeaderPlacement.Above|Points")]
        [Description("This collection should fill the available tab space.")]
        [FillTab]
        [HeaderPlacement(HeaderPlacement.Above)]
        public ObservableCollection<Point3D> Points { get; set; } = new ObservableCollection<Point3D>
        {
            new Point3D { X = 1, Y = 2, Z = 3 },
            new Point3D { X = 4, Y = 5, Z = 6 },
            new Point3D { X = 7, Y = 8, Z = 9 },
            new Point3D { X = 10, Y = 11, Z = 12 },
            new Point3D { X = 13, Y = 14, Z = 15 },
            new Point3D { X = 16, Y = 17, Z = 18 },
            new Point3D { X = 19, Y = 20, Z = 21 },
            new Point3D { X = 22, Y = 23, Z = 24 },
        };

        /// <inheritdoc/>
        string System.ComponentModel.IDataErrorInfo.Error => null;

        /// <inheritdoc/>
        string System.ComponentModel.IDataErrorInfo.this[string columnName] => null;
    }

    /// <summary>
    /// Represents a point in 3D space.
    /// </summary>
    public class Point3D
    {
        /// <summary>Gets or sets the X coordinate.</summary>
        public double X { get; set; }

        /// <summary>Gets or sets the Y coordinate.</summary>
        public double Y { get; set; }

        /// <summary>Gets or sets the Z coordinate.</summary>
        public double Z { get; set; }
    }
}
