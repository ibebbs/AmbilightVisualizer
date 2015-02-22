using System;
using System.Windows.Media;

namespace Ambilight
{
    public class ColorDifference
    {
        private const double Kl = 1;
        private const double K1 = 0.045;
        private const double K2 = 0.015;
        // double KL = 2;
        // double K1 = 0.048;
        // double K2 = 0.014;

        public static double FindDifference(Color color1, Color color2)
        {
            LAB lab1 = ColorConverter.RgbToLab(color1.R, color1.G, color1.B);
            LAB lab2 = ColorConverter.RgbToLab(color2.R, color2.G, color2.B);
            return FindDifference(lab1, lab2);
        }

        public static double FindDifference(int argb1, int argb2)
        {
            LAB lab1 = ColorConverter.RgbToLab(argb1);
            LAB lab2 = ColorConverter.RgbToLab(argb2);
            return FindDifference(lab1, lab2);
        }

        public static double FindDifference(int r1, int g1, int b1, int r2, int g2, int b2)
        {
            LAB lab1 = ColorConverter.RgbToLab(r1, g1, b1);
            LAB lab2 = ColorConverter.RgbToLab(r2, g2, b2);
            return FindDifference(lab1, lab2);
        }

        private static double FindDifference(LAB lab1, LAB lab2)
        {
            double dL = lab1.L - lab2.L;
            double c1 = Math.Sqrt((Math.Pow(lab1.A, 2) + Math.Pow(lab1.B, 2)));
            double c2 = Math.Sqrt((Math.Pow(lab2.A, 2) + Math.Pow(lab2.B, 2)));
            double dC = c1 - c2;
            double da = lab1.A - lab2.A;
            double db = lab1.B - lab2.B;
            double dH = Math.Sqrt((Math.Pow(da, 2) + Math.Pow(db, 2) - Math.Pow(dC, 2)));

            double sec1 = Math.Pow((dL / Kl), 2);
            double sec2 = Math.Pow((dC / (1 + (K1 * c1))), 2);
            double sec3 = Math.Pow((dH / (1 + (K2 * c1))), 2);

            return Math.Sqrt(sec1 + sec2 + sec3);
        }
    }
}
