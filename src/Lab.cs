
namespace Ambilight
{
    public class LAB
    {
        public LAB(double l, double a, double b)
        {
            L = l;
            A = a;
            B = b;
        }

        public double L { get; private set; }

        public double A { get; private set; }

        public double B { get; private set; }
    }
}
