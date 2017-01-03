// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class ContentAttributeExample : Example
    {
        public ContentAttributeExample()
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

        [Content]
        [Description("This property contains a StackPanel")]
        public StackPanel Content1 { get; private set; }
    }
}