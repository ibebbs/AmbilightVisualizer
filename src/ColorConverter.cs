using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Ambilight
{
    /// <summary>
    /// Adapted from here: https://github.com/kennyliou/GAI/blob/master/src/ColorConvertor.java
    /// </summary>
    public static class ColorConverter
    {
        public static LAB RgbToLab(int argb)
        {
            // this is the standard way to convert an int to argb value
            int r = (argb) & 0xFF;
            int g = (argb >> 8) & 0xFF;
            int b = (argb >> 16) & 0xFF;

            return RgbToLab(r, g, b);
        }

        public static LAB RgbToLab(int r, int g, int b)
        {
            // normalize RGB value
            double normalR = r / 255.0;
            double normalG = g / 255.0;
            double normalB = b / 255.0;

            // convert RGB to XYZ
            const double bottom = 1.055;
            const double refX = 95.047;
            const double refY = 100.000;
            const double refZ = 108.883;

            normalR = ((normalR > 0.04045) ? Math.Pow((normalR + 0.055) / bottom, 2.4) : normalR / 12.92) * 100.0;
            normalG = ((normalG > 0.04045) ? Math.Pow((normalG + 0.055) / bottom, 2.4) : normalG / 12.92) * 100.0;
            normalB = ((normalB > 0.04045) ? Math.Pow((normalB + 0.055) / bottom, 2.4) : normalB / 12.92) * 100.0;

            double x = normalR * 0.4124 + normalG * 0.3576 + normalB * 0.1805;
            double y = normalR * 0.2126 + normalG * 0.7152 + normalB * 0.0722;
            double z = normalR * 0.0193 + normalG * 0.1192 + normalB * 0.9505;
            
            double normalX = x / refX;
            double normalY = y / refY;
            double normalZ = z / refZ;

            normalX = (normalX > 0.008856) ? Math.Pow(normalX, 1 / 3) : (7.787 * normalX) + (16.0 / 116.0);
            normalY = (normalY > 0.008856) ? Math.Pow(normalY, 1 / 3) : (7.787 * normalY) + (16 / 116);
            normalZ = (normalZ > 0.008856) ? Math.Pow(normalZ, 1 / 3) : (7.787 * normalZ) + (16 / 116);

            double cieL = (116 * normalY) - 16;
            double cieA = 500 * (normalX - normalY);
            double cieB = 200 * (normalY - normalZ);

            // this is used to scale the value to 5
            return new LAB(cieL / 1.0, cieA / 1, cieB / 1);
        }

        public static Color Mix(this IEnumerable<Tuple<Color, double>> source)
        {
            source = (source ?? Enumerable.Empty<Tuple<Color, double>>()).ToArray();

            double total = source.Sum(tuple => tuple.Item2);
            double multiplier = 1.0 / total;

            var color = source.Aggregate(Tuple.Create(0.0,0.0,0.0), (seed, tuple) => Tuple.Create(
                seed.Item1 + (tuple.Item1.R * (tuple.Item2 * multiplier)),
                seed.Item2 + (tuple.Item1.G * (tuple.Item2 * multiplier)),
                seed.Item3 + (tuple.Item1.B * (tuple.Item2 * multiplier))
            ));

            return Color.FromRgb(Convert.ToByte(color.Item1), Convert.ToByte(color.Item2), Convert.ToByte(color.Item3));
        }
    }
}
