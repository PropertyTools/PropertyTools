// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bitmap.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents an image that avoids blurring over pixel boundaries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Represents an image that avoids blurring over pixel boundaries.
    /// </summary>
    /// <remarks>The Bitmap element is using the ActualWidth/Height of the image for the control size.
    /// It also offsets the image to avoid blurring over pixel boundaries.</remarks>
    public class Bitmap : FrameworkElement
    {
        /// <summary>
        /// Identifies the <see cref="Source"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source),
            typeof(BitmapSource),
            typeof(Bitmap),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnSourceChanged));

        /// <summary>
        /// The source downloaded.
        /// </summary>
        private readonly EventHandler sourceDownloaded;

        /// <summary>
        /// The source failed.
        /// </summary>
        private readonly EventHandler<ExceptionEventArgs> sourceFailed;

        /// <summary>
        /// The pixel offset.
        /// </summary>
        private Point pixelOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap" /> class.
        /// </summary>
        public Bitmap()
        {
            this.sourceDownloaded = this.OnSourceDownloaded;
            this.sourceFailed = this.OnSourceFailed;

            this.LayoutUpdated += this.OnLayoutUpdated;
        }

        /// <summary>
        /// The bitmap failed.
        /// </summary>
        public event EventHandler<ExceptionEventArgs> BitmapFailed;

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public BitmapSource Source
        {
            get
            {
                return (BitmapSource)this.GetValue(SourceProperty);
            }

            set
            {
                this.SetValue(SourceProperty, value);
            }
        }

        /// <summary>
        /// When overridden in a derived class, measures the size in layout required for child elements and determines a size for the <see cref="T:System.Windows.FrameworkElement" />-derived class.
        /// </summary>
        /// <param name="availableSize">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
        /// <returns>
        /// The size that this element determines it needs during layout, based on its calculations of child element sizes.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var measureSize = new Size();

            BitmapSource bitmapSource = this.Source;
            if (bitmapSource != null)
            {
                PresentationSource ps = PresentationSource.FromVisual(this);
                if (ps != null && ps.CompositionTarget != null)
                {
                    Matrix fromDevice = ps.CompositionTarget.TransformFromDevice;

                    var pixelSize = new Vector(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
                    Vector measureSizeV = fromDevice.Transform(pixelSize);
                    measureSize = new Size(measureSizeV.X, measureSizeV.Y);
                }
            }

            return measureSize;
        }

        /// <summary>
        /// Called when rendering.
        /// </summary>
        /// <param name="dc">The dc.</param>
        protected override void OnRender(DrawingContext dc)
        {
            BitmapSource bitmapSource = this.Source;
            if (bitmapSource != null)
            {
                this.pixelOffset = this.GetPixelOffset();

                // Render the bitmap offset by the needed amount to align to pixels.
                dc.DrawImage(bitmapSource, new Rect(this.pixelOffset, this.DesiredSize));
            }
        }

        /// <summary>
        /// Called when the source changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bitmap = (Bitmap)d;

            var oldValue = (BitmapSource)e.OldValue;
            var newValue = (BitmapSource)e.NewValue;

            if (((oldValue != null) && (bitmap.sourceDownloaded != null))
                && (!oldValue.IsFrozen && (oldValue is BitmapSource)))
            {
                oldValue.DownloadCompleted -= bitmap.sourceDownloaded;
                oldValue.DownloadFailed -= bitmap.sourceFailed;

                // ((BitmapSource)newValue).DecodeFailed -= bitmap._sourceFailed; // 3.5
            }

            if (((newValue != null) && (newValue is BitmapSource)) && !newValue.IsFrozen)
            {
                newValue.DownloadCompleted += bitmap.sourceDownloaded;
                newValue.DownloadFailed += bitmap.sourceFailed;

                // ((BitmapSource)newValue).DecodeFailed += bitmap._sourceFailed; // 3.5
            }
        }

        /// <summary>
        /// Applies the visual transform.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="v">The v.</param>
        /// <param name="inverse">if set to <c>true</c> [inverse].</param>
        /// <returns>
        /// The transformed point.
        /// </returns>
        private Point ApplyVisualTransform(Point point, Visual v, bool inverse)
        {
            bool success;
            return this.TryApplyVisualTransform(point, v, inverse, true, out success);
        }

        /// <summary>
        /// Determines if two points are close.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// True if close.
        /// </returns>
        private bool AreClose(Point point1, Point point2)
        {
            return AreClose(point1.X, point2.X) && AreClose(point1.Y, point2.Y);
        }

        /// <summary>
        /// Determines of two values are close.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>
        /// True if close.
        /// </returns>
        private bool AreClose(double value1, double value2)
        {
            if (value1 == value2)
            {
                return true;
            }

            double delta = value1 - value2;
            return (delta < 1.53E-06) && (delta > -1.53E-06);
        }

        /// <summary>
        /// Gets the pixel offset.
        /// </summary>
        /// <returns>
        /// The pixel offset.
        /// </returns>
        private Point GetPixelOffset()
        {
            var pixelOffset = new Point();

            PresentationSource ps = PresentationSource.FromVisual(this);
            if (ps != null)
            {
                Visual rootVisual = ps.RootVisual;

                // Transform (0,0) from this element up to pixels.
                pixelOffset = TransformToAncestor(rootVisual).Transform(pixelOffset);
                pixelOffset = this.ApplyVisualTransform(pixelOffset, rootVisual, false);
                pixelOffset = ps.CompositionTarget.TransformToDevice.Transform(pixelOffset);

                // Round the origin to the nearest whole pixel.
                pixelOffset.X = Math.Round(pixelOffset.X);
                pixelOffset.Y = Math.Round(pixelOffset.Y);

                // Transform the whole-pixel back to this element.
                pixelOffset = ps.CompositionTarget.TransformFromDevice.Transform(pixelOffset);
                pixelOffset = this.ApplyVisualTransform(pixelOffset, rootVisual, true);
                pixelOffset = rootVisual.TransformToDescendant(this).Transform(pixelOffset);
            }

            return pixelOffset;
        }

        /// <summary>
        /// Gets the visual transform.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>
        /// The <see cref="Matrix" />.
        /// </returns>
        /// <remarks>Gets the matrix that will convert a point from "above" the
        /// coordinate space of a visual into the the coordinate space
        /// "below" the visual.</remarks>
        private Matrix GetVisualTransform(Visual v)
        {
            if (v != null)
            {
                Matrix m = Matrix.Identity;

                Transform transform = VisualTreeHelper.GetTransform(v);
                if (transform != null)
                {
                    Matrix cm = transform.Value;
                    m = Matrix.Multiply(m, cm);
                }

                Vector offset = VisualTreeHelper.GetOffset(v);
                m.Translate(offset.X, offset.Y);

                return m;
            }

            return Matrix.Identity;
        }

        /// <summary>
        /// Called when layout is updated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            // This event just means that layout happened somewhere.  However, this is
            // what we need since layout anywhere could affect our pixel positioning.
            Point pixelOffset = this.GetPixelOffset();
            if (!this.AreClose(pixelOffset, this.pixelOffset))
            {
                this.InvalidateVisual();
            }
        }

        /// <summary>
        /// Called when the source has downloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void OnSourceDownloaded(object sender, EventArgs e)
        {
            this.InvalidateMeasure();
            this.InvalidateVisual();
        }

        /// <summary>
        /// Called when source failed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Media.ExceptionEventArgs" /> instance containing the event data.</param>
        private void OnSourceFailed(object sender, ExceptionEventArgs e)
        {
            this.Source = null; // setting a local value seems scetchy...

            this.BitmapFailed(this, e);
        }

        /// <summary>
        /// Tries to apply the visual transform.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="v">The v.</param>
        /// <param name="inverse">if set to <c>true</c> [inverse].</param>
        /// <param name="throwOnError">if set to <c>true</c> [throw on error].</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <returns>
        /// The <see cref="Point" />.
        /// </returns>
        private Point TryApplyVisualTransform(Point point, Visual v, bool inverse, bool throwOnError, out bool success)
        {
            success = true;
            if (v != null)
            {
                Matrix visualTransform = this.GetVisualTransform(v);
                if (inverse)
                {
                    if (!throwOnError && !visualTransform.HasInverse)
                    {
                        success = false;
                        return new Point(0, 0);
                    }

                    visualTransform.Invert();
                }

                point = visualTransform.Transform(point);
            }

            return point;
        }
    }
}