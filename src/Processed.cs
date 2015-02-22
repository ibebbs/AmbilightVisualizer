using System.Windows.Media;

namespace Ambilight
{
    public class Processed
    {
        public Processed(Color[] top, Color[] left, Color[] right, Color[] bottom)
        {
            Top = top;
            Left = left;
            Right = right;
            Bottom = bottom;
        }

        public Color[] Top { get; private set; }
        public Color[] Left { get; private set; }
        public Color[] Right { get; private set; }
        public Color[] Bottom { get; private set; }
    }
}
