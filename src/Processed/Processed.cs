using System.Collections.Generic;
using System.Windows.Media;

namespace Ambilight.Processed
{
    public interface ILayer
    {
        Color[] Top { get; }
        Color[] Left { get; }
        Color[] Right { get; }
        Color[] Bottom { get; }
    }

    public class Layer : ILayer
    {
        public Color[] Top { get; set; }
        public Color[] Left { get; set; }
        public Color[] Right { get; set; }
        public Color[] Bottom { get; set; }
    }

    public interface IData
    {
        IEnumerable<ILayer> Layers { get; }
    }

    public class Data : IData
    {
        public IEnumerable<ILayer> Layers { get; set; }
    }
}
