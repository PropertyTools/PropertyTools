// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InsertionAdorner.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Insertion bar adorner.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// Provides an insertion bar adorner.
    /// </summary>
    /// <remarks>
    /// From http://bea.stollnitz.com/blog/?p=53
    /// </remarks>
    public class InsertionAdorner : Adorner
    {
        #region Constants and Fields

        /// <summary>
        /// The pen.
        /// </summary>
        private static readonly Pen pen;

        /// <summary>
        /// The triangle.
        /// </summary>
        private static readonly PathGeometry triangle;

        /// <summary>
        /// The adorner layer.
        /// </summary>
        private readonly AdornerLayer adornerLayer;

        /// <summary>
        /// The is separator horizontal.
        /// </summary>
        private readonly bool isSeparatorHorizontal;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="InsertionAdorner"/> class. 
        /// </summary>
        static InsertionAdorner()
        {
            // Create the pen and triangle in a static constructor and freeze them to improve performance.
            pen = new Pen { Brush = Brushes.Gray, Thickness = 2 };
            pen.Freeze();

            var firstLine = new LineSegment(new Point(0, -5), false);
            firstLine.Freeze();
            var secondLine = new LineSegment(new Point(0, 5), false);
            secondLine.Freeze();

            var figure = new PathFigure { StartPoint = new Point(5, 0) };
            figure.Segments.Add(firstLine);
            figure.Segments.Add(secondLine);
            figure.Freeze();

            triangle = new PathGeometry();
            triangle.Figures.Add(figure);
            triangle.Freeze();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertionAdorner"/> class.
        /// </summary>
        /// <param name="isSeparatorHorizontal">
        /// if set to <c>true</c> [is separator horizontal].
        /// </param>
        /// <param name="isInFirstHalf">
        /// if set to <c>true</c> [is in first half].
        /// </param>
        /// <param name="adornedElement">
        /// The adorned element.
        /// </param>
        /// <param name="adornerLayer">
        /// The adorner layer.
        /// </param>
        public InsertionAdorner(
            bool isSeparatorHorizontal, bool isInFirstHalf, UIElement adornedElement, AdornerLayer adornerLayer)
            : base(adornedElement)
        {
            this.isSeparatorHorizontal = isSeparatorHorizontal;
            this.IsInFirstHalf = isInFirstHalf;
            this.adornerLayer = adornerLayer;
            this.IsHitTestVisible = false;

            this.adornerLayer.Add(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether IsInFirstHalf.
        /// </summary>
        public bool IsInFirstHalf { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Detaches this instance from the adorner layer.
        /// </summary>
        public void Detach()
        {
            this.adornerLayer.Remove(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// When overridden in a derived class, participates in rendering operations that are directed by the layout system. The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing.
        /// </summary>
        /// <param name="drawingContext">
        /// The drawing instructions for a specific element. This context is provided to the layout system.
        /// </param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            // This draws one line and two triangles at each end of the line.
            Point startPoint;
            Point endPoint;

            this.CalculateStartAndEndPoint(out startPoint, out endPoint);
            drawingContext.DrawLine(pen, startPoint, endPoint);

            if (this.isSeparatorHorizontal)
            {
                this.DrawTriangle(drawingContext, startPoint, 0);
                this.DrawTriangle(drawingContext, endPoint, 180);
            }
            else
            {
                this.DrawTriangle(drawingContext, startPoint, 90);
                this.DrawTriangle(drawingContext, endPoint, -90);
            }
        }

        /// <summary>
        /// Calculates the start and end point.
        /// </summary>
        /// <param name="startPoint">
        /// The start point.
        /// </param>
        /// <param name="endPoint">
        /// The end point.
        /// </param>
        private void CalculateStartAndEndPoint(out Point startPoint, out Point endPoint)
        {
            startPoint = new Point();
            endPoint = new Point();

            double width = this.AdornedElement.RenderSize.Width;
            double height = this.AdornedElement.RenderSize.Height;

            if (this.isSeparatorHorizontal)
            {
                endPoint.X = width;
                if (!this.IsInFirstHalf)
                {
                    startPoint.Y = height;
                    endPoint.Y = height;
                }
            }
            else
            {
                endPoint.Y = height;
                if (!this.IsInFirstHalf)
                {
                    startPoint.X = width;
                    endPoint.X = width;
                }
            }
        }

        /// <summary>
        /// Draws the triangle.
        /// </summary>
        /// <param name="drawingContext">
        /// The drawing context.
        /// </param>
        /// <param name="origin">
        /// The origin.
        /// </param>
        /// <param name="angle">
        /// The angle.
        /// </param>
        private void DrawTriangle(DrawingContext drawingContext, Point origin, double angle)
        {
            drawingContext.PushTransform(new TranslateTransform(origin.X, origin.Y));
            drawingContext.PushTransform(new RotateTransform(angle));

            drawingContext.DrawGeometry(pen.Brush, null, triangle);

            drawingContext.Pop();
            drawingContext.Pop();
        }

        #endregion
    }
}