// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestContentAttribute.cs" company="PropertyTools">
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
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class TestContentAttribute : TestBase
    {
        [Content]
        [Description("This property contains a StackPanel")]
        public StackPanel Content1 { get; private set; }

        public TestContentAttribute()
        {
            var pfcc = new PathFigureCollectionConverter();
            const string Figures = "M 20,20 L 140,20 L 140,140 L 20,140 L 20,20 M 80,30 L 130,80 L 80,130 L 30,80 L 80,30 M 60,60 L 100,60 L 100,100 L 60,100 L 60,60 M 80,65 L 95,80 L 80,95 L 65,80 L 80,65";
            var pfc = (PathFigureCollection)pfcc.ConvertFrom(Figures);
            var path = new Path
                           {
                               Stroke = Brushes.Blue,
                               StrokeThickness = 1.5,
                               Fill = Brushes.LightBlue,
                               StrokeStartLineCap = PenLineCap.Square,
                               StrokeEndLineCap = PenLineCap.Square,
                               Opacity = 0.6,
                               Data = new PathGeometry(pfc) { FillRule = FillRule.Nonzero }
                           };

            var panel = new StackPanel();
            panel.Children.Add(new TextBlock { Text = "The ContentAttribute lets you mix text and figures. The figure is defined by\nM 20,20 L 140,20 L 140,140 L 20,140 L 20,20 M 80,30 L 130,80 L 80,130 L 30,80 L 80,30 M 60,60 L 100,60 L 100,100 L 60,100 L 60,60 M 80,65 L 95,80 L 80,95 L 65,80 L 80,65" });
            panel.Children.Add(path);
            this.Content1 = panel;
        }

        public override string ToString()
        {
            return "Content attribute";
        }
    }
}