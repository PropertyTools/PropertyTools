// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestAdvancedTypes.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
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
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.ComponentModel;
    using System.Numerics;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    using PropertyTools.DataAnnotations;
    using PropertyTools.Wpf;

    using FontFamilyConverter = PropertyTools.Wpf.FontFamilyConverter;

    [PropertyGridExample]
    public class TestAdvancedTypes : TestBase
    {
        public TestAdvancedTypes()
        {
            Uri = new Uri("http://www.google.com");
            FontFamily = new FontFamily("Arial");
            FontFamilySelector = "Times New Roman";
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
        public Vector Vector { get; set; }
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

        public FontFamily FontFamily { get { return FontFamilyWithPreview; } set { FontFamilyWithPreview = value; } }

        [FontFamilySelector]
        [Converter(typeof(FontFamilyConverter))]
        public string FontFamilySelector { get; set; }

        [FontPreview("FontFamily", 32, 500)]
        public string FontPreview
        {
            get
            {
                return string.Format("{0}\nThe quick brown fox jumps over the lazy dog.", this.FontFamily).Trim();
            }
        }

        [Font("Courier New")]
        public string CustomFont
        {
            get
            {
                return "This should be fixed type.";
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