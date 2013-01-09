// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaptureScreenshot.cs" company="PropertyTools">
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
// <summary>
//   Captures a screenshot using gdi32 functions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Captures a screenshot using gdi32 functions.
    /// </summary>
    /// <remarks>
    /// See http://stackoverflow.com/questions/1736287/capturing-a-window-with-wpf
    /// </remarks>
    public static class CaptureScreenshot
    {
        /// <summary>
        /// The ternary raster operations.
        /// </summary>
        private enum TernaryRasterOperations : uint
        {
            /// <summary>
            /// dest = source
            /// </summary>
            SRCCOPY = 0x00CC0020,

            /// <summary>
            /// dest = source OR dest
            /// </summary>
            SRCPAINT = 0x00EE0086,

            /// <summary>
            /// dest = source AND dest
            /// </summary>
            SRCAND = 0x008800C6,

            /// <summary>
            /// dest = source XOR dest
            /// </summary>
            SRCINVERT = 0x00660046,

            /// <summary>
            /// dest = source AND (NOT dest)
            /// </summary>
            SRCERASE = 0x00440328,

            /// <summary>
            /// dest = (NOT source)
            /// </summary>
            NOTSRCCOPY = 0x00330008,

            /// <summary>
            /// dest = (NOT src) AND (NOT dest)
            /// </summary>
            NOTSRCERASE = 0x001100A6,

            /// <summary>
            /// dest = (source AND pattern)
            /// </summary>
            MERGECOPY = 0x00C000CA,

            /// <summary>
            /// dest = (NOT source) OR dest
            /// </summary>
            MERGEPAINT = 0x00BB0226,

            /// <summary>
            /// dest = pattern
            /// </summary>
            PATCOPY = 0x00F00021,

            /// <summary>
            /// dest = DPSnoo
            /// </summary>
            PATPAINT = 0x00FB0A09,

            /// <summary>
            /// dest = pattern XOR dest
            /// </summary>
            PATINVERT = 0x005A0049,

            /// <summary>
            /// dest = (NOT dest)
            /// </summary>
            DSTINVERT = 0x00550009,

            /// <summary>
            /// dest = BLACK
            /// </summary>
            BLACKNESS = 0x00000042,

            /// <summary>
            /// dest = WHITE
            /// </summary>
            WHITENESS = 0x00FF0062
        }

        /// <summary>
        /// Capture the screenshot.
        ///  <returns>
        /// Bitmap source that can be used e.g. as background.
        /// </returns>
        /// </summary>
        /// <param name="area">
        /// Area of screenshot.
        /// </param>
        public static BitmapSource Capture(Rect area)
        {
            IntPtr screenDC = GetDC(IntPtr.Zero);
            IntPtr memDC = CreateCompatibleDC(screenDC);
            IntPtr hBitmap = CreateCompatibleBitmap(screenDC, (int)area.Width, (int)area.Height);
            SelectObject(memDC, hBitmap); // Select bitmap from compatible bitmap to memDC

            // TODO: BitBlt may fail horribly
            BitBlt(
                memDC,
                0,
                0,
                (int)area.Width,
                (int)area.Height,
                screenDC,
                (int)area.X,
                (int)area.Y,
                TernaryRasterOperations.SRCCOPY);
            BitmapSource bsource = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(hBitmap);
            ReleaseDC(IntPtr.Zero, screenDC);
            ReleaseDC(IntPtr.Zero, memDC);
            return bsource;
        }

        /// <summary>
        /// The correct get position.
        /// </summary>
        /// <param name="relativeTo">
        /// The relative to.
        /// </param>
        /// <returns>
        /// The <see cref="Point"/>.
        /// </returns>
        public static Point CorrectGetPosition(Visual relativeTo)
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return relativeTo.PointFromScreen(new Point(w32Mouse.X, w32Mouse.Y));
        }

        /// <summary>
        /// The get mouse screen position.
        /// </summary>
        /// <returns>
        /// The <see cref="Point"/>.
        /// </returns>
        public static Point GetMouseScreenPosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        /// <summary>
        /// The get cursor pos.
        /// </summary>
        /// <param name="pt">
        /// The pt.
        /// </param>
        /// <returns>
        /// The get cursor pos.
        /// </returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        /// <summary>
        /// The bit blt.
        /// </summary>
        /// <param name="hdc">
        /// The hdc.
        /// </param>
        /// <param name="nXDest">
        /// The n x dest.
        /// </param>
        /// <param name="nYDest">
        /// The n y dest.
        /// </param>
        /// <param name="nWidth">
        /// The n width.
        /// </param>
        /// <param name="nHeight">
        /// The n height.
        /// </param>
        /// <param name="hdcSrc">
        /// The hdc src.
        /// </param>
        /// <param name="nXSrc">
        /// The n x src.
        /// </param>
        /// <param name="nYSrc">
        /// The n y src.
        /// </param>
        /// <param name="dwRop">
        /// The dw rop.
        /// </param>
        /// <returns>
        /// The bit blt.
        /// </returns>
        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(
            IntPtr hdc,
            int nXDest,
            int nYDest,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int nXSrc,
            int nYSrc,
            TernaryRasterOperations dwRop);

        /// <summary>
        /// The create bitmap.
        /// </summary>
        /// <param name="nWidth">
        /// The n width.
        /// </param>
        /// <param name="nHeight">
        /// The n height.
        /// </param>
        /// <param name="cPlanes">
        /// The c planes.
        /// </param>
        /// <param name="cBitsPerPel">
        /// The c bits per pel.
        /// </param>
        /// <param name="lpvBits">
        /// The lpv bits.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateBitmap(
            int nWidth, int nHeight, uint cPlanes, uint cBitsPerPel, IntPtr lpvBits);

        /// <summary>
        /// The create compatible bitmap.
        /// </summary>
        /// <param name="hdc">
        /// The hdc.
        /// </param>
        /// <param name="nWidth">
        /// The n width.
        /// </param>
        /// <param name="nHeight">
        /// The n height.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        /// <summary>
        /// The create compatible dc.
        /// </summary>
        /// <param name="hdc">
        /// The hdc.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        /// <summary>
        /// The delete object.
        /// </summary>
        /// <param name="hObject">
        /// The h object.
        /// </param>
        /// <returns>
        /// The delete object.
        /// </returns>
        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// The get dc.
        /// </summary>
        /// <param name="hWnd">
        /// The h wnd.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        /// The release dc.
        /// </summary>
        /// <param name="hWnd">
        /// The h wnd.
        /// </param>
        /// <param name="hDC">
        /// The h dc.
        /// </param>
        /// <returns>
        /// The release dc.
        /// </returns>
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// The select object.
        /// </summary>
        /// <param name="hdc">
        /// The hdc.
        /// </param>
        /// <param name="hgdiobj">
        /// The hgdiobj.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        /// <summary>
        /// The win 32 point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            /// <summary>
            /// The x.
            /// </summary>
            public int X;

            /// <summary>
            /// The y.
            /// </summary>
            public int Y;
        };
    }
}