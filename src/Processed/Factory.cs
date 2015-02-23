using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Ambilight.Processed
{
    public interface IFactory
    {
        IData FromContent(string content);
    }

    internal class Factory : IFactory
    {
        private void TryParseLed(JObject led, Action<Color> addLed)
        {
            JToken r, g, b;

            if (led.TryGetValue("r", out r) && led.TryGetValue("g", out g) && led.TryGetValue("b", out b))
            {
                addLed(Color.FromRgb((byte)r, (byte)g, (byte)b));
            }
        }

        private void TryParseSide(JObject layer, string side, Action<IEnumerable<KeyValuePair<uint, Color>>> addLeds)
        {

            JToken token;

            if (layer.TryGetValue(side, out token) && token is JObject)
            {
                Dictionary<uint, Color> leds = new Dictionary<uint, Color>();
                uint index = 0;

                JToken pixel;

                while (((JObject)token).TryGetValue(index.ToString(), out pixel) && pixel is JObject)
                {
                    TryParseLed((JObject)pixel, led => leds.Add(index, led));
                    index++;
                }

                addLeds(leds);
            }
        }

        private ILayer ParseLayer(JObject json)
        {
            Layer layer = new Layer();

            TryParseSide(json, "left", leds => layer.Left = leds.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToArray());
            TryParseSide(json, "top", leds => layer.Top = leds.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToArray());
            TryParseSide(json, "right", leds => layer.Right = leds.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToArray());
            TryParseSide(json, "bottom", leds => layer.Bottom = leds.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToArray());

            return layer;
        }

        private void TryParseLayer(JObject root, string layerName, Action<ILayer> addLayer)
        {
            JToken token;

            if (root.TryGetValue(layerName, out token) && token is JObject)
            {
                addLayer(ParseLayer((JObject)token));
            }
        }

        public IData FromContent(string content)
        {
            JObject json = JObject.Parse(content);

            Data data = new Data();

            TryParseLayer(json, "layer1", layer => data.Layers = (data.Layers ?? Enumerable.Empty<ILayer>()).Concat(new[] { layer }).ToArray());

            return data;
        }
    }
}
