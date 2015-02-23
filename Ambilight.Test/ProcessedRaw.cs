using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambilight.Test
{
    public class LedRaw
    {
        [JsonProperty("r")]
        public byte Red { get; set; }

        [JsonProperty("g")]
        public byte Green { get; set; }

        [JsonProperty("b")]
        public byte Blue { get; set; }
    }

    public class SideRaw
    {
        public LedRaw Zero { get; set; }
        public LedRaw One { get; set; }
        public LedRaw Two { get; set; }
        public LedRaw Three { get; set; }
        public LedRaw Four { get; set; }
        public LedRaw Five { get; set; }
        public LedRaw Six { get; set; }
        public LedRaw Seven { get; set; }
        public LedRaw Eight { get; set; }
        public LedRaw Nine { get; set; }
    }

    public class LayerRaw
    {
        public SideRaw Left { get; set; }
        public SideRaw Top { get; set; }
        public SideRaw Right { get; set; }
        public SideRaw Bottom { get; set; }
    }

    public class ProcessedRaw
    {
        public LayerRaw Layer1 { get; set; }
        public LayerRaw Layer2 { get; set; }
        public LayerRaw Layer3 { get; set; }
        public LayerRaw Layer4 { get; set; }
        public LayerRaw Layer5 { get; set; }
    }
}
