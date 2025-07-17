// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdvancedTypesExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Numerics;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    using PropertyTools.DataAnnotations;
    using PropertyTools.Wpf;

    using FontFamilyConverter = PropertyTools.Wpf.FontFamilyConverter;

    [PropertyGridExample]
    public class AdvancedTypesExample : Example
    {
        public AdvancedTypesExample()
        {
            this.Uri = new Uri("https://www.google.com");
            this.FontFamily = new FontFamily("Arial");
            this.FontFamilySelector = "Times New Roman";
        }

        [Category("System")]
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public Uri Uri { get; set; }

        [Category("System.Numerics")]
        [Converter(typeof(ComplexConverter))]
        public Complex Complex { get; set; }
        [Converter(typeof(BigIntegerConverter))]
        public BigInteger BigInteger { get; set; }

        [Category("System.Windows")]
        public Point Point { get; set; }
        public System.Windows.Vector Vector { get; set; }
        public Rect Rect { get; set; }
        public Size Size { get; set; }
        public Thickness Thickness { get; set; }
        public GridLength GridLength { get; set; }

        [Category("System.Windows.Media")]
        public Color Color { get; set; }
        [Converter(typeof(ColorToBrushConverter))]
        public SolidColorBrush SolidColorBrush { get; set; }

        [FontPreview(16)]
        public FontFamily FontFamilyWithPreview { get; set; }

        public FontFamily FontFamily { get { return this.FontFamilyWithPreview; } set { this.FontFamilyWithPreview = value; } }

        [FontFamilySelector]
        [Converter(typeof(FontFamilyConverter))]
        public string FontFamilySelector { get; set; }

        [FontPreview("FontFamily", 32)]
        public string FontPreview => $"{this.FontFamily}\nThe quick brown fox jumps over the lazy dog.";

        [Font("Courier New")]
        public string CustomFont => "This should be fixed type.";

        [Category("System.Windows.Media.Media3D")]
        public Point3D Point3D { get; set; }
        public Vector3D Vector3D { get; set; }
        public System.Windows.Media.Media3D.Quaternion Quaternion { get; set; }
        public Matrix3D Matrix3D { get; set; }
    }
}