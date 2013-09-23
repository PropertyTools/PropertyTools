// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestCommentAttribute.cs" company="PropertyTools">
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
// --------------------------------------------------------------------------------------------------------------------
namespace TestLibrary
{
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using PropertyTools.DataAnnotations;

    public class TestCommentAttribute : TestBase
    {
        [Comment]
        public string Comment1 { get; private set; }

        [Comment]
        public string Comment2 { get; private set; }

        [Comment]
        public string Comment3 { get; private set; }

        [Comment]
        [HeaderPlacement(HeaderPlacement.Hidden)]
        public string Comment4 { get; private set; }

        [Comment]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public string Comment5 { get; private set; }

        public TestCommentAttribute()
        {
            Comment1 = "This is a comment.";
            Comment2 = "This is a multiline\ncomment.";
            Comment3 = "This is a wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping comment.";
            Comment4 = "Comment with hidden header";
            Comment5 = "Comment with collapsed header";

            //var pfc = new PathFigureCollectionConverter();
            //var figures = "M 20,20 L 140,20 L 140,140 L 20,140 L 20,20 M 80,30 L 130,80 L 80,130 L 30,80 L 80,30 M 60,60 L 100,60 L 100,100 L 60,100 L 60,60 M 80,65 L 95,80 L 80,95 L 65,80 L 80,65";
            //var path = new Path
            //    {
            //        Stroke = Brushes.Blue,
            //        StrokeThickness = 1.5,
            //        Fill = Brushes.LightBlue,
            //        StrokeStartLineCap = PenLineCap.Square,
            //        StrokeEndLineCap = PenLineCap.Square,
            //        Opacity = 0.6,
            //        Data = new PathGeometry(pfc.ConvertFrom(figures) as PathFigureCollection) { FillRule = FillRule.Nonzero }
            //    };

            //var panel = new StackPanel();
            //panel.Children.Add(new TextBlock() { Text = "Mixed text and figures. " });
            //panel.Children.Add(path);
            //Comment4 = panel;
        }

        public override string ToString()
        {
            return "Comment attribute";
        }
    }
}