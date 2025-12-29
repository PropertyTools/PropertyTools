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
        private DateTime dateTime;
        private TimeSpan timeSpan;
        private Uri uri;
        private Complex complex;
        private BigInteger bigInteger;
        private Point point;
        private System.Windows.Vector vector;
        private Rect rect;
        private Size size;
        private Thickness thickness;
        private GridLength gridLength;
        private Color color;
        private SolidColorBrush solidColorBrush;
        private FontFamily fontFamilyWithPreview;
        private string fontFamilySelector;
        private Point3D point3D;
        private Vector3D vector3D;
        private System.Windows.Media.Media3D.Quaternion quaternion;
        private Matrix3D matrix3D;

        public AdvancedTypesExample()
        {
            this.Uri = new Uri("https://www.google.com");
            this.FontFamily = new FontFamily("Arial");
            this.FontFamilySelector = "Times New Roman";
        }

        [Category("System")]
        public DateTime DateTime { get => this.dateTime; set { this.dateTime = value; this.RaisePropertyChanged(nameof(DateTime)); } }
        public TimeSpan TimeSpan { get => this.timeSpan; set { this.timeSpan = value; this.RaisePropertyChanged(nameof(TimeSpan)); } }
        public Uri Uri { get => this.uri; set { this.uri = value; this.RaisePropertyChanged(nameof(Uri)); } }

        [Category("System.Numerics")]
        [Converter(typeof(ComplexConverter))]
        public Complex Complex { get => this.complex; set { this.complex = value; this.RaisePropertyChanged(nameof(Complex)); } }
        [Converter(typeof(BigIntegerConverter))]
        public BigInteger BigInteger { get => this.bigInteger; set { this.bigInteger = value; this.RaisePropertyChanged(nameof(BigInteger)); } }

        [Category("System.Windows")]
        public Point Point { get => this.point; set { this.point = value; this.RaisePropertyChanged(nameof(Point)); } }
        public System.Windows.Vector Vector { get => this.vector; set { this.vector = value; this.RaisePropertyChanged(nameof(Vector)); } }
        public Rect Rect { get => this.rect; set { this.rect = value; this.RaisePropertyChanged(nameof(Rect)); } }
        public Size Size { get => this.size; set { this.size = value; this.RaisePropertyChanged(nameof(Size)); } }
        public Thickness Thickness { get => this.thickness; set { this.thickness = value; this.RaisePropertyChanged(nameof(Thickness)); } }
        public GridLength GridLength { get => this.gridLength; set { this.gridLength = value; this.RaisePropertyChanged(nameof(GridLength)); } }

        [Category("System.Windows.Media")]
        public Color Color { get => this.color; set { this.color = value; this.RaisePropertyChanged(nameof(Color)); } }
        [Converter(typeof(ColorToBrushConverter))]
        public SolidColorBrush SolidColorBrush { get => this.solidColorBrush; set { this.solidColorBrush = value; this.RaisePropertyChanged(nameof(SolidColorBrush)); } }

        [FontPreview(16)]
        public FontFamily FontFamilyWithPreview { get => this.fontFamilyWithPreview; set { this.fontFamilyWithPreview = value; this.RaisePropertyChanged(nameof(FontFamilyWithPreview)); this.RaisePropertyChanged(nameof(FontFamily)); } }

        public FontFamily FontFamily { get { return this.FontFamilyWithPreview; } set { this.FontFamilyWithPreview = value; } }

        [FontFamilySelector]
        [Converter(typeof(FontFamilyConverter))]
        public string FontFamilySelector { get => this.fontFamilySelector; set { this.fontFamilySelector = value; this.RaisePropertyChanged(nameof(FontFamilySelector)); } }

        [FontPreview("FontFamily", 32)]
        public string FontPreview => $"{this.FontFamily}\nThe quick brown fox jumps over the lazy dog.";

        [Font("Courier New")]
        public string CustomFont => "This should be fixed type.";

        [Category("System.Windows.Media.Media3D")]
        public Point3D Point3D { get => this.point3D; set { this.point3D = value; this.RaisePropertyChanged(nameof(Point3D)); } }
        public Vector3D Vector3D { get => this.vector3D; set { this.vector3D = value; this.RaisePropertyChanged(nameof(Vector3D)); } }
        public System.Windows.Media.Media3D.Quaternion Quaternion { get => this.quaternion; set { this.quaternion = value; this.RaisePropertyChanged(nameof(Quaternion)); } }
        public Matrix3D Matrix3D { get => this.matrix3D; set { this.matrix3D = value; this.RaisePropertyChanged(nameof(Matrix3D)); } }
    }
}