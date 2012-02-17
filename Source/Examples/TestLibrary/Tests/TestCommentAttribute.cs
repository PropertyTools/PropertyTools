namespace TestLibrary
{
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using PropertyTools.DataAnnotations;

    public class TestCommentAttribute : TestBase
    {
        public int Number1 { get; set; }

        [Comment]
        public string Comment1 { get; private set; }

        public int Number2 { get; set; }

        [Comment]
        public string Comment2 { get; private set; }

        [Comment]
        public string Comment3 { get; private set; }

        public TestCommentAttribute()
        {
            Comment1 = "This is a comment.";
            Comment2 = "This is a multiline\ncomment.";
            Comment3 = "This is a wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping comment.";

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