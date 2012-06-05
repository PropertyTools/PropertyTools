namespace TestLibrary
{
    using System;
    using System.ComponentModel;
    using System.Numerics;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    using PropertyTools.DataAnnotations;

    public class TestAdvancedTypes : TestBase
    {
        [Category("System")]
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public Uri Uri { get; set; }

        [Category("System.Numerics")]
        public Complex Complex { get; set; }
        public BigInteger BigInteger { get; set; }

        [Category("System.Windows")]
        public Point Point { get; set; }
        public Vector Vector { get; set; }
        public Rect Rect { get; set; }
        public Size Size { get; set; }
        public Thickness Thickness { get; set; }
        public GridLength GridLength { get; set; }

        [Category("System.Windows.Media")]
        public Color Color { get; set; }
        public SolidColorBrush SolidColorBrush { get; set; }

        [FontPreview(16)]
        public FontFamily FontFamilyWithPreview { get; set; }

        public FontFamily FontFamily { get { return FontFamilyWithPreview; } set { FontFamilyWithPreview = value; } }

        [FontPreview("FontFamily", 32, 500)]
        public string FontPreview
        {
            get
            {
                return string.Format("{0}\nThe quick brown fox jumps over the lazy dog.", this.FontFamily).Trim();
            }
        }

        [Category("System.Windows.Media.Media3D")]
        public Point3D Point3D { get; set; }
        public Vector3D Vector3D { get; set; }
        public Quaternion Quaternion { get; set; }
        public Matrix3D Matrix3D { get; set; }

        public override string ToString()
        {
            return "Advanced types";
        }
    }
}