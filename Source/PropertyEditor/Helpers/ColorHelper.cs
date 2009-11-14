using System;
using System.Windows.Media;
using System.Globalization;

namespace OpenControls
{
    public static class ColorHelper
    {
        public static Color UndefinedColor = Color.FromArgb(0, 0, 0, 0);

        public static Color ChangeAlpha(Color c, byte alpha)
        {
            return Color.FromArgb(alpha, c.R, c.G, c.B);  
        }

        public static string ColorToHex(Color color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B);
        }

        public static double ColorDistance(Color c1, Color c2)
        {
            double dr = c1.R - c2.R;
            double dg = c1.G - c2.G;
            double db = c1.B - c2.B;
            double e = dr*dr + dg*dg + db*db;
            return Math.Sqrt(e);
        }

        public static double HueDistance(Color c1, Color c2)
        {
            var hsv1 = ColorToHsv(c1);
            var hsv2 = ColorToHsv(c2);
            double dh = (double)hsv1[0]-(double)hsv2[0];
            double e = dh*dh;
            return Math.Sqrt(e);
        }

        public static Color HexToColor(string value)
        {
            value = value.Trim('#');
            if (value.Length!=8)
                return UndefinedColor;

            uint u;
            if (uint.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out u))
                return UIntToColor(u);
            return UndefinedColor;
        }


        // todo: test this...
        public static Color UIntToColor(UInt32 color)
        {
            var a = (byte) (color >> 24);
            var r = (byte) (color >> 16);
            var g = (byte) (color >> 8);
            var b = (byte) (color >> 0);
            return Color.FromArgb(a, r, g, b);
        }

        // todo: test this...
        public static UInt32 ColorToUint(Color c)
        {
            UInt32 u = (UInt32) c.A << 24;
            u += (UInt32)c.R << 16;
            u += (UInt32)c.G << 8;
            u += (UInt32)c.B;
            return u;
            //(UInt32)((UInt32)c.A << 24 + (UInt32)c.R << 16 + (UInt32)c.G << 8 + (UInt32)c.B);
        }


        // Converts an RGB color to an HSV color.
        public static byte[] ColorToHsv(Color color)
        {
            byte r = color.R;
            byte g = color.G;
            byte b = color.B;

            double delta, min;
            double h = 0, s, v;

            min = Math.Min(Math.Min(r, g), b);
            v = Math.Max(Math.Max(r, g), b);
            delta = v - min;

            if (v == 0.0)
            {
                s = 0;
            }
            else
                s = delta/v;

            if (s == 0)
                h = 0.0;

            else
            {
                if (r == v)
                    h = (g - b)/delta;
                else if (g == v)
                    h = 2 + (b - r)/delta;
                else if (b == v)
                    h = 4 + (r - g)/delta;

                h *= 60;
                if (h < 0.0)
                    h = h + 360;
            }

            var hsv = new byte[3];
            hsv[0] = (byte) (h/360*255);
            hsv[1] = (byte) (s*255);
            hsv[2] = (byte) (v);
            return hsv;
        }

        // Converts an HSV color to an RGB color.
        public static Color HsvToColor(byte hue, byte saturation, byte value)
        {
            double r = 0, g = 0, b = 0;
            double h = hue*360.0/255;
            double s = saturation/255.0;
            double v = value/255.0;

            if (s == 0)
            {
                r = v;
                g = v;
                b = v;
            }
            else
            {
                int i;
                double f, p, q, t;


                if (h == 360)
                    h = 0;
                else
                    h = h/60;

                i = (int) Math.Truncate(h);
                f = h - i;

                p = v*(1.0 - s);
                q = v*(1.0 - (s*f));
                t = v*(1.0 - (s*(1.0 - f)));

                switch (i)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;

                    default:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }
            }


            return Color.FromArgb(255, (byte) (r*255), (byte) (g*255), (byte) (b*255));
        }

        /// <summary>
        /// Convert from HSV to Color
        /// http://en.wikipedia.org/wiki/HSL_color_space
        /// </summary>
        /// <param name="h">Hue [0,1]</param>
        /// <param name="s">Saturation [0,1]</param>
        /// <param name="v">Value [0,1]</param>
        /// <returns></returns>
        public static Color HsvToColor(double hue, double sat, double val)
        {
            int i;
            double aa, bb, cc, f;
            double r, g, b;
            r = g = b = 0;

            if (sat == 0) // Gray scale
                r = g = b = val;
            else
            {
                if (hue == 1.0) hue = 0;
                hue *= 6.0;
                i = (int) Math.Floor(hue);
                f = hue - i;
                aa = val*(1 - sat);
                bb = val*(1 - (sat*f));
                cc = val*(1 - (sat*(1 - f)));
                switch (i)
                {
                    case 0:
                        r = val;
                        g = cc;
                        b = aa;
                        break;
                    case 1:
                        r = bb;
                        g = val;
                        b = aa;
                        break;
                    case 2:
                        r = aa;
                        g = val;
                        b = cc;
                        break;
                    case 3:
                        r = aa;
                        g = bb;
                        b = val;
                        break;
                    case 4:
                        r = cc;
                        g = aa;
                        b = val;
                        break;
                    case 5:
                        r = val;
                        g = aa;
                        b = bb;
                        break;
                }
            }
            return Color.FromRgb((byte) (r*255), (byte) (g*255), (byte) (b*255));
        }
    }
}